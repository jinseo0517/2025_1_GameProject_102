using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5.0f;  //이동 속도 변수 설정
    public float jumpForce = 7.0f;//점프의 침값을 바꿔준다
    public float turnSpeed = 10f; //회전 속도

    [Header("점프 개선 설정")]
    public float fllMultiplier = 2.5f; //하강 중력 배율
    public float lowJumpMultiplier = 2.0f; //짧은 점프 배율

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrouned = true;

    [Header("글라이더 설정")]
    public GameObject gilderObject; //글라이더 오브젝트
    public float gilderFallSpeed = 1.0f; //글라이더 낙하 속도
    public float gilderMoveSpeed = 7.0f; //글라이더 이동 속도
    public float  gilderMaxTime = 5.0f; //최대 사용 시간
    public float gilderTimeLeft; //남은 사용 시간
    public bool isGilding = false; //글라이딩 중인지 여부

       
    public bool isGrounded = true;  //방에 있는지 체크 하는 변수 (true/false)

    public int coinCount = 0;  //코인 획득 변수 선언
    public int totalCoins = 5;  //총 코인 획득 필요 변수선언

    public Rigidbody rb;  //플레이어 강체를 선언

    // Start is called before the first frame update
    void Start()
    {

        if (gilderObject != null)
        {
            gilderObject.SetActive(false); //시작 시 비활성화
        }

        gilderTimeLeft = gilderMaxTime; //글라이더 시간 초기화


        coyoteTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //지면 감지 안정화
        UpdateGroundedState();

        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //이동방향 백터
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }


        //g키로 글라이더 제어 (누르는 동안만 활성화)
        if(Input.GetKey(KeyCode.G) && !isGrounded && gilderTimeLeft > 0)
        {
            if(!isGilding)
            {

                //글라이더 활성화 함수
                EnableGilder();
            }

            //글라이더 사용 시간 감소
            gilderTimeLeft -= Time.deltaTime;

            //글라이더 시간이 다 되면 비활성화
            if(gilderTimeLeft <= 0)
            {
                //글라이더 비활성화 함수
                DisalbeGilder();
            } 

        }
        else if(isGilding)
        {
            //g키를 때면 글라이더 비활성화
            DisalbeGilder();
        }

        if(isGilding) //움직임 처리
        {
            //글라이더 사용중 이름
            ApplyGilderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            //속도값으로 직접이동
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            //착시 점프 높이 구현
            if (rb.velocity.y < 0)
            {
                //하강시 중력 강화
                rb.velocity += Vector3.up * Physics.gravity.y * (fllMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))//상승 중 점프 버튼을 떼면 낮게 점프
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            //점프입력
            if (Input.GetButtonDown("Jump") && isGrounded)  //&& 두 값이 True 일때-> (Jump 버튼 {보통 스페이스바} 와 땅 위에 있을때
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  //위쪽으로 설정한 힘만큼 
                isGrounded = false;  //점프를 하는순간 땅에서떨어졌기떄문에 False라고한다
                realGrouned = false;
                coyoteTimeCounter = 0;
            }
        }



            //속도값으로 직접이동
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //착시 점프 높이 구현
        if (rb.velocity.y < 0)
        {
            //하강시 중력 강화
            rb.velocity += Vector3.up * Physics.gravity.y * (fllMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))//상승 중 점프 버튼을 떼면 낮게 점프
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }    

        //점프입력
        if(Input.GetButtonDown("Jump") && isGrounded)  //&& 두 값이 True 일때-> (Jump 버튼 {보통 스페이스바} 와 땅 위에 있을때
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode. Impulse);  //위쪽으로 설정한 힘만큼 
            isGrounded = false;  //점프를 하는순간 땅에서떨어졌기떄문에 False라고한다
            realGrouned = false;
            coyoteTimeCounter = 0;

            
        }
        //지면에 있으면 글라이더 시간 회복 및 글라이더 비활성화  If(isGrounded)
    {
            if (isGilding)
            {
                DisalbeGilder();
            }

            //지상에 있을때 체력 회복
            gilderTimeLeft = gilderMaxTime;
        }
    }

    //지면에 있으면 글라이더 시간 회복 및 글라이더 비활성화
    

    //글라이더 활성화 함수
    
    void EnableGilder()
    {
        isGilding = true;

        //글라이더 오브젝트 표시
        if (gilderObject != null)
        {
            gilderObject.SetActive(true);
        }
        //하강 속도 초기화
        rb.velocity = new Vector3(rb.velocity.x, -gilderFallSpeed, rb.velocity.z);
    }

    //글라이더 비활성화 함수
    void DisalbeGilder()
    {
        isGilding = false;

        //글라이더 오브젝트 숨기기
        if (gilderObject != null)
        {
            gilderObject.SetActive(false);
        }

        //즉지 낙하하도록 중력 적용
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    //글라이더 이동 적용
    void ApplyGilderMovement(float horizontal, float vertical)
    {
        //글라이더 효과 : 천천히 떨어지고 수평 방향으로 더 빠르기 이동

        Vector3 gilderVelocity = new Vector3(
            horizontal * gilderMoveSpeed,
            gilderFallSpeed,
            vertical * gilderMoveSpeed
            );
        rb.velocity = gilderVelocity;

    }
    void OnCollisionEnter(Collision collision) //충돌이 일어났을떄 호출되는 함수

    {
        if (collision.gameObject.tag == "Ground")  //충돌이 일어났을때 불체의 Tag가 Ground인 경우
        {
            isGrounded = true;  //땅과 충돌했을떄 true로 변경해준다
        }
    }

    void OnTriggerEnter(Collider other)  //트리거 영역 안에 들어왔다를 암시하는 함수
    {
        //코인수집
        if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"코인 수집 : {coinCount}/{totalCoins}");
        }

        if (other.CompareTag("Door") && coinCount >= totalCoins)
        {
            Debug.Log("게임 클리어");
            //이후 완료 연출 및 Scene 전환 한다.
        }
    }

    //지명 상태 업데이트 함수

    void UpdateGroundedState()
    {
        if(realGrouned)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;

        }
        else
        {
            //실제로는 지면에 없지만 코요테 타임 내에 있으면 여전히 지면으로 판단
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