using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public bool isGrounded = true;

    public int coinCount = 0;    //코인 획득 변수 선언
    public int totalCoins = 5;    //플레이어 강
    
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded)           //&& 두 값을 만족할때 -> (스페이스 버튼을 눌렀을때와 isGorund 가 True 일때)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);                   //위쪽으로 설정한 힘만큼 강체에 준다
            isGrounded = false;                   //점프를 하는 순간 땅에서 떨어졌기 때문에 false라고 해준다.
        }
    }

    void OnCollisionEnter(Collision collision)    //충돌 처리함수
    {
        if (collision.gameObject.tag == "Ground")     //충돌이 일어난 물체의 Tag가 Ground인 경우
        {
            isGrounded = true;    //땅과 충돌하면 true로 변경한다.
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"코인 수집 : {coinCount}/{totalCoins}");
        }

        //목적지 도착 시 종료 로그 출력
        if(other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            Debug.Log("게임 클리어");
            //게임 완료 로직 추가 기능
        }
    }
}
