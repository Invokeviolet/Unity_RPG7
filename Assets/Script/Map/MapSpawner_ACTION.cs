using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner_ACTION : MonoBehaviour
{
    // 싱글톤
    #region 싱글톤

    private static MapSpawner_ACTION Instance = null;
    public static MapSpawner_ACTION INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<MapSpawner_ACTION>();
            }

            return Instance;
        }
    }
    #endregion

    // 맵 지면 생성
    [SerializeField] GameObject MapCubePrefab;

    // 몬스터 프리팹 받아오기
    [SerializeField] BossMonster MonsterParentPrefab;
    [SerializeField] Monster MonsterPrefab;

    GameObject MAPCUBE = null;
    BossMonster ActionMobParents = null;
    Monster ActionMob;

    // 몬스터 수
    private int CurActionMobCount = 0;
    private int MaxActionMobCount = 10;


    Vector3 MobPos;
    Vector3 MobParentPos;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        MobPos = new Vector3(2000,0,10);
        MobParentPos = new Vector3(2000,0,100);
    }

    void Start()
    {
        MAPCUBE = Instantiate(MapCubePrefab, transform.position, Quaternion.identity,transform);        
        ActionMobParents = Instantiate(MonsterParentPrefab, MobParentPos, Quaternion.identity, transform);
        ActionMob = Instantiate(MonsterPrefab, MobPos, Quaternion.identity, transform);
       
    }
    
    void Update()
    {
        
    }

    // 패턴 1
    // 지상 모체
    // 하위 몬스터는 모체주변에서 생성되어 플레이어에게 이동
    // 플레이어가 무기를 사용해서 잡음
    // 하위 몬스터를 다 잡으면 5초 뒤에 재생성
    // 모체를 다 잡을 때까지 재생성 반복

    // 패턴 2
    // 공중 모체
    // 하위 몬스터 생성 없음
    // 랜덤한 위치에 스킬 공격이 주어짐
    // 플레이어는 이동키로 이동하면서 피하기 가능
    // 한 사이클 당 스킬 공격은 2번, 4번, 6번, 8번, 10번까지 이어짐.
    // 맵에 랜덤하게 공격아이템 생성 -> 먹으면 모체에게 데미지
    // 일정 공격횟수를 채우면 모체가 공격가능한 상태가 됨.
    // 모체가 죽으면 게임 끝

    // 패턴 3
    // 모체
    // 하위 몬스터 생성
    // 



    // 난이도에 따른 보상(추후 변경)
    // EASY - 골드 100, 경험치 100, 아이템 1
    // NORMAL - 골드 200, 경험치 200, 아이템 2
    // HARD - 골드 300, 경험치 300, 아이템 3

}
