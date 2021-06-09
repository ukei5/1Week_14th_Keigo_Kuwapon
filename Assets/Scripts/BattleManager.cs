using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// バトルの管理
public class BattleManager : MonoBehaviour
{
    [SerializeField] Battler player = default;// プレイヤー
    [SerializeField] Battler enemy = default;// 敵
    Animator playerAnimator;
    Animator enemyAnimator;
    [SerializeField] KeyCode[] questionArrows = default;// 問題
    int questionCount;
    [SerializeField] List<GameObject> arrows = default;
    [SerializeField] Text playerHPText = default;
    [SerializeField] Text enemyHPText = default;
    [SerializeField] Sprite arrow0 = default;
    [SerializeField] Sprite arrow1 = default;
    int count;
    int c;
    int rand;
    private void Start()
    {
        playerAnimator = player.gameObject.GetComponent<Animator>();
        enemyAnimator = enemy.gameObject.GetComponent<Animator>();
        playerHPText.text = $"HP:{player.hp}";
        enemyHPText.text = $"HP:{enemy.hp}";
        RandChange();
        StartCoroutine(Question());
    }
    void RandChange()
    {
        rand = Random.Range(0, arrows.Count);
        if (rand == 0 || rand == arrows.Count - 1)
        {
            RandChange();
            return;
        }
        arrows[rand].GetComponent<SpriteRenderer>().sprite = null;
    }
    void Output()
    {
        foreach (var arrow in arrows)
        {
            int rand = Random.Range(0, 4);
            switch (rand)
            {
                case 0:
                    arrow.transform.rotation = Quaternion.Euler(0, 0, -90);
                    questionArrows[c] = KeyCode.UpArrow;
                    break;
                case 1:
                    arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                    questionArrows[c] = KeyCode.DownArrow;
                    break;
                case 2:
                    arrow.transform.rotation = Quaternion.Euler(0, 180, -0);
                    questionArrows[c] = KeyCode.RightArrow;
                    break;
                case 3:
                    arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                    questionArrows[c] = KeyCode.LeftArrow;
                    break;
            }
            c++;
        }
        c = 0;
    }
    private void Update()
    {
        arrows[rand].GetComponent<SpriteRenderer>().sprite = null;
        if (Input.GetKeyDown(questionArrows[count]) && !IsNotMouseDown() && count == questionCount)// 成功
        {
            arrows[count].GetComponent<SpriteRenderer>().sprite = arrow1;
            count++;
            if (count >= questionArrows.Length)
            {
                player.Attack(enemy);
                //playerAnimator.SetBool("Attack", true);
                enemyHPText.text = $"HP:{enemy.hp}";
                if (enemy.hp <= 0)
                {
                    enemy.hp = 0;
                    enemyHPText.text = $"HP:{enemy.hp}";
                    SceneManager.LoadScene("MainScene");
                }
                count = 0;
            }
        }
        else if (Input.anyKeyDown && !IsNotMouseDown())// 失敗
        {
            enemy.Attack(player);
            //enemyAnimator.SetBool("Attack", true);
            playerHPText.text = $"HP:{player.hp}";
            if (player.hp <= 0)
            {
                player.hp = 0;
                playerHPText.text = $"HP:{player.hp}";
                SceneManager.LoadScene("MainScene");
            }
        }
    }
    IEnumerator Question()// 時間で矢印を光らせる
    {
        Output();
        Behaviour halo;
        halo = (Behaviour)arrows[0].GetComponent("Halo");
        halo.enabled = true;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < arrows.Count; i++)
        {
            if (count != 0)
            {
                if (arrows[questionCount].GetComponent<SpriteRenderer>().sprite == null)
                {
                    count++;
                    arrows[questionCount - 1].GetComponent<SpriteRenderer>().sprite = arrow0;
                }
                else if (arrows[count - 1].GetComponent<SpriteRenderer>().sprite == null)
                {
                    arrows[count - 1].GetComponent<SpriteRenderer>().sprite = null;
                }
                else
                {
                    arrows[count - 1].GetComponent<SpriteRenderer>().sprite = arrow0;
                }
            }

            halo.enabled = false;
            halo = (Behaviour)arrows[i].GetComponent("Halo");
            halo.enabled = true;
            yield return new WaitForSeconds(0.5f);
            questionCount++;
        }
        if (arrows[arrows.Count - 1].GetComponent<SpriteRenderer>().sprite == arrow0)
        {
            enemy.Attack(player);
            playerHPText.text = $"HP:{player.hp}";
            if (player.hp <= 0)
            {
                player.hp = 0;
                playerHPText.text = $"HP:{player.hp}";
                SceneManager.LoadScene("MainScene");
            }
        }
        foreach (GameObject arrow in arrows)
        {
            var h = (Behaviour)arrow.GetComponent("Halo");
            h.enabled = false;
            arrow.GetComponent<SpriteRenderer>().sprite = arrow0;
        }
        count = 0;
        questionCount = 0;

        StartCoroutine(Question());
    }
    bool IsNotMouseDown()
    {
        return Input.GetMouseButton(0) ||
            Input.GetMouseButton(1) ||
            Input.GetMouseButton(2);

    }
}
