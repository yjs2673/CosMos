using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Basic")]
    public Player player;
    public float scale;
    public int speed;
    // Vector3 start_position;
    Vector3 direction;

    [Header("Animation")]
    public RuntimeAnimatorController[] animatorControllers;
    Animator animator;

    // 진폭
    // public float float_amplitude = 0.5f;
    // public float float_frequency = 1f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (player == null) player = FindObjectOfType<Player>();
    }

    void Start()
    {
        if (animatorControllers != null && animatorControllers.Length > 0)
        {
            int idx = Random.Range(0, animatorControllers.Length);
            animator.runtimeAnimatorController = animatorControllers[idx];
        } 

        scale = Random.Range(0.5f, 2f);
        speed = Random.Range(3, 9);

        transform.localScale = new Vector3(scale, scale, scale);
        // start_position = transform.position;
        direction = new Vector3(0f, 0f, 0f);
        direction = player.transform.position - transform.position;
        direction = direction.normalized;
    }

    void Update()
    {
        Move();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall") gameObject.SetActive(false);
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Planet);
            gameObject.SetActive(false);
            player.Crash();
        }
    }

    void Move()
    {
        /*float newY = start_position.y + Mathf.Sin(Time.time * float_frequency) * float_amplitude;
        transform.position = new Vector3(start_position.x, newY, start_position.z);*/

        transform.position += direction * speed * Time.deltaTime;
    }
}
