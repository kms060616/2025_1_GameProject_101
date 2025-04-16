using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�⺻ �̵� ����")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float turnsSpeed = 10f;

    [Header("���� ���� ����")]
    public float fallmultiplier = 2.5f;  //�ϰ� �߷� ����
    public float lowJumpMultiplier = 2.0f;  //ª�� ���� ����


    [Header("���� ���� ����")]
    public float coyoteTime = 0.15f; // ���� ���� �ð�
    public float coyoteTimeCoundter;  //���� Ÿ�̸�
    public bool realGrouned = true;  //���� ���� ����

    [Header("�۶��̴� ����")]
    public GameObject gliderObject;  //�۶��̴� ������Ʈ
    public float gliderFallSpeed = 1.0f;  //�۶��̴� ���� �ӵ�
    public float gliderMoveSpeed = 7.0f;  //�۶��̴� �̵� �ӵ�
    public float gliderMaxtime = 5.0f;  //�ִ� ��� �ð�
    public float gliderTimeLeft;  //���� ��� �ð� 
    public bool isGliding = false;  //�۶��̵� ����������


    public bool isGrounded = true;  //�����ִ��� üũ

    public int coinCount = 0;    //���� ȹ�� ���� ����
    public int totalCoins = 5;    //�÷��̾� ��
    
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {


        //�۶��̴� ������Ʈ �ʱ�ȭ
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);  //���۽� ��Ȱ��ȭ
        }

        gliderTimeLeft = gliderMaxtime; //�۶��̴� �ð� �ʱ�ȭ

        coyoteTimeCoundter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //���� ���� ����ȭ
        UpdateGroundedState();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical); 


        if (movement.magnitude > 0.1f)  //�ӷ��� �������� ȸ��
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);  //�̵��������� �ε巴�� ȸ��-
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnsSpeed * Time.deltaTime);
        }

        //gŰ�� �۶��̴� ���� (�����µ��ȸ� Ȱ��ȭ)
        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0) //GŰ�� �����鼭 ���� �����ʰ� �۶��̴� ���� �ð��� ������ 
        {
            if(!isGrounded)
            {
                //�۶��̴� Ȱ��ȭ �Լ� (�Ʒ�����)
                EnableGlider();
            }

            //�۶��̴� ��� �ð� ����
            gliderTimeLeft -= Time.deltaTime;

            //�۶��̴� �ð��� �� �Ǹ� ��Ȱ��ȭ
            if(gliderTimeLeft <= 0)
            {
                //�۶��̴� ��Ȱ��ȭ �Լ� (�Ʒ� ����)
                DisableGlider();
            }

            if(isGliding)  //������ ó��
            {
                ApplyGliderMovement(moveHorizontal, moveVertical);

            }
            else  //���� ������ �ڵ���� else���ȿ� �ִ´�
            {
                rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);


                //���� ���� ���� ����
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
            //gŰ�� ���� �۶�� ��Ȱ��ȭ
        }

            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);


        //���� ���� ���� ����
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallmultiplier - 1) * Time.deltaTime;

        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //���� �Է�
        if (Input.GetButtonDown("Jump") && isGrounded)           //&& �� ���� �����Ҷ� -> (�����̽� ��ư�� ���������� isGorund �� True �϶�)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);                   //�������� ������ ����ŭ ��ü�� �ش�
            isGrounded = false;                   //������ �ϴ� ���� ������ �������� ������ false��� ���ش�.
            realGrouned = false;
            coyoteTimeCoundter = 0;

        }

        //���鿡 ������ �۶��̴� �ð� ȸ�� �� �۶��̴� ��Ȱ��ȭ
        if(isGrounded)
        {
            if(isGliding)
            {
                DisableGlider();
            }
            //���� ���� �� �ð� ȸ��
            gliderTimeLeft = gliderMaxtime;
        }

        
    }

    //�۶��̴� Ȱ��ȭ �Լ�
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
    void OnCollisionEnter(Collision collision)    //�浹 ó���Լ�
    {
        if (collision.gameObject.CompareTag("Ground"))     //�浹�� �Ͼ ��ü�� Tag�� Ground�� ���
        {
            isGrounded = true;    //���� �浹�ϸ� true�� �����Ѵ�.
        }

    }

    //�۶��̴� �̵� ����
    void ApplyGliderMovement(float horizontal, float vertcal)
    {
        //�۶��̴� ȿ�� : õõ�� �������� ���� �������� �� ������ �̵�

        Vector3 gliderVelocity = new Vector3(
            horizontal * gliderMoveSpeed,  //x��
            -gliderFallSpeed,  //y��
            vertcal * gliderMoveSpeed  //z��
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
            Debug.Log($"���� ���� : {coinCount}/{totalCoins}");
        }

        //������ ���� �� ���� �α� ���
        if(other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            Debug.Log("���� Ŭ����");
            //���� �Ϸ� ���� �߰� ���
        }
    }

    //���� ���� ������Ʈ �Լ�
    void UpdateGroundedState()
    {
        if(realGrouned)
        {
            coyoteTimeCoundter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            //�����δ� ���鿡 ������ �ڿ��� Ÿ�� ���� ������ �������� �������� �Ǵ�
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
