using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject coinPrefads;      //���� �������� ���� �Ѵ�.
    public GameObject MissildPrfabs;    //�̻��� �������� ���� �Ѵ�.

    [Header("���� Ÿ�̹� ����")]
    public float minSpawnlnterval = 0.5f;       //�ּ� ���� ����(��)
    public float maxSpawnlnterval = 2 / 0f;     //�ִ� ���� ����(��)

    [Header("���� ���� Ȯ�� ����:")]
    [Range(0, 100)]
    public int coinSpawnChance = 50;        //50% Ȯ���� ������ �����ȴ�

    public float timer = 0.0f;      //Ÿ�̸�
    public float nextSpawnTime;     //���� ���� �ð�

    // Start is called before the first frame update
    void Start()
    {
        SetNextSpawnTime();     //�Լ� ȣ��
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;        //�ð��� 0���� ���� �����Ѵ�.

        if(timer >= nextSpawnTime)      //���� �ð��� �Ǹ� ������Ʈ�� ���� �Ѵ�
        {
            SpawnObject();      
            timer = 0.0f;       //�ð��� �ʱ�ȭ �����ش�
            SetNextSpawnTime();     //�ٽ� �Լ� ȣ��
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnlnterval, maxSpawnlnterval);       //�ּ�-�ִ� ������ ������ �ð� ����
    }

    void SpawnObject()
    {
        Transform spawnTransform = transform;       //������ ������Ʈ�� ��ġ�� ȸ������ �����´�. (���� ����)

        //Ȯ���� ���� �����Ǵ¹̻��ϻ���
        int randomValue = Random.Range(0, 100);
        if (randomValue < coinSpawnChance)
        {
            Instantiate(coinPrefads, spawnTransform.position, spawnTransform.rotation);
        }
        else
        {
            Instantiate(MissildPrfabs, spawnTransform.position, spawnTransform.rotation);
        }
    }
}
