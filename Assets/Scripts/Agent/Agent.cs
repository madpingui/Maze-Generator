using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    private Camera mainCamera;
    private NavMeshAgent player;

    private bool hasWon;

    public delegate void WinEvent();
    public event WinEvent Winner;

    private void Awake()
    {
        mainCamera = Camera.main;
        player = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasWon)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                player.SetDestination(hit.point);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Finish")
        {
            hasWon = true;
            Winner?.Invoke();
        }
    }
}
