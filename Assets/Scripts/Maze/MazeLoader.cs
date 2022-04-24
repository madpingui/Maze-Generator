using UnityEngine;
using System.Collections;

public class MazeLoader : MonoBehaviour {

	//Maze Stats
	public int mazeRows, mazeColumns;
	public float size = 6f;
	private Vector3 sizeScaler = new Vector3(1, 1, 0.2f);

	//Maze Components
	public GameObject wall;
	public GameObject floor;
	public Transform mazeContainer;
	private MazeCell[,] mazeCells;
	private Camera mainCamera;

	//AI
	private NavMeshBaker navMeshBaker;
	public GameObject agent;

    private void Awake()
    {
		navMeshBaker = FindObjectOfType<NavMeshBaker>();
		mainCamera = Camera.main;

		//Maze
		InitializeMaze();
		MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
		ma.CreateMaze();


		//AI
		Instantiate(agent, Vector3.zero, Quaternion.identity);
		//Has to have a delay so the maze have time to generate it self.
		Invoke("BakeNavMesh", 0.1f);


		//positionate the camera in the middle, and enough far up to see the whole board (in case of huge boards you need to change the far cliping plane)
		mainCamera.transform.position = new Vector3(((mazeRows * size) / 2) - (size / 2), mazeRows > mazeColumns ? mazeRows * (size * 1.5f) : mazeColumns * (size * 1.5f), ((mazeColumns * size) / 2) - (size / 2));
	}

	public void BakeNavMesh()
    {
		navMeshBaker.BakeNavMesh();	
	}

	//Creates a grid of walls (mazeRows X mazeColumns)
	private void InitializeMaze() {

		mazeCells = new MazeCell[mazeRows,mazeColumns];

		for (int r = 0; r < mazeRows; r++) {
			for (int c = 0; c < mazeColumns; c++) {
				mazeCells [r, c] = new MazeCell ();

				mazeCells [r, c] .floor = Instantiate (floor, new Vector3 (r*size, -(size/2f), c*size), Quaternion.identity, mazeContainer) as GameObject;
				mazeCells [r, c] .floor.name = "Floor " + r + "," + c;
				mazeCells [r, c] .floor.transform.Rotate (Vector3.right, 90f);
				mazeCells[r, c].floor.transform.localScale = sizeScaler * size;

				if (c == 0) {
					mazeCells[r,c].westWall = Instantiate (wall, new Vector3 (r*size, 0, (c*size) - (size/2f)), Quaternion.identity, mazeContainer) as GameObject;
					mazeCells [r, c].westWall.name = "West Wall " + r + "," + c;
					mazeCells[r, c].westWall.transform.localScale = sizeScaler * size;
				}

				mazeCells [r, c].eastWall = Instantiate (wall, new Vector3 (r*size, 0, (c*size) + (size/2f)), Quaternion.identity, mazeContainer) as GameObject;
				mazeCells [r, c].eastWall.name = "East Wall " + r + "," + c;
				mazeCells[r, c].eastWall.transform.localScale = sizeScaler * size;

				if (r == 0) {
					mazeCells [r, c].northWall = Instantiate (wall, new Vector3 ((r*size) - (size/2f), 0, c*size), Quaternion.identity, mazeContainer) as GameObject;
					mazeCells [r, c].northWall.name = "North Wall " + r + "," + c;
					mazeCells [r, c].northWall.transform.Rotate (Vector3.up * 90f);
					mazeCells[r, c].northWall.transform.localScale = sizeScaler * size;
				}

				mazeCells[r,c].southWall = Instantiate (wall, new Vector3 ((r*size) + (size/2f), 0, c*size), Quaternion.identity, mazeContainer) as GameObject;
				mazeCells [r, c].southWall.name = "South Wall " + r + "," + c;
				mazeCells [r, c].southWall.transform.Rotate (Vector3.up * 90f);
				mazeCells[r, c].southWall.transform.localScale = sizeScaler * size;
			}
		}
	}
}
