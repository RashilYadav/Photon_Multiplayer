using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;
    public Text blueTeamText;
    public Text redTeamText;

    public int blueTeamScore = 0;
    public int redTeamScore = 0;

    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        instance = this;
    }

    public void PlayerDied(int playerTeam)
    {
        // team 2 matlab "red team" ka koi member marta hai toh hum "blue Team" matlab team 1 ka score by 1 increase karenge!
        if(playerTeam == 2)
        {
            blueTeamScore++;
        }

        if (playerTeam == 1)
        {
            redTeamScore++;
        }
        view.RPC("UpdateScores", RpcTarget.All, blueTeamScore, redTeamScore);
    }

    [PunRPC]

    private void UpdateScores(int blueScore, int redScore) // UpdateScores function PunRPC m isliye likha taaki yeh jo score update hoga woh poore game m ache se hoga aur sabko dikhega
    {
        blueTeamScore = blueScore;
        redTeamScore = redScore;

        blueTeamText.text = blueTeamScore.ToString();
        redTeamText.text = redTeamScore.ToString();
    }
}
