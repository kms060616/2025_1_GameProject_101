using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour


{
    public int fruitType;               //����Ÿ��
     
    public bool hasMered = false;       //������ ���������� Ȯ���ϴ� �÷���

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMered)                    //�̹� ������ ������ ����
            return;
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();   //�ٸ� ���ϰ� �浹�ߴ��� Ȯ��

        if(otherFruit != null && !otherFruit.hasMered && otherFruit.fruitType == fruitType)   //�浹�Ѱ��� ���� �����̰� Ÿ���� ���ٸ� (���������ʾ������)
        { 
            hasMered = true;   //�������ٰ� ǥ��
            otherFruit.hasMered = true;

            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f; //�ΰ����� �߰���ġ ���

            FruitGame gameManger = FindObjectOfType<FruitGame>();
            if(gameManger != null)
            {
                gameManger.MergeFruits(fruitType, mergePosition);
            }


            //���� �޴������� ���� �����Ȱ��� ȣ��

            //���� ����
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }
}
