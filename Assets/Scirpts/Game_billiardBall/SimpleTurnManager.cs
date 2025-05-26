using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{
    //전역 변수 (모든 공이 공유)
    public static bool canPlay = true;      //공이 칠수있는지
    public static bool anyBallMoving = false;       //어떤 공이라도 움직이는지

    

    // Update is called once per frame
    void Update()
    {
        CheckAllBalls();

        if(!anyBallMoving && !canPlay)
        {
            canPlay = true;
            Debug.Log("턴 종료! 다시 칠 수 있습니다.");
        }
    }

    void CheckAllBalls()
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();     //씬에 있는 모든 공 찾기
        anyBallMoving = false;

        foreach(SimpleBallController ball in allBalls)
        {
            if(ball.IsMoving())
            {
                anyBallMoving = true;
                break;
            }
        }
    }

    public static void OnBallHit()
    {
        canPlay = false;
        anyBallMoving = true;
        Debug.Log("턴 시작! 공이 멈출 떄 까지 기다리세요");
    }
}
