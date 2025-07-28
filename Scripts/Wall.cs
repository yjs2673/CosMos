using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Player player;

    void Update()
    {
        followPlayer();
    }

    void followPlayer()
    {
        Vector3 fixPos = new Vector3(0f, -10f, 0f);
        transform.position = player.transform.position + fixPos;
    }
}
