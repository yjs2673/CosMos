using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Basic")]
    public Player player;
    public int pattern_number;
    public float scale;
    public int speed;
    Vector3 direction;
    Vector3 start_position;

    [Header("Animation")]
    public RuntimeAnimatorController[] animatorControllers;
    Animator animator;

    // 진폭
    public float float_amplitude = 0.5f;
    public float float_frequency = 1f;

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
        
        pattern_number = Random.Range(0, 2);
        scale = Random.Range(3f, 5f);
        speed = Random.Range(1, 4);

        transform.localScale = new Vector3(scale, scale, scale);
        direction = new Vector3(0f, 0f, 0f);
        direction = player.transform.position - transform.position;
        direction = direction.normalized;
    }

    void Update()
    {
        Move(pattern_number);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall") gameObject.SetActive(false);
        if (collision.gameObject.tag == "Player") player.inCloud = true;
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") player.inCloud = false;
    }

    void Move(int pattern_number)
    {
        switch (pattern_number)
        {
            case 1:
                transform.position += direction * speed * Time.deltaTime;
                break;
            default:
                float newY = start_position.y + Mathf.Sin(Time.time * float_frequency) * float_amplitude;
                transform.position = new Vector3(start_position.x, newY, start_position.z);
                break;
        }
    }
}
