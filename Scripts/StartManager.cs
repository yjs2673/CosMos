using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public GameObject[] canvas;
    public GameObject[] readmeBnt;

    public GameObject background;
    public float speed = 0.1f;
    Vector3 backNewPos = new Vector3(1f, 0f, 0f);
    public Animator fadeAnime;

    void Start()
    {
        Time.timeScale = 1f;
        AudioManager.instance.PlayBgm();
    }

    void Update()
    {
        /*if (background.activeSelf)  // 프롤로그 배경화면 이동
        {
            background.transform.position += backNewPos * speed * Time.deltaTime;
        }*/
    }

    public void GameStart()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        SceneManager.LoadScene("GameScene");
    }

    public void GameExit()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        Application.Quit();
    }

    public void OpenReadme()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        readmeBnt[0].SetActive(true);
    }

    public void NextPage()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        readmeBnt[0].SetActive(false);
        readmeBnt[1].SetActive(true);
    }

    public void EndPage()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        readmeBnt[1].SetActive(false);
    }

    //*******  Prologue *******//

    public GameObject[] scripts;
    public GameObject fade;

    public void NextScript()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        scripts[1].SetActive(true);
        scripts[0].SetActive(false);
    }

    public void EndScript()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ClickBtn);
        scripts[1].SetActive(false);
        FadeOut();
    }

    void FadeOut()
    {
        canvas[0].SetActive(false);
        fade.SetActive(true);
        StartCoroutine(DestroyFade(0.4f));
    }

    IEnumerator DestroyFade(float delay)
    {
        // timeScale에 영향 받지 않고 delay 초만큼 대기
        yield return new WaitForSecondsRealtime(delay);
        fade.SetActive(false);
        canvas[1].SetActive(true);
    }
}
