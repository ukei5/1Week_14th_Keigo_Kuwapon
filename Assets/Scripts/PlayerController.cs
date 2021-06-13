using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] AudioSource bgmManager = default;
    [SerializeField] AudioClip fieldBGM = default;
    [SerializeField] AudioClip battleBGM = default;
    [SerializeField] AudioClip bossStageBGM = default;
    [SerializeField] BattleManager battleManager;
    float fadeSpeed = 0.035f;
    float red, green, blue, alfa;
    [SerializeField] Image fadeImage = default;
    public bool isFadeIn = false;
    public bool isFadeOut = false;
    Rigidbody2D rb;
    Vector2 movement;
    float moveSpeed = 4f;
    [SerializeField] GameObject battle = default;
    [SerializeField] GameObject map = default;
    [SerializeField] GameObject bossBattle = default;
    [SerializeField] GameObject canvasBattle = default;
    [SerializeField] GameObject canvasBossBattle = default;
    [SerializeField] int kakuritu = default;
    Animator animator;
    Vector3 playerPos = new Vector3(3,0,-3);
    private void Start()
    {
        AudioManager.instance.PlayBGM(fieldBGM);     
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
        battle.GetComponentInChildren<BattleManager>().cameraPos = Camera.main.transform.position;
        bossBattle.GetComponentInChildren<BossBattleManager>().cameraPos = Camera.main.transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (isFadeOut)
        {
            StartFadeOut();
        }
        if (isFadeIn)
        {
            StartFadeIn();
        }

        Camera.main.transform.rotation = Quaternion.Euler(0,0,0);
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
    void StartFadeIn()
    {
        alfa -= fadeSpeed;
        SetAlpha();                     
        if (alfa <= 0)
        {
            AudioManager.instance.PlayBGM(bossStageBGM);
            isFadeIn = false;
            fadeImage.enabled = false;   
        }
    }
    void StartFadeOut()
    {
        fadeImage.enabled = true;
        alfa += fadeSpeed;
        SetAlpha();
        if (alfa >= 1)
        {
            transform.position = new Vector3(-95, -23, -3);
            isFadeOut = false;
            isFadeIn = true;
        }
    }
    void SetAlpha()
    {
        fadeImage.color = new Color(red, green, blue, alfa);
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    void EnemySpawn()
    {
        AudioManager.instance.PlayBGM(battleBGM);
        battle.GetComponentInChildren<BattleManager>().cameraPos = Camera.main.transform.position;
        battle.SetActive(true);
        canvasBattle.SetActive(true);
        map.SetActive(false);
    }
    bool EnemyCheck()
    {
        int rand = Random.Range(0, 10000);
        return rand > kakuritu;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            AudioManager.instance.PlayBGM(battleBGM);
            Camera.main.transform.parent = null;
            bossBattle.GetComponentInChildren<BossBattleManager>().cameraPos = Camera.main.transform.position;
            bossBattle.SetActive(true);
            canvasBossBattle.SetActive(true);
            map.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Taikai")
        {
            isFadeOut = true;
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
