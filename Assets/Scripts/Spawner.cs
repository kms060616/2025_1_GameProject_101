using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject coinPrefabs;  //동전 프리팹
    public GameObject MissilePrefabs;

    [Header("스폰 타이밍 설정")]
    public float minSpawnInterval = 0.5f;  //
    public float maxspawnInterval = 2.0f;

    [Header("동전 스폰 확률 설정")]      // 유니티 ui에서 할 수 있게 한다
    [Range(0, 100)]                        // 유니티 ui에서 할 수 있게 한다
    public int coinSpawnChance = 50;     //동전이 생성되는 확률 0 100

    public float timer = 0.0f;
    public float nextSpawnTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > nextSpawnTime )
        {
            SpawnObject();
            timer = 0.0f;    //시간을 초기화 시켜준다
            SetNextSpawnTime();     //다시 함수를 실행
        }
    }

    void SpawnObject()
    {
        Transform spawnTransform = transform;   //스포너 오브젝트ㅇ와 회전값을 가져온다 

        //확률에 따라 동전 또는 미사일 생성
        int randomValue = Random.Range(0, 100);
        if( randomValue < coinSpawnChance)
        {
            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);
        }
        else
        {
            Instantiate(MissilePrefabs, spawnTransform.position, spawnTransform.rotation);
        }

            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);
    }

    void SetNextSpawnTime()
    {
       //최소 최대 사이의 랜덤한 시간 설정
        nextSpawnTime = Random.Range(minSpawnInterval, maxspawnInterval);
    }
}
