using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{

    public GameObject cubePrefab;           //생성할 큐브 프리팹
    public int totalCubes = 10;             //총 생설할 큐브개수
    public float cubeSpacing = 1.0f;        //큐브 간격

    // Start is called before the first frame update
    void Start()
    {
        GenCube();
    }

    public void GenCube()
    {
        Vector3 myPposition = transform.position;       //스크립트가 붙은 오브젝트의 위치( x,y,z)
        GameObject firstCube = Instantiate(cubePrefab, myPposition, Quaternion.identity);       //첫번쨰 큐브생성

        for (int i = 1; i < totalCubes; i++)
        {
            //내 위치에서 z축으로 일정간격 떨어진 위치에 생성
            Vector3 position = new Vector3(myPposition.x, myPposition.y, myPposition.z + (i * cubeSpacing)); 
            Instantiate(cubePrefab, position, Quaternion.identity);  //큐브 생성
        }
    }
   
}
