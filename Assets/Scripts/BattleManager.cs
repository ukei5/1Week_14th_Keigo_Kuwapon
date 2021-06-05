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
    Dictionary<GameObject, KeyCode> arrows2 = new Dictionary<GameObject, KeyCode>();
    [SerializeField] Text playerHPText = default;
    [SerializeField] Text enemyHPText = default;
    [SerializeField] Sprite arrow0 = default;
    [SerializeField] Sprite arrow1 = default;
    int count;
    int c;
    private void Start()
    {
        arrows2.Add(arrows[0],KeyCode.UpArrow);
        arrows2.Add(arrows[1], KeyCode.DownArrow);
        arrows2.Add(arrows[2], KeyCode.RightArrow);
        arrows2.Add(arrows[3], KeyCode.LeftArrow);
        playerHPText.text = $"HP:{player.hp}";
        enemyHPText.text = $"HP:{enemy.hp}";
        StartCoroutine(Question());
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
        Output();
        Behaviour halo;
        halo = (Behaviour)arrows[0].GetComponent("Halo");
        halo.enabled = true;
        yield return new WaitForSeconds(1f);
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
