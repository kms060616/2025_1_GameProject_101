using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{
    //전역 변수 모든 공이 공유해서 사용 할 수 없음
    public static bool canPlay = true;
    public static bool anyBallMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAllBalls();     //모든공이 움직임을 확인하는 함수

        if(!anyBallMoving && !canPlay)
        {
            canPlay = true;  //모든 공이 멈추면 다시 칠수있게함
            Debug.Log("턴종료 ! 다시 칠수있습니다");
        }
    }

    void CheckAllBalls()
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();
        anyBallMoving = false;

        foreach(SimpleBallController ball in allBalls)  //배열 전체 클래스를 순환하면서
        {
            if(ball.IsMoving())  //공이 움직이고 있는지 확인 하는 함수를 호출
            {
                anyBallMoving = true;  //공이 움직인다고 변수 변경
                break;  //루프문을 빠져 나온다
            }
        }
    }

    public static void OnBallHit()  //공을 플레이 했을때 호출
    {
        canPlay = false;         //다른공이 못움직이게함
        anyBallMoving = true;   //공이 움직이기 시작하기 때문에 bool값 변경
        Debug.Log("턴 시작! 공이 멈출때 까지 기다리세요.");  
    }
}
