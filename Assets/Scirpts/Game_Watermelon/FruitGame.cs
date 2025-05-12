using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitGame : MonoBehaviour
{
    public GameObject[] fruitPrefabs;       //���� ������ �迭 (�ͽ����Ϳ��� �Ҵ�)

    public float[] fruitSizes = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f };     //���� ũ�� �迭

    public GameObject currentFruit;        //�������ִ°���
    public int currentFruitType;

    public float fruitStartHeight = 6f;     //���� ���۳��� (�ν����Ϳ��� ��������)

    public float gameWidth = 5f;            //������ ����

    public bool isGameOver = false;         //���� ����

    public Camera mainCamera;               //ī�޶� ����(���콺 ��ġ ��ȯ�� �ʿ�)

    public float fruitTimer;                //�� �ð� ������ ���� Ÿ�̸�

    public float gameHeight;                //���� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;       //����ī�޶� ���� ��������

        SpawnNewFruit();                //���ӽ��۽� ù ���� ����
        fruitTimer = -3.0f;     //Ÿ�̸� �ð��� -3���� ������
    }

    // Update is called once per frame
    void Update()
    {
        //���� ������ ����
        if (isGameOver)
            return;

        if (fruitTimer >= 0)                         //Ÿ�̸Ӱ� 0���� Ŭ���
            fruitTimer -= Time.deltaTime;

        if(fruitTimer < 0 && fruitTimer > -2)       //Ÿ�̸� �ð��� 0�� -2���̿� ������ �����ϰ�
        {
            CheckGameOver();
            SpawnNewFruit();
            fruitTimer = -3.0f;                     //Ÿ�̸ӽð��� -3���� ������
        }

        if (currentFruit != null)       //���� ������ ��������ó��
        {
            Vector3 mousePosition = Input.mousePosition;        //���콺 ��ġ�� ���� x��ǥ�� �̵���Ű������ ���
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 newPosition = currentFruit.transform.position;      //������ġ ������Ʈ (x��ǥ��,y�� �״�� ����)
            newPosition.x = worldPosition.x;

            float halfFruitSize = fruitSizes[currentFruitType] / 2;     //������ ���̱� ������ �ݰ��� �������ٱ��Ϲ����� �ȳ����Լ���
            if (newPosition.x < -gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }
            if (newPosition.x > gameWidth / 2 - halfFruitSize)
            {
                newPosition.x = gameWidth / 2 - halfFruitSize;
            }
            currentFruit.transform.position = newPosition;     //���� ��ǥ���� 
        }

        //���콺�� ��Ŭ���ϸ� ���� ����߸���
        if(Input.GetMouseButtonDown(0) && fruitTimer == -3.0f)
        {
            DropFruit();
         }
    }

    public void MergeFruits(int fruitType , Vector3 position)
    {
        if(fruitType < fruitPrefabs.Length -1)      //������ ���� Ÿ���� �ƴ϶��
        {
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType + 1] , position, Quaternion.identity);       //�����ܰ���ϻ���

            newFruit.transform.localScale = new Vector3(fruitSizes[fruitType + 1], fruitSizes[fruitType + 1], 1.0f);

            //�߰� ���� ����
        }
    }

    

    void SpawnNewFruit()
    {
        if(!isGameOver)
        {
            currentFruitType = Random.Range(0, 3);

            Vector3 mousePostion = Input.mousePosition;
            Vector3 worldPoision = mainCamera.ScreenToWorldPoint(mousePostion);

            Vector3 spawnPosition = new Vector3(worldPoision.x, fruitStartHeight, 0);

            float halfFruitSize = fruitSizes[currentFruitType] / 2;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

            currentFruit = Instantiate(fruitPrefabs[currentFruitType], spawnPosition, Quaternion.identity);

            currentFruit.transform.localScale = new Vector3(fruitSizes[currentFruitType], fruitSizes[currentFruitType], 1f);

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
            }

        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if(rb !=null)
        {
            rb.gravityScale = 1f;

            currentFruit = null;        //���� ��� �ִ� ���� ��ü

            fruitTimer = 1.0f;
        }
    }

    void CheckGameOver()
    {
        Fruit[] allFruits = FindObjectsOfType<Fruit>();

        float gameOverHeight = gameHeight;

        for(int i = 0; i < allFruits.Length; i++)
        {
            if (allFruits[i] !=null)
            {
                Rigidbody2D rb = allFruits[i].GetComponent<Rigidbody2D>();
                //������ ����   �����̰� ���� ��ġ�� �ִٸ�
                if (rb != null && rb.velocity.magnitude < 0.1f && allFruits[i].transform.position.y > gameOverHeight)
                {
                    //���ӿ���
                    isGameOver = true;
                    Debug.Log("���ӿ���");

                    break;
                }
            }
        }
    }
}
