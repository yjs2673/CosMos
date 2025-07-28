using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public FuelGame fuleGame;
    public Player player;
    public Sprite[] sprites;
    public Sprite[] fix_sprites;
    SpriteRenderer spriteRenderer;

    int restPush = 2;
    float posX;
    float posY;
    Vector3 firstPos;
    Sprite sprite;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        posX = Random.Range(-5.5f, 5.5f);
        posY = Random.Range(-3.5f, 3.5f);
        transform.position += new Vector3(posX, posY, 0f);
        firstPos = transform.position;

        spriteRenderer.sprite = sprites[Random.Range(0, 7)];
        sprite = spriteRenderer.sprite;
    }

    void Update()
    {
        follow();
    }

    void OnMouseEnter()  // 2번 터치하면 구멍 메워짐
    {
        spriteRenderer.sprite = fix_sprites[Random.Range(0, fix_sprites.Length)];
        StartCoroutine(ReturnSpriteAfterRealtime(0.2f));
    }

    IEnumerator ReturnSpriteAfterRealtime(float delay)
    {
        // timeScale에 영향 받지 않고 delay 초만큼 대기
        yield return new WaitForSecondsRealtime(delay);

        // 원래 스프라이트로 복원하고 처리
        spriteRenderer.sprite = sprite;
        Done();
    }

    void follow()
    {
        Vector3 fixPos = new Vector3(0f, 4.5f, 0f);
        transform.position = player.transform.position + firstPos + fixPos;
    }

    /*void ChangeSprite()
    {
        spriteRenderer.sprite = fix_sprites[Random.Range(0, 3)];
        Invoke("ReturnSprite", 0.2f);
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprite;
        Done();
    }*/

    void Done()
    {
        restPush--;
        if (restPush <= 0)
        {
            Destroy(gameObject);
            fuleGame.holeCnt--;
        }

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Mini1Btn);
    }
}
