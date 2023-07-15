using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : Singleton<GameStateController>
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    public void EndGame (bool value)
    {
        if (value)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
    }

    public void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
