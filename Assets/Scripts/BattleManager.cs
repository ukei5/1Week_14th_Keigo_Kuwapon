using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// バトルの管理
public class BattleManager : MonoBehaviour
{
    [SerializeField] Battler player = default;// プレイヤー
    [SerializeField] Battler enemy = default;// 敵
    [SerializeField] KeyCode[] questionArrows = default;// 問題
    int questionCount;
    [SerializeField] GameObject[] arrows = default;// 矢印
    int count;
    private void Start()
    {
        StartCoroutine(Question());
    }
    private void Update()
    {   
        if (Input.GetKeyDown(questionArrows[count]) && !IsNotMouseDown() && count == questionCount)// 成功
        {  
            count++;
            Debug.Log("成功");
            if (count >= questionArrows.Length)
            {
                player.Attack(enemy);
                count = 0;
            }
        }
        else if (Input.anyKeyDown && !IsNotMouseDown())// 失敗
        {
            Debug.Log("失敗");
            enemy.Attack(player);
        }
    }
    IEnumerator Question()// 時間で矢印を光らせる
    {
        Behaviour halo;
        halo = (Behaviour)arrows[0].GetComponent("Halo");
        halo.enabled = true;
        yield return new WaitForSeconds(2);
        questionCount++;
        halo.enabled = false;
        halo = (Behaviour)arrows[1].GetComponent("Halo");
        halo.enabled = true;
        yield return new WaitForSeconds(2);
        questionCount++;
        halo.enabled = false;
        halo = (Behaviour)arrows[2].GetComponent("Halo");
        halo.enabled = true;
        yield return new WaitForSeconds(2);
        questionCount++;
        halo.enabled = false;
        halo = (Behaviour)arrows[3].GetComponent("Halo");
        halo.enabled = true;
        yield return new WaitForSeconds(3);
    }
    bool IsNotMouseDown()
    {
        return Input.GetMouseButton(0) ||
            Input.GetMouseButton(1) ||
            Input.GetMouseButton(2);

    }
}
