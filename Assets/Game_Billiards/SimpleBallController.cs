using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SimpleBallController : MonoBehaviour
{

    [Header("기본 설정")]
    public float power = 10f;   //타격 힘
    public Sprite arrowSprite;  //화살표 이미지

    private Rigidbody rb;       //공의 물리
    private GameObject arrow;   //화살표 오브젝트
    private bool isDragging = false;   //드래그 중인지 확인 하는 Bool
    private Vector3 startPos;          //드래그 시작 위치

    // Start is called before the first frame update
    void Start()
    {
        SetupBall();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateArrow();
    }

    void SetupBall()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        //물리 설정
        rb.mass = 2;
        rb.drag = 1;
    }

        public bool IsMoving()  //공이 움직이고있는지 확인
    {
        return rb.velocity.magnitude > 0.2f;  //공이 속도를 가지고있다면 움직이고 있다고 판단
    }

    void HandleInput()
    {
        if (!SimpleTurnManager.canPlay) return;
        if (SimpleTurnManager.anyBallMoving) return;
        if (IsMoving()) return;   //공이 움직이고 있으면 조작불가
        
        if (Input.GetMouseButtonDown(0))  //마우스 클릭 시작
        {
            StartDrag();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)   //드래그 중이였는데 마우ㅅ 버튼업했을때
        {
            Shoot();
        }


        

    }
    void Shoot()
    {
        Vector3 mouseDelta = Input.mousePosition - startPos;
        float force = mouseDelta.magnitude * 0.01f * power;

        if (force < 5) force = 5;

        Vector3 direction = new Vector3(-mouseDelta.x, 0, - mouseDelta.y).normalized;

        rb.AddForce(direction * force, ForceMode.Impulse);

        SimpleTurnManager.OnBallHit();

        isDragging = false;
        Destroy(arrow);
        arrow = null;

        Debug.Log("발사! 힘 : " + force);
    }

    void CreateArrow()         //화살표 만들기
    {
        if (arrow != null)
        {
            Destroy(arrow);    //화살표가 있는 경우 제거
        }

        arrow = new GameObject("Arrow");    //빈 오브젝트 생성 -> new GameObject(이름))새 화살표 만들기
        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();    //새로만든 오브젝트에 랜더러를 붙인다

        sr.sprite = arrowSprite;
        sr.color = Color.green;
        sr.sortingOrder = 10;


        arrow.transform.position = transform.position + Vector3.up;  //화살표 위치를 잡아준다
        arrow.transform.localScale = Vector3.one;
        
    }


    void UpdateArrow()   //화살표 업데이트
    {
        if (!isDragging || arrow == null) return;   

        Vector3 mouseDelta = Input.mousePosition - startPos;   //마우스 이동 거리 계산
        float distance = mouseDelta.magnitude;

        float size = Mathf.Clamp(distance * 0.01f, 0.5f, 2.0f);  //화살표 크기를 힘에 따라 변경
        arrow.transform.localScale = Vector3.one * size;

        SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();  //이동거리가 길어질수록 초록에서 빨강으로 변한다
        float colorRatio = Mathf.Clamp01(distance * 0.005f);
        sr.color = Color.Lerp(Color.green, Color.red, colorRatio);

        if(distance > 10f)//최소거리 이상 드래그 했을때
        {
            Vector3 direction = new Vector3(-mouseDelta.x, 0, -mouseDelta.y);

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(90, angle, 0);
        }

                
    }

    void StartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                startPos = Input.mousePosition;
                CreateArrow();
                Debug.Log("드래그시작");
            }
        }
    }

}
