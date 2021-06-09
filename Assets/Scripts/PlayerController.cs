using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public Vector2 lastMove;

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
        battle.GetComponentInChildren<BattleManager>().cameraPos = Camera.main.transform.position;
        bossBattle.GetComponentInChildren<BossBattleManager>().cameraPos = Camera.main.transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        Animate();
        if (movement.x != 0 || movement.y != 0)
        {
            if (EnemyCheck())
            {
                Camera.main.transform.parent = null;
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
        int rand = Random.Range(0, 200);
        return rand > kakuritu;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            Camera.main.transform.parent = null;
            bossBattle.SetActive(true);
            canvasBossBattle.SetActive(true);
            map.SetActive(false);
            canvasMap.SetActive(false);
        }
    }
    public void Animate()
    {
        if (Mathf.Abs(movement.x) > 0.5f)
        {
            lastMove.x = movement.x;
            lastMove.y = 0;
        }
        if (Mathf.Abs(movement.y) > 0.5f)
        {
            lastMove.y = movement.y;
            lastMove.x = 0;
        }

        animator.SetFloat("Dir_X", movement.x);
        animator.SetFloat("Dir_Y", movement.y);
        animator.SetFloat("LastMove_X", lastMove.x);
        animator.SetFloat("LastMove_Y", lastMove.y);
        animator.SetFloat("Input", movement.magnitude);
    }

}
