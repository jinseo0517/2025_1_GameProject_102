using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;  //이동 속도 변수 설정
    public float jumpForce = 5.0f;  //점프의 침값을 바꿔준다

    public bool isGrounded = true;  //방에 있는지 체크 하는 변수 (true/false)

    public int coinCount = 0;  //코인 획득 변수 선언
    public int totalCoins = 5;  //총 코인 획득 필요 변수선언

    public Rigidbody rb;  //플레이어 강체를 선언

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //속도값으로 직접이동
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //점프입력
        if(Input.GetButtonDown("Jump") && isGrounded)  //&& 두 값이 True 일때-> (Jump 버튼 {보통 스페이스바} 와 땅 위에 있을때
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode. Impulse);  //위쪽으로 설정한 힘만큼 
            isGrounded = false;  //점프를 하는순간 땅에서떨어졌기떄문에 False라고한다

        }
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
}
