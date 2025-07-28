using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Player player;

    void Update()
    {
        followPlayer();
    }

    void followPlayer()
    {
        Vector3 fixPos = new Vector3(0f, 4.5f, -10f);
        transform.position = player.transform.position + fixPos;
    }
}
