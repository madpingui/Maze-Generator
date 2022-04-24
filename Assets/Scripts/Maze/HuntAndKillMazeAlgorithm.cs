using UnityEngine;
using System.Collections;

public class HuntAndKillMazeAlgorithm : MazeAlgorithm {

	private int currentRow = 0;
	private int currentColumn = 0;

	private bool courseComplete = false;

	private bool hasEntrance = false;
	private bool hasExit = false;

	public HuntAndKillMazeAlgorithm(MazeCell[,] mazeCells) : base(mazeCells) {
	}

	public override void CreateMaze () {
		HuntAndKill ();
	}

	private void HuntAndKill() {
		mazeCells [currentRow, currentColumn].visited = true;

		while (! courseComplete) {
			Kill(); // Will run until it hits a dead end.
			Hunt(); // Finds the next unvisited cell with an adjacent visited cell. If it can't find any, it sets courseComplete to true.
		}

        if (!hasExit) //if after all the generation of the maze there is still no exit, make the exit in the opposite point of the entrance
        {
			currentRow = mazeRows - 1;
			currentColumn = mazeColumns - 1;
			MakeExit();
        }
	}

	private void Kill() {
		while (RouteStillAvailable (currentRow, currentColumn)) {

			int direction = Random.Range(1,5);

			//this only happens once
			MakeEntrance();

			if (direction == 1 && CellIsAvailable (currentRow - 1, currentColumn)) {
				// North
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn].northWall);
				DestroyWallIfItExists (mazeCells [currentRow - 1, currentColumn].southWall);
				currentRow--;
			} else if (direction == 2 && CellIsAvailable (currentRow + 1, currentColumn)) {
				// South
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn].southWall);
				DestroyWallIfItExists (mazeCells [currentRow + 1, currentColumn].northWall);
				currentRow++;
			} else if (direction == 3 && CellIsAvailable (currentRow, currentColumn + 1)) {
				// east
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn].eastWall);
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn + 1].westWall);
				currentColumn++;
			} else if (direction == 4 && CellIsAvailable (currentRow, currentColumn - 1)) {
				// west
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn].westWall);
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn - 1].eastWall);
				currentColumn--;
			}

			mazeCells [currentRow, currentColumn].visited = true;
		}

		if(currentRow + 1 >= mazeRows / 2 && currentColumn + 1 >= mazeColumns / 2) //So the exit is not spawned close to the entrance
			MakeExit();//this only happens once

	}

	private void Hunt() {
		courseComplete = true; // Set it to this, and see if we can prove otherwise below!

		for (int r = 0; r < mazeRows; r++) {
			for (int c = 0; c < mazeColumns; c++) {
				if (!mazeCells [r, c].visited && CellHasAnAdjacentVisitedCell(r,c)) {
					courseComplete = false; // Yep, we found something so definitely do another Kill cycle.
					currentRow = r;
					currentColumn = c;
					DestroyAdjacentWall (currentRow, currentColumn);
					mazeCells [currentRow, currentColumn].visited = true;
					return; // Exit the function
				}
			}
		}
	}


	private bool RouteStillAvailable(int row, int column) {
		int availableRoutes = 0;

		if (row > 0 && !mazeCells[row-1,column].visited) {
			availableRoutes++;
		}

		if (row < mazeRows - 1 && !mazeCells [row + 1, column].visited) {
			availableRoutes++;
		}

		if (column > 0 && !mazeCells[row,column-1].visited) {
			availableRoutes++;
		}

		if (column < mazeColumns-1 && !mazeCells[row,column+1].visited) {
			availableRoutes++;
		}

		return availableRoutes > 0;
	}

	//Check if the cell which the direction was pointing is valid and is not out of the board.
	private bool CellIsAvailable(int row, int column) {
		if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeCells [row, column].visited) {
			return true;
		} else {
			return false;
		}
	}

	private bool CellIsOutside(int row, int column)
	{
		if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void DestroyWallIfItExists(GameObject wall) {
		if (wall != null) {
			GameObject.Destroy (wall);
		}
	}

	private bool CellHasAnAdjacentVisitedCell(int row, int column) {
		int visitedCells = 0;

		// Look 1 row up (north) if we're on row 1 or greater
		if (row > 0 && mazeCells [row - 1, column].visited) {
			visitedCells++;
		}

		// Look one row down (south) if we're the second-to-last row (or less)
		if (row < (mazeRows-2) && mazeCells [row + 1, column].visited) {
			visitedCells++;
		}

		// Look one row left (west) if we're column 1 or greater
		if (column > 0 && mazeCells [row, column - 1].visited) {
			visitedCells++;
		}

		// Look one row right (east) if we're the second-to-last column (or less)
		if (column < (mazeColumns-2) && mazeCells [row, column + 1].visited) {
			visitedCells++;
		}

		// return true if there are any adjacent visited cells to this one
		return visitedCells > 0;
	}

	private void DestroyAdjacentWall(int row, int column) {
		bool wallDestroyed = false;

		while (!wallDestroyed) {
			// int direction = Random.Range (1, 5);
			int direction = Random.Range(1, 5);

			if (direction == 1 && row > 0 && mazeCells [row - 1, column].visited) {
				DestroyWallIfItExists (mazeCells [row, column].northWall);
				DestroyWallIfItExists (mazeCells [row - 1, column].southWall);
				wallDestroyed = true;
			} else if (direction == 2 && row < (mazeRows-2) && mazeCells [row + 1, column].visited) {
				DestroyWallIfItExists (mazeCells [row, column].southWall);
				DestroyWallIfItExists (mazeCells [row + 1, column].northWall);
				wallDestroyed = true;
			} else if (direction == 3 && column > 0 && mazeCells [row, column-1].visited) {
				DestroyWallIfItExists (mazeCells [row, column].westWall);
				DestroyWallIfItExists (mazeCells [row, column-1].eastWall);
				wallDestroyed = true;
			} else if (direction == 4 && column < (mazeColumns-2) && mazeCells [row, column+1].visited) {
				DestroyWallIfItExists (mazeCells [row, column].eastWall);
				DestroyWallIfItExists (mazeCells [row, column+1].westWall);
				wallDestroyed = true;
			}
		}

	}

	private void MakeEntrance()
    {
		if (!hasEntrance)
		{
			int randomWall = Random.Range(0, 2);

			switch (randomWall)
			{
				case 0:
					DestroyWallIfItExists(mazeCells[currentRow, currentColumn].northWall);
					break;
				case 1:
					DestroyWallIfItExists(mazeCells[currentRow, currentColumn].westWall);
					break;
			}

			hasEntrance = true;
		}
	}

	private void MakeExit()
    {
		bool direction1, direction2, direction3, direction4;
		direction1 = direction2 = direction3 = direction4 = false;

		while (!hasExit)
        {
			int direction = Random.Range(1, 5);

			if (direction == 1 && !CellIsOutside(currentRow - 1, currentColumn))
			{
				// North
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].northWall);
				mazeCells[currentRow, currentColumn].floor.tag = "Finish";
				mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size = new Vector3(mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.x,
																											mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.y,
																											mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.z * 1.2f);
				hasExit = true;
				break;
			}
			else if (direction == 2 && !CellIsOutside(currentRow + 1, currentColumn))
			{
				// South
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].southWall);
				mazeCells[currentRow, currentColumn].floor.tag = "Finish";
				mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size = new Vector3(mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.x,
																											mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.y,
																											mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.z * 1.2f);
				hasExit = true;
				break;
			}
			else if (direction == 3 && !CellIsOutside(currentRow, currentColumn + 1))
			{
				// east
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].eastWall);
				mazeCells[currentRow, currentColumn].floor.tag = "Finish";
				mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size = new Vector3(mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.x,
																											mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.y ,
																											mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.z * 1.2f);
				hasExit = true;
				break;
			}
			else if (direction == 4 && !CellIsOutside(currentRow, currentColumn - 1))
			{
				// west
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].westWall);
				mazeCells[currentRow, currentColumn].floor.tag = "Finish";
				mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size = new Vector3(mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.x, 
																											mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.y, 
																											mazeCells[currentRow, currentColumn].floor.GetComponent<BoxCollider>().size.z * 1.2f);
				hasExit = true;
				break;
			}

            switch (direction)
            {
				case 1:
					direction1 = true;
					break;
				case 2:
					direction2 = true;
					break;
				case 3:
					direction3 = true;
					break;
				case 4:
					direction4 = true;
					break;
			}

			if (direction1 == true && direction2 == true && direction3 == true && direction4 == true)
				break;
		}
	}

}
