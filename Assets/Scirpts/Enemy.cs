using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 100;                    //ü���� ���� �Ѵ�. (����)
    public float Timer = 1.0f;                  //Ÿ�̸� ������ �����Ѵ�. (float)
    public int AttackPoint = 50;                //���ݷ��� ���� �Ѵ�.

    // ���� �������� ������Ʈ �Ǳ� �� �ѹ� ���� �ȴ�.
    void Start()
    {
        Health = 100;                         //�� ��ũ��Ʈ�� ���� �� �� 100���� �����Ѵ�.
    }

    // ���� �������� �� ������ ���� ȣ��ȴ�.
    void Update()
    {
        CharacterHealthup();

        if (Input.GetKeyDown(KeyCode.Space))        //�����̽� Ű�� ������ ��
        {
            Health -= AttackPoint;              //ü�� ����Ʈ�� ���� ����Ʈ ��ŭ ���� ���� �ش�.  (Health = Health - AttackPoint)
        }
        CheckDeath();
    }
    void CharacterHealthup()
    {
        Timer -= Time.deltaTime;             //�ð��� �� �����̸��� ���� ��Ų�� (deltaTime ������ ������ �ð��� �ǹ�)

        if (Timer <= 0)                      //���� Timer�� ��ġ�� 0���Ϸ� ������ ��� (1�ʸ��� ���۵Ǵ� �ൿ�� ���鋚)
        {
            Timer = 1;                      //�ٽ� 1�ʷ� Ÿ�̹��� �ʱ�ȭ �����ش�.
            Health += 10;                   //1�ʸ��� ü���� 10 �÷��ش�.
        }
    }
    public void CharacterHit(int Damage)           //�������� �޴� �Լ��� �����Ѵ�.
    {
        Health -= Damage;               //���� ���ݷ¿� ���� ü���� ���� ��Ų��.
    }
    void CheckDeath()
    {

        if (Health <= 0)         //ü���� 0���� �� ���
        {
            Destroy(gameObject);            //�� ������Ʈ�� �ı� ��Ų��.
        }
    }
}
