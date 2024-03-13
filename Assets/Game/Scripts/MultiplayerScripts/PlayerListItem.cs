using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    public Text playerUsername;

    public Text teamText;

    Player player;

    int team;

    public void SetUp(Player _player, int _team) // yaha jo argument m Player ko refer kiya hua hai woh Photon.realtime ka player hai...naa ki script ko refer kr rhe
    {
        player = _player;
        team = _team;
        playerUsername.text = _player.NickName;
        teamText.text = "Team " + _team;

        // we created this custom property because without this we will see different team name for different player's side
        ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
        customProps["Team"] = _team; // in custom property for this one, "Team" is the key and "_team" is the value
        _player.SetCustomProperties(customProps);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer)
        {
            Destroy(gameObject); // if player leaves the room, we are going to destory it
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
