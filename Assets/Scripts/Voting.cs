using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using System.Linq;

public class Voting : MonoBehaviour
{
    
    // Dictionary to store player names and their votes
    private Dictionary<string, int> votes = new Dictionary<string, int>();

    private void Start()
    {
        InitializePlayerVotes();
    }
    public void InitializePlayerVotes()
    {
        var list = PlayerManager.Instance.playerList;
        foreach (string player in list)
        {
            // Assuming that the player's name is stored in the NickName property
          
            int initialVoteCount = 0; // Set an initial vote count for each player (0 in this example)

            // Add the player's name to the dictionary with the initial vote count
            votes[player] = initialVoteCount;
        }
    }
    // Function to be called when a player votes
    public void VoteForPlayer(string playerName)
    {
        PhotonView photonView = PhotonView.Get(this);
        if (photonView.IsMine)
        {
            photonView.RPC("UpdateVoteCount", RpcTarget.All, playerName);
        }
    }


    [PunRPC]
    private void UpdateVoteCount(string playerName)
    {
        int totalVoteCount = 0;
        if (votes.ContainsKey(playerName))
        {
            votes[playerName]++;
        }
        else
        {
            votes[playerName] = votes[playerName];
        }

        foreach (var voteKey in votes.Keys) {
            totalVoteCount += votes[voteKey];
            Debug.Log("Total Vote count:" + totalVoteCount);
            Debug.Log("Total Players:" + PhotonNetwork.PlayerList.Length);

        }
        if (totalVoteCount == PhotonNetwork.PlayerList.Length) {
            Debug.Log("Everyone has voted");
            GetWinner();
            GameManager.Instance.GetComponent<Timer>().CancelCountdown();

        }
    }
    // Function to get the player with the most votes
    public void GetWinner()
    {
        string winner = "";
        int maxVotes = 0;

        // Loop through the votes dictionary to find the player with the most votes
        foreach (var kvp in votes)
        {
            if (kvp.Value > maxVotes)
            {
                maxVotes = kvp.Value;
                winner = kvp.Key;
            }
        }
        //Debug.Log(winner);
        GameManager.Instance.GoToNextRound();
        
      
    }

    // Function to reset the voting process
    public void ResetVotes()
    {
        foreach (var key in votes.Keys.ToList())
        {
            votes[key] = 0;
            
        }   
        //Debug.Log("Votes have been reset!");

        // You can also update the UI to show that the votes have been reset
    }
}
