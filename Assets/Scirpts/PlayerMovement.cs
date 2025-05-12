using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerMovement : MonoBehaviour
{
    [Header("�⺻ �̵� ����")]
    public float moveSpeed = 5.0f;  //�̵� �ӵ� ���� ����
    public float jumpForce = 7.0f;//������ ħ���� �ٲ��ش�
    public float turnSpeed = 10f; //ȸ�� �ӵ�

    [Header("���� ���� ����")]
    public float fllMultiplier = 2.5f; //�ϰ� �߷� ����
    public float lowJumpMultiplier = 2.0f; //ª�� ���� ����

    [Header("���� ���� ����")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrouned = true;

    [Header("�۶��̴� ����")]
    public GameObject gilderObject; //�۶��̴� ������Ʈ
    public float gilderFallSpeed = 1.0f; //�۶��̴� ���� �ӵ�
    public float gilderMoveSpeed = 7.0f; //�۶��̴� �̵� �ӵ�
    public float  gilderMaxTime = 5.0f; //�ִ� ��� �ð�
    public float gilderTimeLeft; //���� ��� �ð�
    public bool isGilding = false; //�۶��̵� ������ ����

       
    public bool isGrounded = true;  //�濡 �ִ��� üũ �ϴ� ���� (true/false)

    public int coinCount = 0;  //���� ȹ�� ���� ����
    public int totalCoins = 5;  //�� ���� ȹ�� �ʿ� ��������

    public Rigidbody rb;  //�÷��̾� ��ü�� ����

    // Start is called before the first frame update
    void Start()
    {

        if (gilderObject != null)
        {
            gilderObject.SetActive(false); //���� �� ��Ȱ��ȭ
        }

        gilderTimeLeft = gilderMaxTime; //�۶��̴� �ð� �ʱ�ȭ


        coyoteTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //���� ���� ����ȭ
        UpdateGroundedState();

        //������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //�̵����� ����
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }


        //gŰ�� �۶��̴� ���� (������ ���ȸ� Ȱ��ȭ)
        if(Input.GetKey(KeyCode.G) && !isGrounded && gilderTimeLeft > 0)
        {
            if(!isGilding)
            {

                //�۶��̴� Ȱ��ȭ �Լ�
                EnableGilder();
            }

            //�۶��̴� ��� �ð� ����
            gilderTimeLeft -= Time.deltaTime;

            //�۶��̴� �ð��� �� �Ǹ� ��Ȱ��ȭ
            if(gilderTimeLeft <= 0)
            {
                //�۶��̴� ��Ȱ��ȭ �Լ�
                DisalbeGilder();
            } 

        }
        else if(isGilding)
        {
            //gŰ�� ���� �۶��̴� ��Ȱ��ȭ
            DisalbeGilder();
        }

        if(isGilding) //������ ó��
        {
            //�۶��̴� ����� �̸�
            ApplyGilderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            //�ӵ������� �����̵�
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            //���� ���� ���� ����
            if (rb.velocity.y < 0)
            {
                //�ϰ��� �߷� ��ȭ
                rb.velocity += Vector3.up * Physics.gravity.y * (fllMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))//��� �� ���� ��ư�� ���� ���� ����
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            //�����Է�
            if (Input.GetButtonDown("Jump") && isGrounded)  //&& �� ���� True �϶�-> (Jump ��ư {���� �����̽���} �� �� ���� ������
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  //�������� ������ ����ŭ 
                isGrounded = false;  //������ �ϴ¼��� �������������⋚���� False����Ѵ�
                realGrouned = false;
                coyoteTimeCounter = 0;
            }
        }



            //�ӵ������� �����̵�
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //���� ���� ���� ����
        if (rb.velocity.y < 0)
        {
            //�ϰ��� �߷� ��ȭ
            rb.velocity += Vector3.up * Physics.gravity.y * (fllMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))//��� �� ���� ��ư�� ���� ���� ����
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }    

        //�����Է�
        if(Input.GetButtonDown("Jump") && isGrounded)  //&& �� ���� True �϶�-> (Jump ��ư {���� �����̽���} �� �� ���� ������
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode. Impulse);  //�������� ������ ����ŭ 
            isGrounded = false;  //������ �ϴ¼��� �������������⋚���� False����Ѵ�
            realGrouned = false;
            coyoteTimeCounter = 0;

            
        }
        //���鿡 ������ �۶��̴� �ð� ȸ�� �� �۶��̴� ��Ȱ��ȭ  If(isGrounded)
    {
            if (isGilding)
            {
                DisalbeGilder();
            }

            //���� ������ ü�� ȸ��
            gilderTimeLeft = gilderMaxTime;
        }
    }

    //���鿡 ������ �۶��̴� �ð� ȸ�� �� �۶��̴� ��Ȱ��ȭ
    

    //�۶��̴� Ȱ��ȭ �Լ�
    
    void EnableGilder()
    {
        isGilding = true;

        //�۶��̴� ������Ʈ ǥ��
        if (gilderObject != null)
        {
            gilderObject.SetActive(true);
        }
        //�ϰ� �ӵ� �ʱ�ȭ
        rb.velocity = new Vector3(rb.velocity.x, -gilderFallSpeed, rb.velocity.z);
    }

    //�۶��̴� ��Ȱ��ȭ �Լ�
    void DisalbeGilder()
    {
        isGilding = false;

        //�۶��̴� ������Ʈ �����
        if (gilderObject != null)
        {
            gilderObject.SetActive(false);
        }

        //���� �����ϵ��� �߷� ����
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    //�۶��̴� �̵� ����
    void ApplyGilderMovement(float horizontal, float vertical)
    {
        //�۶��̴� ȿ�� : õõ�� �������� ���� �������� �� ������ �̵�

        Vector3 gilderVelocity = new Vector3(
            horizontal * gilderMoveSpeed,
            gilderFallSpeed,
            vertical * gilderMoveSpeed
            );
        rb.velocity = gilderVelocity;

    }
    void OnCollisionEnter(Collision collision) //�浹�� �Ͼ���� ȣ��Ǵ� �Լ�

    {
        if (collision.gameObject.tag == "Ground")  //�浹�� �Ͼ���� ��ü�� Tag�� Ground�� ���
        {
            isGrounded = true;  //���� �浹������ true�� �������ش�
        }
    }

    void OnTriggerEnter(Collider other)  //Ʈ���� ���� �ȿ� ���Դٸ� �Ͻ��ϴ� �Լ�
    {
        //���μ���
        if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"���� ���� : {coinCount}/{totalCoins}");
        }

        if (other.CompareTag("Door") && coinCount >= totalCoins)
        {
            Debug.Log("���� Ŭ����");
            //���� �Ϸ� ���� �� Scene ��ȯ �Ѵ�.
        }
    }

    //���� ���� ������Ʈ �Լ�

    void UpdateGroundedState()
    {
        if(realGrouned)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;

        }
        else
        {
            //�����δ� ���鿡 ������ �ڿ��� Ÿ�� ���� ������ ������ �������� �Ǵ�
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
   void OnCollsionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = true;
        }    

    }

    void OnCollisionStay(Collision collision)
    {
       if (collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = true;
        }
    }



    void OnCollsionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = false;
        }
    }
}

internal class isGrounded
{
}