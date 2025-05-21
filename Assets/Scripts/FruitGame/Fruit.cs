using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour


{
    public int fruitType;               //과일타입
     
    public bool hasMered = false;       //과일이 합쳐졌는지 확인하는 플래그

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMered)                    //이미 합쳐진 과일은 무시
            return;
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();   //다른 과일과 충돌했는지 확인

        if(otherFruit != null && !otherFruit.hasMered && otherFruit.fruitType == fruitType)   //충돌한것이 같은 과일이고 타일이 같다면 (합쳐지지않았을경우)
        { 
            hasMered = true;   //합쳐졌다고 표시
            otherFruit.hasMered = true;

            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f; //두과일의 중간위치 계산

            FruitGame gameManger = FindObjectOfType<FruitGame>();
            if(gameManger != null)
            {
                gameManger.MergeFruits(fruitType, mergePosition);
            }


            //게임 메니저에서 마지 구현된것을 호출

            //과일 제거
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }
}
