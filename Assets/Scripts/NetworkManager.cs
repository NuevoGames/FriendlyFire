using UnityEngine;
using Photon.Pun;
using Photon.Realtime;



public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance { get; private set; }

    [SerializeField] private string gameVersion = "1.0";

    private bool isConnecting = false;

    public bool IsConnecting
    {
        get => isConnecting;
        
    }

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        ConnectToPhotonServer();
    }

    private void ConnectToPhotonServer()
    {
        isConnecting = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    

    private void RestartConnection()
    {
        if (!PhotonNetwork.IsConnected)
        {
            ConnectToPhotonServer();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Disconnected from Photon Server. Reason: " + cause);
        RestartConnection();
    }

   
  
    
  
}
