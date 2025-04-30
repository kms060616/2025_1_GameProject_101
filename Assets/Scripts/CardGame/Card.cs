using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class Card : MonoBehaviour
{
    public int cardValue;   //ī�� �� (ī�� �ܰ�)
    public Sprite cardImage;   //ī���̹���
    public TextMeshPro cardText;  //ī���ؽ�Ʈ

    //ī�� ���� �ʱ�ȭ �Լ�
    public void InitCard(int value, Sprite Image)
    {
        cardValue = value;
        cardImage = Image;
        //ī�� �̹��� ����
        GetComponent<SpriteRenderer>().sprite = Image;  //�ش� �̹����� ī�忡 ǥ���Ѵ�
            
        //ī�� �ؽ�Ʈ ���� �ִ°��
        if (cardText != null)
        {
            cardText.text = cardValue.ToString();  //ī�� ���� ǥ���Ѵ�.
        }
    }
 




    
}
