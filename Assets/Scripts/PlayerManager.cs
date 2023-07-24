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
    public static PlayerManager Instance { get; private set; }
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TextMeshProUGUI playerListText;
    public List<string> playerList = new List<string>();

    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private GameObject readyButton;


    [SerializeField]public int playersReady = 0;
    private bool gameStarted = false;


    private const string PlayerNameKey = "PlayerName";


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
        if (PhotonNetwork.IsMasterClient)
        {
            playersReady++;
        }
        else
        {
            readyButton.SetActive(true);
        }

    }

    private void UpdatePlayerList()
    {
        playerList.Clear();
       

        string playerListString = "Player List:\n";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            string playerName = player.NickName;
            playerList.Add(playerName);
            playerListString += playerName + "\n";
        }

        playerListText.text = playerListString;
        //Debug.Log(playerList);
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

    [PunRPC]
    private void IncreasePlayersReady()
    {
        playersReady++;
       
    }


    public void OnReadyButtonClicked()
    {
        if (!gameStarted && PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
        {

            photonView.RPC("IncreasePlayersReady", RpcTarget.MasterClient);
            readyButton.SetActive(false);
        }
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
