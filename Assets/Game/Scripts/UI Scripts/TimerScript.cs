using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerScript : MonoBehaviour
{
    public Text timerText;
    private float timeRemaining = 600f;

    private void Update()
    {
        if(timeRemaining > 0)
        {
            // The line timeRemaining -= Time.deltaTime; is responsible
            // for decrementing the timeRemaining variable, effectively implementing the countdown for the timer.
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            timeRemaining = 0;
            // End the game
        }
    }

    private void UpdateTimerText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
