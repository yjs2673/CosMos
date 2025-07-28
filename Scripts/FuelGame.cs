using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelGame : MonoBehaviour
{
    public MinigameController minigameController;
    public Hole holePrefab;
    public Player player;
    public int holeCnt;
    bool nowPlay = false;

    public float currtime = 0f;
    public float checktime = 2f;

    void Update()
    {
        if (!nowPlay) return;
        checkCnt();

        currtime += Time.unscaledDeltaTime;
        if (currtime >= checktime)
        {
            currtime = 0f;
            SelfFix();
        }
    }

    public void Load()
    {
        nowPlay = true;

        holeCnt = 7;
        for (int i = 0; i < 7; i++)
        {
            Hole newHole = Instantiate(holePrefab);
            newHole.fuleGame = this;
            newHole.player = player;
        }
    }

    void checkCnt()
    {
        if (holeCnt <= 0)
        {
            nowPlay = false;
            minigameController.Success(1);
            gameObject.SetActive(false);

            AudioManager.instance.PlaySfx(AudioManager.Sfx.MiniClear);
        }
    }

    void SelfFix()
    {
        Hole hole = FindAnyObjectByType<Hole>();
        if (hole != null) Destroy(hole.gameObject);
        holeCnt--;
    }
}
