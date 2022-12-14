using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSceneCamera : MonoBehaviour
{
    // 카메라 위치
    [Header("카메라 이동 위치 정보")]
    [SerializeField] Transform[] C_Pos; // 본인 위치 배열로 담기

    // 이동 감도
    float cameraMove;
    float speed;
    float time = 0f;



    private void Awake()
    {
        speed = 50f;
    }

    void Start()
    {
        // 카메라 시작 위치 저장        

    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(MOVEPOSSTATE(C_Pos[0].position));
        }
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    StartCoroutine(MOVEPOSSTATE(C_Pos[1].position));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    StartCoroutine(MOVEPOSSTATE(C_Pos[2].position));
        //}

    }

    public enum STATE
    {
        NONE, TITLE, MOVEPOSSTATE
    }

    Coroutine curCoroutine = null;
    STATE curState = STATE.NONE;

    void newSTATE(STATE newState)
    {
        // newState = curState
        if (newState == curState) return;
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
        }
        curState = newState;
        curCoroutine = StartCoroutine(newState.ToString() + "_STATE");
        
    }


    // GameObject -> 위치값 변경 / Camera -> 회전값 변경
    IEnumerator MOVEPOSSTATE(Vector3 target)
    {
        Vector3 temp = target; // temp에 target 위치 담기
        target = new Vector3(this.transform.position.x, target.y+20, this.transform.position.z); // 타겟 위치 좌표 설정 y축은 0으로 고정
        // 목표 위치 - 내 위치 . normalize
        Vector3 dir = (target - gameObject.transform.position).normalized; // 방향벡터 구하기

        while (Vector3.Distance(target, transform.position) >= 1f) // 타겟 위치와 내 위치 사이 거리 구하기
        {
            transform.Translate(dir * speed * Time.deltaTime); // 위에서 계산한 방향으로 이동
            yield return null;
        }
        //this.transform.position = temp;

    }
}
