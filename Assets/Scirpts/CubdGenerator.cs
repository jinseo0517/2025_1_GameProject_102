using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{

    public GameObject cubePrefab;           //������ ť�� ������
    public int totalCubes = 10;             //�� ������ ť�갳��
    public float cubeSpacing = 1.0f;        //ť�� ����

    // Start is called before the first frame update
    void Start()
    {
        GenCube();
    }

    public void GenCube()
    {
        Vector3 myPposition = transform.position;       //��ũ��Ʈ�� ���� ������Ʈ�� ��ġ( x,y,z)
        GameObject firstCube = Instantiate(cubePrefab, myPposition, Quaternion.identity);       //ù���� ť�����

        for (int i = 1; i < totalCubes; i++)
        {
            //�� ��ġ���� z������ �������� ������ ��ġ�� ����
            Vector3 position = new Vector3(myPposition.x, myPposition.y, myPposition.z + (i * cubeSpacing)); 
            Instantiate(cubePrefab, position, Quaternion.identity);  //ť�� ����
        }
    }
   
}
