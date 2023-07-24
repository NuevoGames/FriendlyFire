using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private const int CodeLength = 3;


    [SerializeField] private byte maxPlayersPerRoom = 4;
    public TMP_InputField _roomCodeInputField;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI roomCodeDisplay;
    public GameObject _playerIconPrefab;
    [SerializeField] private Transform PlayerIconPanel;
    private Dictionary<string, GameObject> playerIconsObj = new Dictionary<string, GameObject>();
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject RoomLobby,CreatJoinRoomMenu, JoinRoomLobby;

    private void Start()
    {
       
    }

    private void UpdatePlayerCount()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;

        playerCountText.text = playerCount + "/" + maxPlayers;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();
        SpawnPlayerIcon(newPlayer.NickName);
        ActivateStartButton();

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();
        Destroy(playerIconsObj[otherPlayer.NickName]);
        ActivateStartButton();

    }

    public void ActivateStartButton() {
        if (startGameButton != null && PlayerManager.Instance.playersReady > 1)
        {
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
            Debug.Log("START BUTTON CALLED");

        }
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayersPerRoom
            
        };

        PhotonNetwork.CreateRoom(GenerateRoomCode(), roomOptions, TypedLobby.Default);
        HandleError.Instance.ErrorHandler("Creating a room....");
        

    }
    public override void OnCreatedRoom()
    {
        
    }
    public void JoinRoom(string roomCode)
    {
        roomCode = _roomCodeInputField.text;
        if (!string.IsNullOrEmpty(roomCode))
        {
            PhotonNetwork.JoinRoom(roomCode.ToUpper());
            HandleError.Instance.ErrorHandler("Joining a room....");
        }
    }

    public void LeaveRoom()
    {
        foreach (var item in playerIconsObj.Values)
        {
            Destroy(item.gameObject);
        }
        playerIconsObj.Clear();
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("LeftRoom");
        RoomLobby.SetActive(false);
        CreatJoinRoomMenu.SetActive(true);

    }


    private string GenerateRoomCode()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";

        string code = string.Empty;
        System.Random random = new System.Random();

        // Generate four random digits
        for (int i = 0; i < CodeLength; i++)
        {
            code += digits[random.Next(digits.Length)];
        }

        // Append a random letter
        code += letters[random.Next(letters.Length)];

        return code;
    }


    

    private void SpawnPlayerIcon(string newPlayer = "")
    {
        
        if (string.IsNullOrEmpty(newPlayer))
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {

                GameObject go = Instantiate(_playerIconPrefab, PlayerIconPanel);
                go.GetComponentInChildren<TextMeshProUGUI>().text = player.NickName;
                playerIconsObj[player.NickName] = go;

            }
        }
        else
        {
            GameObject go = Instantiate(_playerIconPrefab, PlayerIconPanel);
            go.GetComponentInChildren<TextMeshProUGUI>().text = newPlayer;
            playerIconsObj[newPlayer] = go;
        }
       

    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.JoinLobby();
        if (JoinRoomLobby != null) JoinRoomLobby.SetActive(false);
        RoomLobby.SetActive(true);
        
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;
        SpawnPlayerIcon();
        UpdatePlayerCount();
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        roomCodeDisplay.text = PhotonNetwork.CurrentRoom.Name;
    }

  

}
