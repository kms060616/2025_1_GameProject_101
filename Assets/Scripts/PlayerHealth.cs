using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;   //최대 생명력
    public int currentLives;   //현재 셩명력

    public float invincibleTime = 1.0f;   //피격 후 무적 시간 (반복 피격 방지)
    public bool isInvincible = false;   //무적 여부의 값
    // Start is called before the first frame update
    void Start()
    {
        currentLives = maxLives;    //생명력 초기화
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))  //미사일과 충돌하면
        {
            currentLives--;
            Destroy(other.gameObject);   //미사일 오프젝트를 없엔다

            if (currentLives <= 0)   //지금 체력이 0이하일경우
            {
                GameOver();
                Invoke("RestartGame", 3.0f);
            }
        }
    }

    void GameOver()
    {
        //플레이어 비활성화  
        gameObject.SetActive(false);  //플레이어 비활성화
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
