
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using System.Collections;



public class HandleError : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private static HandleError instance;
    [SerializeField] public TextMeshProUGUI errorTextDisplay;
    [SerializeField] private string message = "";
    public GameObject PanelObj;

    public static HandleError Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandleError>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("_ErrorManager");
                    instance = obj.AddComponent<HandleError>();
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        //ConnectingState();
    }
    private IEnumerator TurnOffObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PanelObj.SetActive(false);
    }
    public void ErrorHandler(string Message)
    {
        if (PanelObj != null) PanelObj.SetActive(true);
        errorTextDisplay.text = Message;
        StartCoroutine(TurnOffObjectAfterDelay(2f));
    }
    //private void Update()
    //{
    //    ConnectingState();
    //}
    //public void ConnectingState()
    //{
    //    if (PhotonNetwork.NetworkClientState == ClientState.ConnectingToMasterServer)
    //    {
    //        ErrorHandler("Connecting to Server....");    
           
    //    }

    //    //if (PhotonNetwork.NetworkClientState == ClientState.)
    //    //{
    //    //    ErrorHandler("Connecting to Server....");


    //    //}
    //}
   

    public override void OnJoinedRoom()
    {
        // Called when the local player creates a new room.
        if(PanelObj != null ) PanelObj.SetActive(false);

    }
    public override void OnCreatedRoom()
    {
        // Called when the local player creates a new room.
        if (PanelObj != null) PanelObj.SetActive(false);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room. Error: " + message);
        ErrorHandler("Failed to create room.Please check your internet connection");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room. Error: " + message);
        ErrorHandler("Failed to join room. Error: " + message);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server!");
        if (PanelObj != null) PanelObj.SetActive(false);


    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Disconnected from Photon Server. Reason: " + cause);
        ErrorHandler("Disconnected from Server.Please check your internet connection");
    }
}
