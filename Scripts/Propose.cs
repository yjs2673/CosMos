using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propose : MonoBehaviour
{
    public float maxTime = 5f;
    public float curTime = 0f;

    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= maxTime) gameObject.SetActive(false);
    }
}
