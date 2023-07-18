using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private const int CodeLength = 4;

    [SerializeField] private byte maxPlayersPerRoom = 4;
    public TMP_InputField _roomCodeInputField;
    [SerializeField] private TextMeshProUGUI playerCountText;


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
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayersPerRoom
        };

        PhotonNetwork.CreateRoom(GenerateRoomCode(), roomOptions, TypedLobby.Default);
    }

    public void JoinRoom(string roomCode)
    {
        roomCode = _roomCodeInputField.text;
        if (!string.IsNullOrEmpty(roomCode))
        {
            PhotonNetwork.JoinRoom(roomCode.ToUpper());
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnLeaveRoomButtonClicked()
    {
        LeaveRoom();
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


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room. Error: " + message);
    }

    public override void OnJoinedRoom()
    {
        UpdatePlayerCount();
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room. Error: " + message);
    }

}
