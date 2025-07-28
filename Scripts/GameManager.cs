using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public Player player;
    public Scrollbar dis_scrollbar;
    public Scrollbar hp_scrollbar;

    [Header("UI")]
    public GameObject shipPanel;
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;
    public GameObject dangerPanel;
    public GameObject pausePanel;

    [Header("Ship Warning")]
    public GameObject[] warningImage;   // 수리 경고 이미지
    public bool[] warning;              // 수리 필요 여부
    public bool danger = false;         // 수리 경고 패널 활성화 여부
    public int warningCnt = 0;          // 수리 경고 개수

    [Header("Pool Manager")]
    public ObjectManager objectManager;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public float spawnInterval = 1f;
    public float spawnTimer = 0f;

    [Header("Station & Distance UI")]
    public Vector3 station;
    public float distance;
    public float current_distance;
    public float dis_bar_value = 0f;
    public float hp_bar_value = 1f;

    [Header("State")]
    public bool isLive = true;
    public bool isPause = false;
    public bool isPlaying = false;

    [Header("Audio")]
    public AudioManager audioManager;

    GameObject Player;

    void Start()
    {
        Player = player.gameObject;
        for (int i = 0; i < 3; i++) warning[i] = false;
        distance = Vector2.Distance(player.transform.position, station);
        audioManager.PlayBgm();
    }

    void Update()
    {
        if (!isLive) return;

        Distance();
        UpdateSpawnTimer();

        if (Input.GetKey(KeyCode.Space)) OpenShipMenu();
        // if (Input.GetKey(KeyCode.Escape)) CloseShipMenu();

        /*if (dangerPanel.activeSelf)
        {
            Invoke("Warning", 2);
        }*/
    }

    /*void Warning()                  // 경고 패널 활성화 시 경고음 지속 재생
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Warning);
    }*/

    void Distance()                 // 플레이어 -> 지구의 거리 스크롤바의 값 계산
    {
        current_distance = Vector2.Distance(player.transform.position, station);
        dis_bar_value = 1 - current_distance / distance;
        dis_scrollbar.value = dis_bar_value;

        spawnInterval = current_distance / distance + 0.1f;
    }

    public void HP()                // 플레이어 HP 스크롤바의 값 계산
    {
        float hp = player.HP;
        hp_bar_value = hp / 100;
        hp_scrollbar.size = hp_bar_value;
    }

    public void Win()               // 게임 클리어
    {
        GameObject Player = player.gameObject;
        Player.SetActive(false);
        gameClearPanel.SetActive(true);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Clear);
    }

    public void Lose(int type)      // 게임 오버 (종류에 따른 문구 출력)
    {
        GameObject Player = player.gameObject;
        Player.SetActive(false);

        // Explosion();
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        switch (type)
        {
            case 0: // hp가 0이 되면서 오버
                break;
            case 1: // 남은 시간이 0이 되면서 오버
                break;
            case 2: // 블랙홀과 충돌 시 오버
                break;
            case 3: // 경고 3중첩
                break;
        }

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Over);
    }

    void UpdateSpawnTimer()         // 스폰 타이머 업데이트 및 간격 도달 시 스폰 호출
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()        // 랜덤으로 장애물 생성
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        // 타입 선택
        string[] types = new string[] { "Planet", "Cloud", "Blackhole" };
        string type;

        for (int i = 0; i < 2; i++)
        {
            int num = Random.Range(1, 6);   // 3 : 1 : 1 비율 확률
            if (num <= 3) type = types[0];
            else if (num == 4) type = types[1];
            else type = types[2];

            // 풀에서 오브젝트 가져오기
            GameObject obj = objectManager.MakeObj(type);
            if (obj != null)
            {
                // 스폰 포인트 랜덤 선택
                Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
                obj.transform.position = sp.position;
                obj.transform.rotation = sp.rotation;
            }
        }
    }

    public void AlarmWarning()      // 소행성 충돌 -> 수리 하나 추가
    {
        int type = Random.Range(0, 3) % 3;
        //int type = Random.Range(1, 2) % 3;
        int idx = type;

        if (!warning[idx])
        {
            warning[idx] = true;
            warningCnt++;
        }
        else
        {
            int next = Random.Range(0, 2);
            if (next == 1)
            {
                idx = (idx + 1) % 3;
                if (!warning[idx])
                {
                    warning[idx] = true;
                    warningCnt++;
                }
                else Lose(3);
            }
            else
            {
                idx = (idx + 2) % 3;
                if (!warning[idx])
                {
                    warning[idx] = true;
                    warningCnt++;
                }
                else Lose(3);
            }
        }

        CheckWarning();
        warningImage[idx].SetActive(true);
    }

    void CheckWarning()             // 수리 2개면 경고문 출력. 3개면 게임 오버
    {
        if (warningCnt == 3) Lose(3);
        else if (warningCnt == 2)
        {
            if (!danger)
            {
                danger = true;
                OpenDangerPanel();
            }
        }
        else
        {
            if (danger)
            {
                danger = false;
                CloseDangerPanel();
            }
        }
    }

    public void ClearMinigame(int type)       // mini game 성공
    {
        warning[type] = false;
        warningCnt--;
        CheckWarning();
    }

    //************** UI **************//

    public void OpenShipMenu()      // 우주선 메뉴 패널 활성화
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        Time.timeScale = 0f;
        shipPanel.SetActive(true);
    }

    public void CloseShipMenu()
    {
        if (isPlaying) return;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        shipPanel.SetActive(false); // 우주선 메뉴 패널 비활성화
    }

    void OpenDangerPanel()          // 수리 경고 패널 활성화
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Warning);
        dangerPanel.SetActive(true);
    }

    public void CloseDangerPanel()  // 수리 경고 패널 비활성화
    {
        dangerPanel.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        isPause = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        isPause = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void Retry()             // 게임 재시작
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1f;
    }

    public void Menu()              // 메인 메뉴 이동
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        SceneManager.LoadScene("StartScene");
    }
}
