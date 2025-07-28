using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioClip bgmClip;
    public float Bvolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float Svolume;
    public int Schannels;
    AudioSource[] sfxPlayers;

    int channelIdx;

    /* 
        OnBtn.24 ClickBtn.17  
    */
    public enum Sfx
    {
        OnBtn, ClickBtn, Mini0, Mini1, Mini2, MiniClear, Mini0Btn, Mini1Btn,
        Warning, Planet, Cloud, Blackhole, Over, Clear
    }

    void Awake()
    {
        instance = this;
        Init();
    }
    void Init()
    {
        // init bgm player
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = Bvolume;
        bgmPlayer.clip = bgmClip;

        // init sfx player
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[Schannels];
        for (int i = 0; i < Schannels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = Svolume;
        }
    }

    public void PlayBgm()
    {
        bgmPlayer.Play();
        // else bgmPlayer.Stop();
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < Schannels; i++)
        {
            int loopIdx = (i + channelIdx) % Schannels;

            if (sfxPlayers[loopIdx].isPlaying) continue;

            int ranIdx = 0;

            channelIdx = loopIdx;
            sfxPlayers[loopIdx].clip = sfxClips[(int)sfx + ranIdx];
            sfxPlayers[loopIdx].Play();
            break;
        }
    }
}
