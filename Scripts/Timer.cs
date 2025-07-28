using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameManager gameManager;
    Text timerText;

    public float leftTime = 300f;
    private bool isRunning = false;

    void Start()
    {
        if (timerText == null) timerText = GetComponent<Text>();
        isRunning = true;
        UpdateTimer();
    }

    void Update()
    {
        if (!isRunning) return;

        leftTime -= Time.deltaTime;
        UpdateTimer();

        if (leftTime <= 0f) gameManager.Lose(1); // time over
    }

    private void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(leftTime / 60f);
        int seconds = Mathf.FloorToInt(leftTime % 60f);
        int milliseconds = Mathf.FloorToInt((leftTime - minutes * 60f - seconds) * 1000f);
        milliseconds /= 10;

        timerText.text = string.Format("{0}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
