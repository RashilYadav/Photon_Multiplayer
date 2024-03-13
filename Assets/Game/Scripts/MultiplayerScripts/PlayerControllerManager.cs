using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using UnityEngine.UI;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerControllerManager : MonoBehaviourPunCallbacks
{
    PhotonView view;
    GameObject controller;

    public int playerTeam;

    private Dictionary<int, int> playerTeams = new Dictionary<int, int>();

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(view.IsMine) // if the player is ours, we are going to spawn the player prefab
        {
            CreateController();
        }
    }

    void CreateController()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            playerTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            Debug.Log("Player's Team: " + playerTeam);
        }

        AssignPlayerToSpawnArea(playerTeam);
    }

    // iss function mai hum team ke hisaab se spawn area allocate kar rhe hain..team1 ko "SpawnArea1" aur team 2 ko "SpawnArea2"
    void AssignPlayerToSpawnArea(int team)
    {

        GameObject spawnArea1 = GameObject.Find("SpawnArea1");
        GameObject spawnArea2 = GameObject.Find("SpawnArea2");

        if(spawnArea1 == null || spawnArea2 == null)
        {
            Debug.LogError("SpawnArea not found!");
            return;
        }

        Transform spawnPoint = null;

        // if the team of the player is 1 then we will spawn it to "SpawnArea1" at any random point from the 4 points that we mentioned in the SpawnArea1
        if(team == 1)
        {
            spawnPoint = spawnArea1.transform.GetChild(Random.Range(0, spawnArea1.transform.childCount));
        }

        if (team == 2)
        {
            spawnPoint = spawnArea2.transform.GetChild(Random.Range(0, spawnArea2.transform.childCount));
        }

        if(spawnPoint != null)
        {
            // Attach a PhotonView component to the prefab or GameObject that you want to instantiate across the network. 
            // photon m instantiate karwane ke liye "PhotonView" component ka hona zaroori hai...PhotonView component hum kisi gameObject pe lagata hain
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { view.ViewID });
            Debug.Log("Instantiated player controller at spawn point!");
        }
        else
        {
            Debug.LogError("No available spawn points for team " + team);
        }
    }

    void AssignTeamsToAllPlayers()
    {
        foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Team"))
            {
                int team = (int)player.CustomProperties["Team"];
                playerTeams[player.ActorNumber] = team;
                Debug.Log(player.NickName + "'s Team: " + team);

                // Call AssignPlayerToSpawnArea for each player when they join
                AssignPlayerToSpawnArea(team);
            }
        }
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller); // we are using this, because we want to destroy the player throughout the network
        Debug.Log("A player Died!");
        CreateController(); // calling the method again so that after the player is destroyed, it is spawned in the game again
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AssignTeamsToAllPlayers();
    }
}
