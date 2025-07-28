using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public GameManager gameManager;
    public Player player;

    void Update()
    {
        followPlayer();
    }

    void followPlayer()
    {
        Vector3 fixPos = new Vector3(0f, 4.5f, 0f);
        transform.position = player.transform.position + fixPos;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            gameManager.Lose(1);
        }
    }

}
