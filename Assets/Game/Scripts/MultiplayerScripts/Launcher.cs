using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    [SerializeField]
    private InputField roomNameInputField;
    [SerializeField]
    private Text roomNameText;
    [SerializeField]
    private Text errorText;

    [SerializeField]
    Transform roomListContent;

    [SerializeField]
    GameObject roomListItemPrefab;

    [SerializeField]
    Transform playerListContent;

    [SerializeField]
    GameObject playerListItemPrefab;

    public GameObject startButton;

    int nextTeamNumber = 1;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // once the player joins the lobby
    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("UserNameMenu");
        Debug.Log("Joined Lobby");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text)) // we are checking if the input field is not null or empty, if it is then we'll return otherwise we'll create room
        {
            return;
        }

        // jo bhi room name user input field m enter karega..usi naam ka room create karenge ..photon ke CreateRoom() funciton ki madad se
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.instance.OpenMenu("LoadingMenu"); // room create karne ke baad loading menu show karenge
    }

    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("RoomMenu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name; // jo bhi photonNetwork ke according room name(matlab jis naam ka room create kiya player ne) hai..usko text field m likh denge

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < players.Count(); i++)
        {
            int teamNumber = GetNextTeamNumber();

            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i], teamNumber);
        }

        startButton.SetActive(PhotonNetwork.IsMasterClient); // StartButton will be shown only to the player that has created the room(owner, master)
    }

    // agar master(game start krne wala) game leave kr deta hai toh hum startButton doosre player ko assign kr denge
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Generation Unsuccessful" + message;
        MenuManager.instance.OpenMenu("ErrorMenu");
    }

    // JoinRoom method built-in photon method nhi hai
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("LoadingMenu");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1); // in the build settings, scene "Game" is labelled as 1..so we want to load that level when the game starts
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("LoadingMenu");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("TitleMenu");
    }

    // OnRoomListUpdate is a built-in function that comes with photon..with this 
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for(int i = 0; i < roomList.Count; i++)
        {
            // agar room leave kar diya ho matlab koi uss room m na ho..aur woh room "Find Room" m show na ho isliye hum aage instantiate na krke continue krna chahenge
            if(roomList[i].RemovedFromList) // RemovedFromList is in-built in photon pun
            {
                continue;
            }

            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int teamNumber = GetNextTeamNumber();

        GameObject playerItem = Instantiate(playerListItemPrefab, playerListContent);
        playerItem.GetComponent<PlayerListItem>().SetUp(newPlayer, teamNumber);
    }

    private int GetNextTeamNumber()
    {
        int teamNumber = nextTeamNumber;
        nextTeamNumber = 3 - nextTeamNumber;
        return teamNumber;
    }
}
