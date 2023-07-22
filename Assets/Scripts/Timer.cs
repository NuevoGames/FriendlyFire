using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timerDuration;

    // this script is responsible for creating Timers


    public void StartCountdown(float duration)
    {
        timerDuration = duration;
        InvokeRepeating(nameof(UpdateCountdown), 0f, 1f);
    }

    private void UpdateCountdown()
    {
        timerDuration -= 1f;
        //Debug.Log("Time Remaining: " + timerDuration.ToString("0"));
        if (timerText != null) {
            timerText.text = timerDuration.ToString();
        }

        if (timerDuration <= 0f)
        {
            GameManager.Instance.GetComponent<Voting>().GetWinner();
            CancelCountdown();
            Debug.Log("Countdown Finished!");
        }
    }

    private void CancelCountdown()
    {
        CancelInvoke(nameof(UpdateCountdown));
    }
}
