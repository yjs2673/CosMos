using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Pysical")]
    public float speed;
    public float HP = 100f;

    [Header("State")]
    public bool isHit = false;
    public bool inCloud;
    public bool inBlackhole;
    public bool canMove = true;
    public bool isPoint = false;

    [Header("Manager")]
    public GameManager gameManager;

    [Header("Blackhole Settings")]
    public float blackholeDamagePerSecond = 10f;
    public float blackholePullSpeed = 2f;
    Vector3 blackholePos = new Vector3(0f, 0f, 0f);

    [Header("Components")]
    SpriteRenderer spriteRenderer;

    [Header("Sprites")]
    public Sprite[] sprites;
    public Sprite[] planetSprites;
    public Sprite[] cloudSprites;
    public Sprite[] blackholeSprites;
    int spriteIdx;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
        ReachCloud();
        ReachBlackhole();
        if (isPoint) CheckIsPoint();
    }

    void Move()
    {
        if (gameManager.isPause || !canMove) return;

        Vector3 newPosition = new Vector3(0f, 0f, 0f);

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 newDirection = new Vector3(0f, 1f, 0f);
            newPosition += newDirection;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 newDirection = new Vector3(0f, -1f, 0f);
            newPosition += newDirection;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 newDirection = new Vector3(1f, 0f, 0f);
            newPosition += newDirection;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 newDirection = new Vector3(-1f, 0f, 0f);
            newPosition += newDirection;
        }

        if (newPosition.x == 0 && newPosition.y >= 0)
        {
            if (!isHit && !isPoint) spriteRenderer.sprite = sprites[0];
            spriteIdx = 0;
        }     // center
        else if (newPosition.x > 0)
        {
            if (newPosition.y >= 0)
            {
                if (!isHit && !isPoint) spriteRenderer.sprite = sprites[1];
                spriteIdx = 1;
            }
            else
            {
                if (!isHit && !isPoint) spriteRenderer.sprite = sprites[4];
                spriteIdx = 4; // 우하향
            }

            // if (!isHit && !isPoint) spriteRenderer.sprite = sprites[1];
            //spriteIdx = 1;
        }    // right
        else if (newPosition.x < 0)
        {
            if (newPosition.y >= 0)
            {
                if (!isHit && !isPoint) spriteRenderer.sprite = sprites[2];
                spriteIdx = 2;
            }
            else
            {
                if (!isHit && !isPoint) spriteRenderer.sprite = sprites[5];
                spriteIdx = 5; // 좌하향
            }

            // if (!isHit && !isPoint) spriteRenderer.sprite = sprites[2];
            // spriteIdx = 2;
        }    // left
        else
        {
            if (!isHit && !isPoint) spriteRenderer.sprite = sprites[3];
            spriteIdx = 3; // 뒤로
        }

        newPosition = newPosition.normalized;
        transform.position += newPosition * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Station")
        {
            gameManager.Win();
        }
        /*if (collision.gameObject.tag == "Planet")
        {
            Crash();

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Planet);
        }*/
        // if (collision.gameObject.tag == "Cloud") inCloud = true;
        if (collision.gameObject.tag == "Blackhole")
        {
            gameManager.Lose(2);

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Planet);
        }
        if (collision.gameObject.tag == "Blackhole Cloud")
        {
            inBlackhole = true;
            blackholePos = collision.transform.parent.position;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cloud") inCloud = false;
        if (collision.gameObject.tag == "Blackhole Cloud") inBlackhole = false;
    }

    public void Crash()
    {
        if (isHit) return;
        isHit = true;
        gameManager.AlarmWarning();
        Unbeatable();

        HP -= 5f;
        gameManager.HP();
        if (HP <= 0) gameManager.Lose(0);
    }

    void ReachCloud()
    {
        if (isHit || !inCloud) return;
        HP -= 1f * Time.deltaTime;
        gameManager.HP();
        if (HP <= 0) gameManager.Lose(0);

        if (spriteIdx == 0 && !isPoint) spriteRenderer.sprite = cloudSprites[0];
        else if (spriteIdx == 1 && !isPoint) spriteRenderer.sprite = cloudSprites[1];
        else if (spriteIdx == 2 && !isPoint) spriteRenderer.sprite = cloudSprites[2];
        else if (spriteIdx == 3 && !isPoint) spriteRenderer.sprite = cloudSprites[3];
        else if (spriteIdx == 4 && !isPoint) spriteRenderer.sprite = cloudSprites[4];
        else if (spriteIdx == 5 && !isPoint) spriteRenderer.sprite = cloudSprites[5];

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Cloud);
    }

    void ReachBlackhole()
    {
        if (isHit || !inBlackhole) return;
        HP -= 3f * Time.deltaTime;
        gameManager.HP();
        if (HP <= 0) gameManager.Lose(0);

        pullPlayer();

        if (spriteIdx == 0 && !isPoint) spriteRenderer.sprite = blackholeSprites[0];
        else if (spriteIdx == 1 && !isPoint) spriteRenderer.sprite = blackholeSprites[1];
        else if (spriteIdx == 2 && !isPoint) spriteRenderer.sprite = blackholeSprites[2];
        else if (spriteIdx == 3 && !isPoint) spriteRenderer.sprite = blackholeSprites[3];
        else if (spriteIdx == 4 && !isPoint) spriteRenderer.sprite = blackholeSprites[4];
        else if (spriteIdx == 5 && !isPoint) spriteRenderer.sprite = blackholeSprites[5];

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Blackhole);
    }

    void pullPlayer()
    {
        Vector2 dir = (blackholePos - transform.position).normalized;
        transform.position += (Vector3)dir * blackholePullSpeed * Time.deltaTime;
    }

    void Unbeatable()
    {
        if (spriteIdx == 0) spriteRenderer.sprite = planetSprites[0];
        else if (spriteIdx == 1) spriteRenderer.sprite = planetSprites[1];
        else if (spriteIdx == 2) spriteRenderer.sprite = planetSprites[2];
        else if (spriteIdx == 3) spriteRenderer.sprite = planetSprites[3];
        Invoke("returnHit", 1);
    }

    void returnHit()
    {
        isHit = false;
    }

    public void LightSprite()
    {
        isPoint = true;
    }

    public void ReturnSprite()
    {
        isPoint = false;
    }

    void CheckIsPoint() {
        if (inCloud)
        {
            if (spriteIdx == 0) spriteRenderer.sprite = cloudSprites[6];
            else if (spriteIdx == 1) spriteRenderer.sprite = cloudSprites[7];
            else if (spriteIdx == 2) spriteRenderer.sprite = cloudSprites[8];
            else if (spriteIdx == 3) spriteRenderer.sprite = cloudSprites[9];
            else if (spriteIdx == 4) spriteRenderer.sprite = cloudSprites[10];
            else if (spriteIdx == 5) spriteRenderer.sprite = cloudSprites[11];
        }
        else if (inBlackhole)
        {
            if (spriteIdx == 0) spriteRenderer.sprite = blackholeSprites[6];
            else if (spriteIdx == 1) spriteRenderer.sprite = blackholeSprites[7];
            else if (spriteIdx == 2) spriteRenderer.sprite = blackholeSprites[8];
            else if (spriteIdx == 3) spriteRenderer.sprite = blackholeSprites[9];
            else if (spriteIdx == 4) spriteRenderer.sprite = blackholeSprites[10];
            else if (spriteIdx == 5) spriteRenderer.sprite = blackholeSprites[11];
        }
        else
        {
            if (spriteIdx == 0) spriteRenderer.sprite = sprites[6];
            else if (spriteIdx == 1) spriteRenderer.sprite = sprites[7];
            else if (spriteIdx == 2) spriteRenderer.sprite = sprites[8];
            else if (spriteIdx == 3) spriteRenderer.sprite = sprites[9];
            else if (spriteIdx == 4) spriteRenderer.sprite = sprites[10];
            else if (spriteIdx == 5) spriteRenderer.sprite = sprites[11];
        }
    }
}
