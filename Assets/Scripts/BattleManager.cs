using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// バトルの管理
public class BattleManager : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController[] enemyAnim = default;
    [SerializeField] GameObject gameOver = default;
    [SerializeField] Battler player = default;// プレイヤー
    [SerializeField] Battler enemy = default;// 敵
    [SerializeField] Animator playerAnimator = default;
    [SerializeField] Animator enemyAnimator = default;
    [SerializeField] KeyCode[] questionArrows = default;// 問題
    int questionCount;
    [SerializeField] List<GameObject> arrows = default;
    [SerializeField] Text playerHPText = default;
    [SerializeField] Text enemyHPText = default;
    [SerializeField] Sprite arrow0 = default;
    [SerializeField] Sprite arrow1 = default;
    [SerializeField] Sprite arrow2 = default;
    [SerializeField] Sprite kyuufu = default;
    [SerializeField] Text chatText = default;
    [SerializeField] Text countDownText = default;
    public Vector3 cameraPos;
    int count;
    int c;
    int letCount;
    int rand;
    int updateCount;
    bool isStart = false;
    [SerializeField] GameObject battle = default;
    [SerializeField] GameObject map = default;
    [SerializeField] GameObject bossBattle = default;
    [SerializeField] GameObject canvasBattle = default;
    [SerializeField] GameObject canvasMap = default;
    [SerializeField] GameObject canvasBossBattle = default;
    [SerializeField] Transform player_Map = default;
    void Shokika()
    {
        count = 0;
        c = 0;
        letCount = 0;
        rand = 0;
        isStart = false;
        enemy.hp = 15;
        player.hp = 25;
        playerHPText.text = "HP:" + player.hp;
        enemyHPText.text = "HP:" + enemy.hp;
        chatText.text = "";
    }
    IEnumerator CountDownStart()
    {
        Camera.main.transform.position = cameraPos;
        playerHPText.text = $"HP:{player.hp}";
        enemyHPText.text = $"HP:{enemy.hp}";
        countDownText.text = "3";
        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1f);
        countDownText.text = "Go!";
        yield return new WaitForSeconds(1f);
        countDownText.text = "";
        isStart = true;
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
        arrows[rand].GetComponent<SpriteRenderer>().sprite = kyuufu;
    }
    void Output()
    {
        foreach (var arrow in arrows)
        {
            arrow.SetActive(true);
            int rand = Random.Range(0, 4);
            switch (rand)
            {
                case 0:
                    questionArrows[c] = KeyCode.UpArrow;
                    break;
                case 1:
                    questionArrows[c] = KeyCode.DownArrow;
                    break;
                case 2:
                    questionArrows[c] = KeyCode.RightArrow;
                    break;
                case 3:
                    questionArrows[c] = KeyCode.LeftArrow;
                    break;
            }
            arrows[rand].transform.rotation = Quaternion.Euler(0, 0, 0);
            c++;
        }
        c = 0;
        foreach (var questionArrow in questionArrows)
        {
            switch (questionArrow)
            {
                case KeyCode.UpArrow:
                    arrows[c].transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case KeyCode.DownArrow:
                    arrows[c].transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case KeyCode.RightArrow:
                    arrows[c].transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case KeyCode.LeftArrow:
                    arrows[c].transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }
            c++;
        }
        c = 0;
    }
    private void Update()
    {
        if (updateCount == 0)
        {
            Shokika();
            int r = Random.Range(0, enemyAnim.Length);
            enemy.gameObject.GetComponent<Animator>().runtimeAnimatorController = enemyAnim[r];
            StartCoroutine(CountDownStart());
            updateCount++;
        }
        Camera.main.transform.position = new Vector3(0, 0, -133.3333f);
        if (isStart == true)
        {
         
            arrows[rand].transform.rotation = Quaternion.Euler(0, 0, 0);
            arrows[rand].GetComponent<SpriteRenderer>().sprite = kyuufu;
            if (Input.GetKeyDown(questionArrows[count]) && !IsNotMouseDown() && count == questionCount)// 成功
            {
                arrows[count].GetComponent<SpriteRenderer>().sprite = arrow1;
                count++;
                AudioManager.instance.PlaySE(AudioManager.instance.danceTrue);
                if (count >= questionArrows.Length)
                {
                    player.Attack(enemy);
                    switch (questionArrows[count - 1])
                    {
                        case KeyCode.UpArrow:
                            playerAnimator.SetTrigger("W");
                            break;
                        case KeyCode.DownArrow:
                            playerAnimator.SetTrigger("S");
                            break;
                        case KeyCode.RightArrow:
                            playerAnimator.SetTrigger("D");
                            break;
                        case KeyCode.LeftArrow:
                            playerAnimator.SetTrigger("A");
                            break;
                    }
                    enemyHPText.text = $"HP:{enemy.hp}";
                    if (enemy.hp <= 0)
                    {
                        AudioManager.instance.PlaySE(AudioManager.instance.win);
                        enemy.hp = 0;
                        enemyHPText.text = $"HP:{enemy.hp}";
                        chatText.text = "敵を倒した!";
                        StartCoroutine(SceneLoad());
                    }
                    letCount = count;
                    count = 0;
                }
                else
                {
                    switch (questionArrows[count - 1])
                    {
                        case KeyCode.UpArrow:
                            playerAnimator.SetTrigger("W");
                            break;
                        case KeyCode.DownArrow:
                            playerAnimator.SetTrigger("S");
                            break;
                        case KeyCode.RightArrow:
                            playerAnimator.SetTrigger("D");
                            break;
                        case KeyCode.LeftArrow:
                            playerAnimator.SetTrigger("A");
                            break;
                    }
                }
            }
            else if (Input.anyKeyDown && !IsNotMouseDown())// 失敗
            {
                if (questionCount >= arrows.Count)
                {
                    letCount = count;
                }
                enemy.Attack(player);
                if (count == 0)
                {
                    switch (questionArrows[count])
                    {
                        case KeyCode.UpArrow:
                            enemyAnimator.SetTrigger("W");
                            break;
                        case KeyCode.DownArrow:
                            enemyAnimator.SetTrigger("S");
                            break;
                        case KeyCode.RightArrow:
                            enemyAnimator.SetTrigger("D");
                            break;
                        case KeyCode.LeftArrow:
                            enemyAnimator.SetTrigger("A");
                            break;
                    }
                }
                else
                {
                    switch (questionArrows[count - 1])
                    {
                        case KeyCode.UpArrow:
                            enemyAnimator.SetTrigger("W");
                            break;
                        case KeyCode.DownArrow:
                            enemyAnimator.SetTrigger("S");
                            break;
                        case KeyCode.RightArrow:
                            enemyAnimator.SetTrigger("D");
                            break;
                        case KeyCode.LeftArrow:
                            enemyAnimator.SetTrigger("A");
                            break;
                    }
                }
                AudioManager.instance.PlaySE(AudioManager.instance.danceFalse);
                playerHPText.text = $"HP:{player.hp}";
                if (player.hp <= 0)
                {
                    AudioManager.instance.PlaySE(AudioManager.instance.lose);
                    player.hp = 0;
                    playerHPText.text = $"HP:{player.hp}";
                    gameOver.SetActive(true);
                    canvasBattle.SetActive(false);
                    battle.SetActive(false);
                }
            }
        }
       
    }
    IEnumerator SceneLoad()
    {
        yield return new WaitForSeconds(2f);
        Shokika();
        updateCount = 0;
        map.SetActive(true);
        Camera.main.transform.parent = player_Map;
        Camera.main.transform.position = cameraPos;
        canvasBattle.SetActive(false);
        battle.SetActive(false);
    }
    IEnumerator Question()// 時間で矢印を光らせる
    {
        Output();
        arrows[0].GetComponent<SpriteRenderer>().sprite = arrow2;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < arrows.Count; i++)
        {
            if (questionCount != 0)
            {
                if (arrows[questionCount].GetComponent<SpriteRenderer>().sprite == kyuufu)
                {
                    count++;
                    arrows[questionCount - 1].GetComponent<SpriteRenderer>().sprite = arrow0;
                }
                else if (arrows[questionCount - 1].GetComponent<SpriteRenderer>().sprite == kyuufu)
                {
                    arrows[questionCount - 1].GetComponent<SpriteRenderer>().sprite = kyuufu;
                }
                else
                {
                    arrows[questionCount - 1].GetComponent<SpriteRenderer>().sprite = arrow0;
                }
            }
            arrows[i].GetComponent<SpriteRenderer>().sprite = arrow2;
            yield return new WaitForSeconds(1f);
            if (arrows[i].GetComponent<SpriteRenderer>().sprite != kyuufu)
            {
                arrows[i].GetComponent<SpriteRenderer>().sprite = arrow0;
            }
            questionCount++;
        }
        if (letCount != questionCount)
        {
            enemy.Attack(player);
            playerHPText.text = $"HP:{player.hp}";
            if (player.hp <= 0)
            {
                AudioManager.instance.PlaySE(AudioManager.instance.lose);
                player.hp = 0;
                playerHPText.text = $"HP:{player.hp}";
                gameOver.SetActive(true);
                canvasBattle.SetActive(false);
                battle.SetActive(false);
            }
        }
        foreach (GameObject arrow in arrows)
        {
            arrow.GetComponent<SpriteRenderer>().sprite = arrow0;
        }
        count = 0;
        questionCount = 0;
        letCount = 0;
        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(Question());
        }
    }
    bool IsNotMouseDown()
    {
        return Input.GetMouseButton(0) ||
            Input.GetMouseButton(1) ||
            Input.GetMouseButton(2);

    }
}
