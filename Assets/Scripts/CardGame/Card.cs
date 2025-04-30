using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class Card : MonoBehaviour
{
    public int cardValue;   //카드 값 (카드 단계)
    public Sprite cardImage;   //카드이미지
    public TextMeshPro cardText;  //카드텍스트

    //카드 정보 초기화 함수
    public void InitCard(int value, Sprite Image)
    {
        cardValue = value;
        cardImage = Image;
        //카드 이미지 설정
        GetComponent<SpriteRenderer>().sprite = Image;  //해당 이미지를 카드에 표시한다
            
        //카드 텍스트 설정 있는경우
        if (cardText != null)
        {
            cardText.text = cardValue.ToString();  //카드 값을 표시한다.
        }
    }
 




    
}
