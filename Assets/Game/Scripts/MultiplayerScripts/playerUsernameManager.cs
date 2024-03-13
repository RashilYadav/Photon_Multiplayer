using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class playerUsernameManager : MonoBehaviour
{
    [SerializeField]
    private InputField usernameInput;

    [SerializeField]
    private Text errorMessageText;

    private void Start()
    {
        if(PlayerPrefs.HasKey("username"))
        {
            usernameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
    }

    public void playerUsernameInputValueChanged()
    {
        string username = usernameInput.text;

        if (!string.IsNullOrEmpty(username) && username.Length <= 20){
            PhotonNetwork.NickName = username;
            // the username of the player will be stored in playerprefs so that when the player will play the game again, they would not need to enter the name again
            PlayerPrefs.SetString("username", username);
            errorMessageText.text = "";
            MenuManager.instance.OpenMenu("TitleMenu");
        }
        else
        {
            errorMessageText.text = "Username must not be empty and should be 20 characters or less.";
        }
    }
}
