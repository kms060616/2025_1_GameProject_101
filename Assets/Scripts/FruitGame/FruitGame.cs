using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGame : MonoBehaviour
{

    public GameObject[] fruitPrefabs;       //과일 배열 선언

    public float[] fruitSize = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f };  //과일 크기 선언

    public GameObject currentFruit;         //현재 들고 있는 과일
    public int currentFruitType;            

    public float fruitStartHeight = 6.0f;   // 과일 시작시 높이 설정 (인스펙트에서 조절가능)
    public float gameWidth = 5.0f;          //게임판 너비
    public bool isGameOver = false;         //게임 상태
    public Camera mainCamera;               //카메라 참조 (마우스 위치 변환에 필요)

    public float fruitTimer;  //잰 시간 설정을 위한 타이머

    public float gameheight;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewFruit();
        fruitTimer = -3.0f;
        gameheight = fruitStartHeight + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

        if (isGameOver) return;

        if(fruitTimer >= 0)
        {
            fruitTimer -= Time.deltaTime;
        }

        if (fruitTimer < 0 && fruitTimer > -2)
        {
            CheckGameOver();
            SpawnNewFruit();
            fruitTimer = -3.0f;
        }

        if (currentFruit != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 newPosition = currentFruit.transform.position;
            newPosition.x = worldPosition.x;

            float halfFruitSize = fruitSize[currentFruitType] / 2f;
            if (newPosition.x < -gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }
            if (newPosition.x > gameWidth / 2  + halfFruitSize)
            {
                newPosition.x = gameWidth / 2 + halfFruitSize;
            }

            currentFruit.transform.position = newPosition;
        }

        if(Input.GetMouseButtonDown(0) && fruitTimer == -3.0f)
        {
            DropFruit();

        }
        
    }

    void SpawnNewFruit()
    {
        if(!isGameOver)  //게임오버가 아닐때만 새 과일 생성
        {
            currentFruitType = Random.Range(0, 3);  //0~2사이 랜덤 과일 타입

            Vector3 mousePosition = Input.mousePosition; //마우스 위치를 받아온다
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 spawnPosition = new Vector3(worldPosition.x, fruitStartHeight, 0);

            float halfFruitSize = fruitSize[currentFruitType] / 2;

            currentFruit = Instantiate(fruitPrefabs[currentFruitType], spawnPosition, Quaternion.identity);
            currentFruit.transform.localScale = new Vector3(fruitSize[currentFruitType], fruitSize[currentFruitType], 1);

            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
            }
        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.gravityScale = 1f;

            currentFruit = null;

            fruitTimer = 1.0f;
        }
    }

    public void MergeFruits(int fruitType, Vector3 position)
    {
        if(fruitType < fruitPrefabs.Length -1)
        {
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType + 1], position, Quaternion.identity);
            newFruit.transform.localScale = new Vector3(fruitSize[fruitType + 1], fruitSize[fruitType + 1], 1.0f);
        }
    }

    public void CheckGameOver()
    {
        Fruit[] allFruits = FindObjectsOfType<Fruit>();   //씬에 있는 모든 고일 컴포먼트가 붙어 있는 오브젝트를 가져온다 작은게임에서만 사용 비용이쌤

        float gameOverHeight = gameheight;

        for(int i = 0; i < allFruits.Length; ++i)
        {
            if (allFruits[i] != null)
            {
                Rigidbody2D rb = allFruits[i].GetComponent<Rigidbody2D>();

                if (rb != null && rb.velocity.magnitude < 0.1f && allFruits[i].transform.position.y > gameOverHeight)
                {
                    isGameOver = true;
                    Debug.Log("game over");

                    break;
                }
            }
        }
    }
}
