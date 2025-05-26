using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBallController : MonoBehaviour
{
    [Header("기본설정")]
    public float power = 10f;       //타격 힘
    public Sprite arrowSprite;      //화살표 이미지

    private Rigidbody rb;           //공의 물리
    private GameObject arrow;       //화살표 오브젝트
    private bool isDragging = false;    //드래그 중인지
    private Vector3 startPos;           //드래그 시작 위치

    //턴관리를 위한 전역 변수 (모든 공이 공유)
    static bool isAnyBallPlaying = false;       //어떤 공이라도 턴 중인지
    static bool isAnyBallMoving = false;       //어떤 공이라도 움직이는지


    // Start is called before the first frame update
    void Start()
    {
        SetupBall();
    }

    // Update is called once per frame
    void Update()
    {
        Handlelnput();
        UpdateArrow();

    }

    void SetupBall()        //공 설정하기
    {
        rb = GetComponent<Rigidbody>();     //물리 컴포넌트 가져오기
        if(rb == null)
            rb = gameObject.AddComponent<Rigidbody>();      //없을경우 붙여준다

        //물리설정
        rb.mass = 1;
        rb.drag = 1;
    }

    public bool IsMoving()          //공이 움직이고 있는지 확인
    {
        return rb.velocity.magnitude > 0.2f;    //공이 속도를 가지고 있으면 움직인다고 판단
    }

    void Handlelnput()
    {
        //턴 매니저가 허용하지 않으면 조작불가
        if (!SimpleTurnManager.canPlay) return;

        //다른 공이 움직일때
        if (SimpleTurnManager.anyBallMoving) return;


        //공이 움직이고 있으면 조작 불가
        if (IsMoving()) return;

        if (Input.GetMouseButtonDown(0))        //마우스 버튼 클릭
        {
            StartDrag();
        }
        if (Input.GetMouseButtonUp(0) && isDragging)      //마우스 버튼 떼기
        {
            Shoot();
        }
    }

    void StartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                startPos = Input.mousePosition;
                CreatArrow();
                Debug.Log("드래그 시작");
            }
        }
    }

    void Shoot()
    {
        Vector3 mouseDelta = Input.mousePosition - startPos;
        float force = mouseDelta.magnitude * 0.01f * power;

        if (force < 5) force = 5;

        Vector3 direction = new Vector3(-mouseDelta.x, 0, -mouseDelta.y).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);

        //턴 매니저에게 공을 쳤다고 알림
        SimpleTurnManager.OnBallHit();

        //정리

        isDragging = false;
        Destroy(arrow);
        arrow = null;

        Debug.Log("발사! 힘 : " + force);
    }

    void CreatArrow()
    {
        if(arrow != null)
        {
            Destroy(arrow);
        }

        arrow = new GameObject("Arrow");
        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();

        sr.sprite = arrowSprite;
        sr.color = Color.green;
        sr.sortingOrder = 10;

        arrow.transform.position = transform.position + Vector3.up;
        arrow.transform.localScale = Vector3.one;
    }

    void UpdateArrow()
    {
        if (!isDragging || arrow == null) return;

        Vector3 mouseDelta = Input.mousePosition - startPos;        //마우스 이동 거리 계산
        float distance = mouseDelta.magnitude;

        float size = Mathf.Clamp(distance * 0.01f, 0.5f, 2f);       //화살표 크기변경(힘에따라)
        arrow.transform.localScale = Vector3.one * size;

        SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();   //화살표 색상변경 (초록-> 빨강)
        float colorRatio = Mathf.Clamp01(distance * 0.005f);
        sr.color = Color.Lerp(Color.green, Color.red, colorRatio);

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);

        if(distance > 10f)
        {
            Vector3 direction = new Vector3(-mouseDelta.x, 0, -mouseDelta.y);
            //2D 평면 ( 위에서 본 시점 )에서 di
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(90, angle, 0);
        }
    }
}
