using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public void Stop()
    {
        pausePanel.SetActive(true);
    }
    public void Continue()
    {
        pausePanel.SetActive(false);
    }
}
