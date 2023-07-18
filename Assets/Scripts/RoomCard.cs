using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class RoomCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;

    private RoomInfo roomInfo;

    public void Initialize(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;
        UpdateRoomInfo();
    }

    private void UpdateRoomInfo()
    {
        roomNameText.text = roomInfo.Name;
        playerCountText.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
    }

    public void DestroyCard()
    {
        Destroy(gameObject);
    }
}
