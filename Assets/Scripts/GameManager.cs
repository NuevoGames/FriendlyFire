using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recievedQuestion;
    private QuestionGenerator questionGenerator;


    // Start is called before the first frame update
    void Start()
    {
        questionGenerator = GetComponent<QuestionGenerator>();
        Invoke("SendStringOverNetwork", 2f);
    }

    public void SendStringOverNetwork()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
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
    }
}

