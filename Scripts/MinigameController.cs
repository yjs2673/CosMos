using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameController : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Script")]
    public OxygenGame oxygenGame;
    public FuelGame fuelGame;
    public BatteryGame batteryGame;

    [Header("GameObject")]
    public GameObject oxygenGamePanel;
    public GameObject fuelGamePanel;
    public GameObject batteryGamePanel;
    public GameObject successPanel;

    [Header("Ship Warning")]
    public GameObject[] warningImage;   // 수리 경고 이미지

    void Update()
    {
        float delta = Time.unscaledDeltaTime;
    }

    public void OpenOxygenGame()
    {
        if (!gameManager.warning[0]) return;
        oxygenGamePanel.SetActive(true);
        oxygenGame.Load();
        gameManager.isPlaying = true;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mini0);
    }
    public void OpenFuelGame()
    {
        if (!gameManager.warning[1]) return;
        fuelGamePanel.SetActive(true);
        fuelGame.Load();
        gameManager.isPlaying = true;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mini1);
    }
    public void OpenBatteryGame()
    {
        if (!gameManager.warning[2]) return;
        batteryGamePanel.SetActive(true);
        batteryGame.Load();
        gameManager.isPlaying = true;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mini2);
    }

    public void Success(int type)
    {
        warningImage[type].SetActive(false);

        successPanel.SetActive(true);
        StartCoroutine(CloseSuccess(0.5f));

        Time.timeScale = 1f;
        gameManager.isPlaying = false;
        gameManager.warning[type] = false;
        gameManager.ClearMinigame(type);
        
    }

    IEnumerator CloseSuccess(float delay)             // mini game 성공 패널 비활성화
    {
        // timeScale에 영향 받지 않고 delay 초만큼 대기
        yield return new WaitForSecondsRealtime(delay);

        successPanel.SetActive(false);
        Time.timeScale = 0f;
    }
}
