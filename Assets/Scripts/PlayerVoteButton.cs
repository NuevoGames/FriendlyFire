using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PlayerVoteButton : MonoBehaviour
{
    Button button;
    
    // Start is called before the first frame update
    void Start()
    {
       button =  GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnButtonClick()
    {
        // Add your custom logic here that you want to execute when the button is clicked
        Debug.Log("Button Clicked!");
        GameManager.Instance.GetComponent<Voting>().VoteForPlayer(button.name);
    }
}
