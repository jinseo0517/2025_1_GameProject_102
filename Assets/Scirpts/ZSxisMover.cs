using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//5초 동안 5의 스피드로 앞으로 이동하고 사라지는 컴포넌트 클래스 (오브젝트에서의 앞z축
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
        if (timer < 0)      //간이만료되면
        {
            Destroy(gameObject);    //자기 자신을 파괴한ㄷ가
        }
    }
}
