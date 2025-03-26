using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public bool isGrounded = true;

    public int coinCount = 0;    //���� ȹ�� ���� ����
    public int totalCoins = 5;    //�÷��̾� ��
    
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

        //���� �Է�
        if (Input.GetButtonDown("Jump") && isGrounded)           //&& �� ���� �����Ҷ� -> (�����̽� ��ư�� ���������� isGorund �� True �϶�)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);                   //�������� ������ ����ŭ ��ü�� �ش�
            isGrounded = false;                   //������ �ϴ� ���� ������ �������� ������ false��� ���ش�.
        }
    }

    void OnCollisionEnter(Collision collision)    //�浹 ó���Լ�
    {
        if (collision.gameObject.tag == "Ground")     //�浹�� �Ͼ ��ü�� Tag�� Ground�� ���
        {
            isGrounded = true;    //���� �浹�ϸ� true�� �����Ѵ�.
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
}
