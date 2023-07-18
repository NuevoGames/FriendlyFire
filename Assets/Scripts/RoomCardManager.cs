using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class RoomCardManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private GameObject roomCardPrefab;

    private Dictionary<string, RoomCard> roomCards = new Dictionary<string, RoomCard>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Remove inactive room cards
        foreach (string roomId in new List<string>(roomCards.Keys))
        {
            if (!roomList.Exists(room => room.Name == roomId))
            {
                DestroyRoomCard(roomId);
            }
        }

        // Add or update room cards
        foreach (RoomInfo room in roomList)
        {
            if (room.IsOpen && room.IsVisible)
            {
                if (!roomCards.ContainsKey(room.Name))
                {
                    CreateRoomCard(room);
                }
                else
                {
                    UpdateRoomCard(room);
                }
            }
        }
    }

    private void CreateRoomCard(RoomInfo roomInfo)
    {
        GameObject roomCardObject = Instantiate(roomCardPrefab, cardParent);
        RoomCard roomCard = roomCardObject.GetComponent<RoomCard>();
        roomCard.Initialize(roomInfo);
        roomCards.Add(roomInfo.Name, roomCard);
    }

    private void UpdateRoomCard(RoomInfo roomInfo)
    {
        if (roomCards.TryGetValue(roomInfo.Name, out RoomCard roomCard))
        {
            roomCard.Initialize(roomInfo);
        }
    }

    private void DestroyRoomCard(string roomId)
    {
        if (roomCards.TryGetValue(roomId, out RoomCard roomCard))
        {
            roomCards.Remove(roomId);
            roomCard.DestroyCard();
        }
    }
}
