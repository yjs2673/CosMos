using System.Collections;
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
    public GameObject[] warningImage;

    void Start()
    {
        oxygenGamePanel.SetActive(false);
        fuelGamePanel.SetActive(false);
        batteryGamePanel.SetActive(false);
        successPanel.SetActive(false);
    }

    void BeginMinigame()
    {
        gameManager.isPlaying = true;
        // Time.timeScale = 0f; // 우주선 멈춤
    }

    void EndMinigame()
    {
        gameManager.isPlaying = false;
        // Time.timeScale = 1f; // 우주선 재개
    }

    public void OpenOxygenGame()
    {
        if (!gameManager.warning[0]) return;

        oxygenGame.gameObject.SetActive(true);
        oxygenGamePanel.SetActive(true);

        BeginMinigame();
        oxygenGame.Load();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mini0);
    }

    public void OpenFuelGame()
    {
        if (!gameManager.warning[1]) return;

        fuelGame.gameObject.SetActive(true);
        fuelGamePanel.SetActive(true);

        BeginMinigame();
        fuelGame.Load();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mini1);
    }

    public void OpenBatteryGame()
    {
        if (!gameManager.warning[2]) return;

        batteryGame.gameObject.SetActive(true);
        batteryGamePanel.SetActive(true);

        BeginMinigame();
        batteryGame.Load();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mini2);
    }

    public void Success(int type)
    {
        warningImage[type].SetActive(false);
        
        if (type == 0) oxygenGamePanel.SetActive(false);
        if (type == 1) fuelGamePanel.SetActive(false);
        if (type == 2) batteryGamePanel.SetActive(false);
        
        gameManager.warning[type] = false;
        gameManager.ClearMinigame(type);
        
        successPanel.SetActive(true);
        StartCoroutine(CloseSuccessUI(0.5f));

        EndMinigame();
    }

    IEnumerator CloseSuccessUI(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        successPanel.SetActive(false);
        
        // Time.timeScale = 0f;  <-- 제거
    }
}