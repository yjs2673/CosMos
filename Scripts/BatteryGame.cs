using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BatteryGame : MonoBehaviour
{
    public MinigameController minigameController;
    public Scrollbar up;
    public Scrollbar down;
    public Scrollbar upMove;
    public Scrollbar downMove;

    public float speed;
    int num;

    bool upMovable = true;
    bool downMovable = true;
    bool nowPlay = false;

    void Update()
    {
        if (!nowPlay) return;
        CheckLine();
        Move();
    }

    public void Load()
    {
        // ✅ 이전 판 잔재 제거
        StopAllCoroutines();
        nowPlay = true;
        upMovable = true;
        downMovable = true;

        num = Random.Range(0, 4);
        float value = num / 3f;
        up.value = down.value = value;

        upMove.value = 0f;
        downMove.value = 1f;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mini2);
    }

    void CheckLine()
    {
        if (!nowPlay) return;
        if (upMovable || downMovable) return;

        float uv = upMove.value;
        float dv = downMove.value;
        bool connect = false;

        switch (num)
        {
            case 0: connect = (0f <= uv && uv <= 0.22f && 0f <= dv && dv <= 0.22f); break;
            case 1: connect = (0.24f <= uv && uv <= 0.48f && 0.24f <= dv && dv <= 0.48f); break;
            case 2: connect = (0.52f <= uv && uv <= 0.75f && 0.52f <= dv && dv <= 0.75f); break;
            case 3: connect = (0.79f <= uv && uv <= 1f && 0.79f <= dv && dv <= 1f); break;
        }

        if (connect && Mathf.Abs(uv - dv) <= 0.6f)
        {
            nowPlay = false;
            minigameController.Success(2);
            
            AudioManager.instance.PlaySfx(AudioManager.Sfx.MiniClear);
        }
    }

    void Move()
    {
        float t = (Mathf.Sin(Time.unscaledTime * speed * Mathf.PI * 2f) + 1f) * 0.5f;

        if (upMovable) upMove.value = t;
        if (downMovable) downMove.value = 1f - t;
    }

    public void StopUp()
    {
        if (!nowPlay) return;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);

        upMovable = false;
        StartCoroutine(SetUp(4f));
    }

    public void StopDown()
    {
        if (!nowPlay) return;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);

        downMovable = false;
        StartCoroutine(SetDown(4f));
    }

    IEnumerator SetUp(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        upMovable = true;
    }

    IEnumerator SetDown(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        downMovable = true;
    }
}