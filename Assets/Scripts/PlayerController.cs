using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum DIRECTION_TYPE
    {
        RIGHT,
        LEFT,
        UP,
        DOWN,
    }
    Rigidbody2D rb;
    Vector2 movement;
    float moveSpeed = 4f;
    [SerializeField] GameObject battle = default;
    [SerializeField] GameObject map = default;
    [SerializeField] GameObject bossBattle = default;
    [SerializeField] GameObject canvasBattle = default;
    [SerializeField] GameObject canvasMap = default;
    [SerializeField] GameObject canvasBossBattle = default;
    [SerializeField] int kakuritu = default;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.x != 0 || movement.y != 0)
        {
            if (EnemyCheck())
            {
                EnemySpawn();
            }
        }        
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    void EnemySpawn()
    {
        battle.SetActive(true);
        canvasBattle.SetActive(true);
        map.SetActive(false);
        canvasMap.SetActive(false);
    }
    bool EnemyCheck()
    {
        int rand = Random.Range(0, 1000000);
        return rand > kakuritu;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            bossBattle.SetActive(true);
            canvasBossBattle.SetActive(true);
            map.SetActive(false);
            canvasMap.SetActive(false);
        }
    }
}
