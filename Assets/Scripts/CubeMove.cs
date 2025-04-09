using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,0, -moveSpeed * Time.deltaTime);

        if (transform.position.z < -20)    //큐브가 z축 -20이하로 갔는지확인  
        {
            Destroy(gameObject);   //자기 자신 제거
        }
    }
}
