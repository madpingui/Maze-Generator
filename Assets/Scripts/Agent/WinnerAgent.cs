using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerAgent : MonoBehaviour
{
    public GameObject winnerText;
    public GameObject winnerButton;
    public GameObject winnerBackground;

    private Agent agent;

    private void Start()
    {
        agent = FindObjectOfType<Agent>();
        agent.Winner += Win;
    }

    public void Win()
    {
        winnerText.SetActive(true);
        winnerButton.SetActive(true);
        winnerBackground.SetActive(true);
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }
}
