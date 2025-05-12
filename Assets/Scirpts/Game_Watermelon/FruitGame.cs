using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitGame : MonoBehaviour
{
    public GameObject[] fruitPrefabs;       //과일 프리팹 배열 (익스펙터에서 할당)

    public float[] fruitSizes = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f };     //과일 크기 배열

    public GameObject currentFruit;        //현재들고있는과일
    public int currentFruitType;

    public float fruitStartHeight = 6f;     //과일 시작높이 (인스펙터에서 조절가능)

    public float gameWidth = 5f;            //게임판 정보

    public bool isGameOver = false;         //게임 상태

    public Camera mainCamera;               //카메라 참조(마우스 위치 변환에 필요)

    public float fruitTimer;                //잰 시간 설정을 위한 타이머

    public float gameHeight;                //게임 높이 설정

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;       //메인카메라 참조 가져오기

        SpawnNewFruit();                //게임시작시 첫 과일 생성
        fruitTimer = -3.0f;     //타이머 시간을 -3으로 보낸다
    }

    // Update is called once per frame
    void Update()
    {
        //게임 오버면 종료
        if (isGameOver)
            return;

        if (fruitTimer >= 0)                         //타이머가 0보다 클경우
            fruitTimer -= Time.deltaTime;

        if(fruitTimer < 0 && fruitTimer > -2)       //타이머 시간이 0과 -2사이에 있을떄 잰을하거
        {
            CheckGameOver();
            SpawnNewFruit();
            fruitTimer = -3.0f;                     //타이머시간을 -3으로 보낸다
        }

        if (currentFruit != null)       //현재 과일이 있을떄만처리
        {
            Vector3 mousePosition = Input.mousePosition;        //마우스 위치를 따라 x좌표만 이동시키기위해 사용
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 newPosition = currentFruit.transform.position;      //과일위치 업데이트 (x좌표만,y는 그대로 유지)
            newPosition.x = worldPosition.x;

            float halfFruitSize = fruitSizes[currentFruitType] / 2;     //과일이 원이기 떄문에 반값을 나눠서바구니밖으로 안나가게설정
            if (newPosition.x < -gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }
            if (newPosition.x > gameWidth / 2 - halfFruitSize)
            {
                newPosition.x = gameWidth / 2 - halfFruitSize;
            }
            currentFruit.transform.position = newPosition;     //과일 좌표갱신 
        }

        //마우스를 좌클릭하면 과일 떨어뜨리기
        if(Input.GetMouseButtonDown(0) && fruitTimer == -3.0f)
        {
            DropFruit();
         }
    }

    public void MergeFruits(int fruitType , Vector3 position)
    {
        if(fruitType < fruitPrefabs.Length -1)      //마지막 과일 타입이 아니라면
        {
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType + 1] , position, Quaternion.identity);       //다음단계과일생성

            newFruit.transform.localScale = new Vector3(fruitSizes[fruitType + 1], fruitSizes[fruitType + 1], 1.0f);

            //추가 점수 로직
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

            currentFruit = null;        //현재 들고 있는 과일 해체

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
                //과일이 정지   상태이고 높은 위치에 있다면
                if (rb != null && rb.velocity.magnitude < 0.1f && allFruits[i].transform.position.y > gameOverHeight)
                {
                    //게임오버
                    isGameOver = true;
                    Debug.Log("게임오버");

                    break;
                }
            }
        }
    }
}
