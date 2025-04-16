using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float turnsSpeed = 10f;

    [Header("점프 개선 설정")]
    public float fallmultiplier = 2.5f;  //하강 중력 배율
    public float lowJumpMultiplier = 2.0f;  //짧은 점프 배율


    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f; // 지면 관성 시간
    public float coyoteTimeCoundter;  //관성 타이머
    public bool realGrouned = true;  //실제 지면 상태

    [Header("글라이더 설정")]
    public GameObject gliderObject;  //글라이더 오브젝트
    public float gliderFallSpeed = 1.0f;  //글라이더 낙하 속도
    public float gliderMoveSpeed = 7.0f;  //글라이더 이동 속도
    public float gliderMaxtime = 5.0f;  //최대 사용 시간
    public float gliderTimeLeft;  //남은 사용 시간 
    public bool isGliding = false;  //글라이딩 중인지여부


    public bool isGrounded = true;  //땅에있는지 체크

    public int coinCount = 0;    //코인 획득 변수 선언
    public int totalCoins = 5;    //플레이어 강
    
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {


        //글라이더 오브젝트 초기화
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);  //시작시 비활성화
        }

        gliderTimeLeft = gliderMaxtime; //글라이더 시간 초기화

        coyoteTimeCoundter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //지면 감지 안정화
        UpdateGroundedState();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical); 


        if (movement.magnitude > 0.1f)  //임력이 있을때만 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);  //이동방향으로 부드럽게 회전-
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnsSpeed * Time.deltaTime);
        }

        //g키로 글라이더 제어 (누르는동안만 활성화)
        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0) //G키를 누르면서 땅에 있지않고 글라이더 남은 시간이 있을때 
        {
            if(!isGrounded)
            {
                //글라이더 활성화 함수 (아래정의)
                EnableGlider();
            }

            //글라이더 사용 시간 감소
            gliderTimeLeft -= Time.deltaTime;

            //글라이더 시간이 다 되면 비활성화
            if(gliderTimeLeft <= 0)
            {
                //글라이더 비활성화 함수 (아래 정의)
                DisableGlider();
            }

            if(isGliding)  //움직임 처리
            {
                ApplyGliderMovement(moveHorizontal, moveVertical);

            }
            else  //기존 움직임 코드들을 else문안에 넣는다
            {
                rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);


                //착시 점프 높이 구현
                if (rb.velocity.y < 0)
                {
                    rb.velocity += Vector3.up * Physics.gravity.y * (fallmultiplier - 1) * Time.deltaTime;

                }
                else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                }
            }

        }
        else if(isGliding)
        {
            //g키를 때면 글라읻 비활성화
        }

            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);


        //착시 점프 높이 구현
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallmultiplier - 1) * Time.deltaTime;

        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //점프 입력
        if (Input.GetButtonDown("Jump") && isGrounded)           //&& 두 값을 만족할때 -> (스페이스 버튼을 눌렀을때와 isGorund 가 True 일때)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);                   //위쪽으로 설정한 힘만큼 강체에 준다
            isGrounded = false;                   //점프를 하는 순간 땅에서 떨어졌기 때문에 false라고 해준다.
            realGrouned = false;
            coyoteTimeCoundter = 0;

        }

        //지면에 있으면 글라이더 시간 회복 및 글라이더 비활성화
        if(isGrounded)
        {
            if(isGliding)
            {
                DisableGlider();
            }
            //지상에 있을 때 시간 회복
            gliderTimeLeft = gliderMaxtime;
        }

        
    }

    //글라이더 활성화 함수
    void EnableGlider()
    {
        isGliding = true;

        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);
    }

    void DisableGlider()
    {
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
    void OnCollisionEnter(Collision collision)    //충돌 처리함수
    {
        if (collision.gameObject.CompareTag("Ground"))     //충돌이 일어난 물체의 Tag가 Ground인 경우
        {
            isGrounded = true;    //땅과 충돌하면 true로 변경한다.
        }

    }

    //글라이더 이동 적용
    void ApplyGliderMovement(float horizontal, float vertcal)
    {
        //글라이더 효과 : 천천히 떨어지고 수평 방향으로 더 빠르게 이동

        Vector3 gliderVelocity = new Vector3(
            horizontal * gliderMoveSpeed,  //x축
            -gliderFallSpeed,  //y축
            vertcal * gliderMoveSpeed  //z축
            );
    }
    

    


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = false;
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

    //지명 상태 업데이트 함수
    void UpdateGroundedState()
    {
        if(realGrouned)
        {
            coyoteTimeCoundter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            //실제로는 지면에 없지만 코요테 타임 내에 있으면 여전히ㅐ 지면으로 판단
            if (coyoteTimeCoundter > 0)
            {
                coyoteTimeCoundter -= Time.deltaTime;
                    isGrounded = true;
            }
            else 
            {
                isGrounded = false;
            }
        }
    }
}
