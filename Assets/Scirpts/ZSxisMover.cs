using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//5�� ���� 5�� ���ǵ�� ������ �̵��ϰ� ������� ������Ʈ Ŭ���� (������Ʈ������ ��z��
public class ZAxisMover : MonoBehaviour

{
    public float speed = 5.0f;
    public float timer = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer < 0)      //���̸���Ǹ�
        {
            Destroy(gameObject);    //�ڱ� �ڽ��� �ı��Ѥ���
        }
    }
}
