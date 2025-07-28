using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenGame : MonoBehaviour
{
    public MinigameController minigameController;
    public Scrollbar oxygenTank;
    public Slider[] slider;
    int tankInt;
    bool nowPlay = false;

    void Update()
    {
        float delta = Time.unscaledDeltaTime;
        CheckLine();
    }

    public void Load()
    {
        nowPlay = true;

        for (int i = 0; i < 6; i++)
        {
            if (i == 5)
            {
                tankInt = Random.Range(0, 5);
                float tankNum = tankInt / 4f;
                oxygenTank.size = tankNum;
            }
            else
            {
                int num = Random.Range(0, 5);
                slider[i].value = num;
            }
        }
    }

    void CheckLine()
    {
        if (!nowPlay) return;

        int flag = 0;
        for (int i = 0; i < 5; i++)
        {
            if (tankInt == slider[i].value) flag++;
            else return;
        }

        if (flag == 5)
        {
            minigameController.Success(0);
            nowPlay = false;
            gameObject.SetActive(false);

            AudioManager.instance.PlaySfx(AudioManager.Sfx.MiniClear);
        }
    }

    public void ChangeSound()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mini0Btn);
    }
}
