using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool IsMenuOpened = false;
    public GameObject menuUI;

    //public GameObject healthCrosshair;
    public GameObject scoreUI;
    //public GameObject scopeUI;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && IsMenuOpened == false)
        {
            scoreUI.SetActive(false);
            menuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            IsMenuOpened = true;
            AudioListener.pause = true; // audio band kar denge
        }

        else if(Input.GetKeyDown(KeyCode.Escape) && IsMenuOpened == true)
        {
            scoreUI.SetActive(true);
            menuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            IsMenuOpened = false;
            AudioListener.pause = false;
        }
    }

    public void LeaveGame()
    {
        Debug.Log("Game Left!");
        Application.Quit();
    }
}
