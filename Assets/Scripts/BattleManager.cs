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
    [SerializeField] KeyCode[] questionArrows = default;// 問題
    int questionCount;
    [SerializeField] GameObject[] arrows = default;// 矢印
    [SerializeField] Text playerHPText = default;
    [SerializeField] Text enemyHPText = default;
    [SerializeField] Sprite arrow0 = default;
    [SerializeField] Sprite arrow1 = default;
    int count;
    private void Start()
    {
        playerHPText.text = $"HP:{player.hp}";
        enemyHPText.text = $"HP:{enemy.hp}";
        StartCoroutine(Question());
    }
    private void Update()
    {
        Debug.Log(count);
        if (Input.GetKeyDown(questionArrows[count]) && !IsNotMouseDown() && count == questionCount)// 成功
        {
            arrows[count].GetComponent<SpriteRenderer>().sprite = arrow1;
            if (count != 0)
                arrows[count - 1].GetComponent<SpriteRenderer>().sprite = arrow0;
            count++;
            Debug.Log("成功");
            if (count >= questionArrows.Length)
            {
                player.Attack(enemy);
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
            Debug.Log("失敗");
            enemy.Attack(player);
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
        Behaviour halo;
        halo = (Behaviour)arrows[0].GetComponent("Halo");
        halo.enabled = true;
        yield return new WaitForSeconds(0.75f);
        questionCount++;
        halo.enabled = false;
        halo = (Behaviour)arrows[1].GetComponent("Halo");
        halo.enabled = true;
        yield return new WaitForSeconds(1f);
        questionCount++;
        halo.enabled = false;
        halo = (Behaviour)arrows[2].GetComponent("Halo");
        halo.enabled = true;
        yield return new WaitForSeconds(0.75f);
        questionCount++;
        halo.enabled = false;
        halo = (Behaviour)arrows[3].GetComponent("Halo");
        halo.enabled = true;
        yield return new WaitForSeconds(2f);
        halo.enabled = false;
        count = 0;
        questionCount = 0;
        arrows[arrows.Length - 1].GetComponent<SpriteRenderer>().sprite = arrow0;
        StartCoroutine(Question());
    }
    bool IsNotMouseDown()
    {
        return Input.GetMouseButton(0) ||
            Input.GetMouseButton(1) ||
            Input.GetMouseButton(2);

    }
}
