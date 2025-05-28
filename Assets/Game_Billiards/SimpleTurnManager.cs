using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{
    //���� ���� ��� ���� �����ؼ� ��� �� �� ����
    public static bool canPlay = true;
    public static bool anyBallMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAllBalls();     //������ �������� Ȯ���ϴ� �Լ�

        if(!anyBallMoving && !canPlay)
        {
            canPlay = true;  //��� ���� ���߸� �ٽ� ĥ���ְ���
            Debug.Log("������ ! �ٽ� ĥ���ֽ��ϴ�");
        }
    }

    void CheckAllBalls()
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();
        anyBallMoving = false;

        foreach(SimpleBallController ball in allBalls)  //�迭 ��ü Ŭ������ ��ȯ�ϸ鼭
        {
            if(ball.IsMoving())  //���� �����̰� �ִ��� Ȯ�� �ϴ� �Լ��� ȣ��
            {
                anyBallMoving = true;  //���� �����δٰ� ���� ����
                break;  //�������� ���� ���´�
            }
        }
    }

    public static void OnBallHit()  //���� �÷��� ������ ȣ��
    {
        canPlay = false;         //�ٸ����� �������̰���
        anyBallMoving = true;   //���� �����̱� �����ϱ� ������ bool�� ����
        Debug.Log("�� ����! ���� ���⶧ ���� ��ٸ�����.");  
    }
}
