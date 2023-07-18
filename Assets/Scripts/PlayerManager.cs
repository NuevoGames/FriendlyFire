using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TextMeshProUGUI playerListText;
    [SerializeField] private List<Player> playerList = new List<Player>();

    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private GameObject readyButton;


    private int playersReady = 0;
    private bool gameStarted = false;


    private const string PlayerNameKey = "PlayerName";

   

    private void Start()
    {
        LoadPlayerName();
    }

    private void LoadPlayerName()
    {
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            string playerName = PlayerPrefs.GetString(PlayerNameKey);
            nameInputField.text = playerName;
            PhotonNetwork.NickName = playerName;
        }
    }
    private void Update()
    {
        OnNameEntered(nameInputField.text);


    }

    private void OnNameEntered(string playerName)
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(PlayerNameKey, playerName);
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "PlayerName", playerName } });
        }
    }

    public override void OnJoinedRoom()
    {
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        playerList.Clear();
        playerList.AddRange(PhotonNetwork.PlayerList);

        string playerListString = "Player List:\n";

        foreach (Player player in playerList)
        {
            string playerName = player.NickName;
            playerListString += playerName + "\n";
        }

        playerListText.text = playerListString;
        Debug.Log(playerList);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();

        if (gameStarted)
        {
            return;
        }

   
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void OnReadyButtonClicked()
    {
        if (!gameStarted && PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("IncreasePlayersReady", RpcTarget.MasterClient);
            readyButton.SetActive(false);
        }
    }

    [PunRPC]
    private void IncreasePlayersReady()
    {
        Ready();
    }

    public void Ready()
    {
        playersReady++;
    }

    public void StartGame()
    {
        if (gameStarted)
        {
            return;
        }

        if (PhotonNetwork.IsMasterClient && playersReady == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            gameStarted = true;
            PhotonNetwork.LoadLevel(gameSceneName);
        }

        
       
    }
}
