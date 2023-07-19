using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI recievedQuestion;
    [SerializeField] private Button playerVoteButtonPrefab;
    [SerializeField] private Transform votingTab;
    private QuestionGenerator questionGenerator;
    PhotonView photonView;
    public int maxRounds = 5; // Maximum number of rounds for the game

    [SerializeField] private TextMeshProUGUI roundText;
    private int currentRound = 1; // Start with round 1

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView= PhotonView.Get(this);
        questionGenerator = GetComponent<QuestionGenerator>();
        Invoke("SendStringOverNetwork", 2f);

        if (PhotonNetwork.IsMasterClient)
        {
            // Only the master client should set the initial round count
            currentRound = 1;
            photonView.RPC("SyncRoundCount", RpcTarget.All, currentRound);
        }

        UpdateRoundText();
    }


    private void UpdateRoundText()
    {
        // Update the round count text (assuming you have a TextMeshProUGUI attached)
        roundText.text = "Round: " + currentRound + "/" + maxRounds;
    }

    // Function to advance to the next round
    public void GoToNextRound()
    {
        if (currentRound < maxRounds)
        {
            currentRound++;
            photonView.RPC("SyncRoundCount", RpcTarget.All, currentRound);
            UpdateRoundText();
        }
    }

    [PunRPC]
    private void SyncRoundCount(int round)
    {
        // This RPC updates the round count for all clients
        currentRound = round;
        UpdateRoundText();
    }

    public void SendStringOverNetwork()
    {
        if (PhotonNetwork.IsMasterClient)
        {
           
            string question = questionGenerator.RandomQuestions().question;
            photonView.RPC("ReceiveString", RpcTarget.AllBuffered, question);
        }
       
    }
    
    [PunRPC]
    private void ReceiveString(string receivedMessage)
    {
        Debug.Log("Received message: " + receivedMessage);
        recievedQuestion.text = receivedMessage;
        // Do something with the received message

        GeneratePlayerButtons();
    }


    private void GeneratePlayerButtons()
    {
        var list = PlayerManager.Instance.playerList;

        foreach (var playerName in list)
        {
            var tempButton = Instantiate(playerVoteButtonPrefab, votingTab);
            var child = tempButton.transform.GetChild(0);
            child.GetComponent<TextMeshProUGUI>().text = playerName; 
        }
    }
}

