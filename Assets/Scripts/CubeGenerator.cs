using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int totalcubes = 10;
    public float cubeSpacing = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {

        GenCube();

    }

    // Update is called once per frame

    public void GenCube()
    {
        Vector3 myPosition = transform.position; //스크립트가 붙은 오브젝트의 위치 (x,y,z)

        GameObject firestCube = Instantiate(cubePrefab, myPosition , Quaternion.identity);

        for(int i = 1; i < totalcubes; i++)
        {
            Vector3 position = new Vector3(myPosition.x, myPosition.y, myPosition.z + (i * cubeSpacing)); 
                Instantiate(cubePrefab , position , Quaternion.identity);
        }


    }    
}
