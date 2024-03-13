using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UsernameDisplay : MonoBehaviour
{
    public Text usernameText;

    public Text teamText;

    public PhotonView view;

    private void Start()
    {
        if(view.IsMine)
        {
            // if view is ours then we dont want to display the username of our player on its head rather we want other players to able to see the name of our player
            gameObject.SetActive(false);
        }

        usernameText.text = view.Owner.NickName;

        // show the team number
        if(view.Owner.CustomProperties.ContainsKey("Team"))
        {
            int team = (int)view.Owner.CustomProperties["Team"];
            teamText.text = "Team " + team;
        }
    }
}
