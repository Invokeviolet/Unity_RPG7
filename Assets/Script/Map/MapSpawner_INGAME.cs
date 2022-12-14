using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class MapSpawner_INGAME : MonoBehaviour
{
    // 맵을 싱글톤으로 선언해서 씬이동이 있어도 저장되도록 -> 플레이어 위치도 여기에 저장

    // 싱글톤
    #region 싱글톤

    private static MapSpawner_INGAME Instance = null;
    public static MapSpawner_INGAME INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<MapSpawner_INGAME>();
            }
            
            return Instance;
        }
    }
    #endregion

    // 시작점 도착점
    Vector3 zeroPos;
    Vector3 finPos;


    // 플레이어
    [SerializeField] Player PlayerPrefab;
    // 몬스터
    [SerializeField] MonsterMarble MonsterPrefab;
    // 맵 큐브
    [SerializeField] Cube CubePrefab;
    // 바나나존
    // [SerializeField] Banana BananaPrefab; 


    // 1개 -> 10 x 10 배열로 받아서 생성    
    MonsterMarble MOBS = null;
    Cube MAPTILE = null;
    Player PLAYER = null; // 플레이어 생성
    GameObject EmptyBox;
    Cube[,] cubes;
    MonsterMarble[] mobs;

    // 큐브 갯수
    private int count = 10;
    // 스테이지 표시 -> 1
    private int stage = 1;
    // 몬스터 수
    private int CurMobCount = 0;
    private int MaxMobCount = 30;
    // 랜덤 위치 생성 해서 배열에 저장
    int random;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // 큐브에 값 넣어야 함
        CubePrefab.transform.position = new Vector3(0f, 0f, 0f);
        zeroPos = new Vector3(0f, 0f, 0f);
        finPos = new Vector3(9f, 0f, 9f);


        // 큐브 담을 배열
        cubes = new Cube[count, count];
        // 몬스터 담을 배열
        mobs = new MonsterMarble[MaxMobCount];

        // 로컬 스케일 받아오기
        float xScale = CubePrefab.transform.localScale.x;
        float zScale = CubePrefab.transform.localScale.z;

        // 큐브에 랜덤값 배정해주기
        int CubeRandom = 0;
        int MonsterRandom = 0;
        int[] rand = new int[10];
        int range = 5;


        // 각 이름별로 바꿔서 빈 게임오브젝트로 생성
        for (int i = 0; i < (int)MAPINFO.START; i++)
        {
            EmptyBox = new GameObject((MAPINFO)i + "BOX");
        }


        // 상점 좌표 생성
        for (int i = 0; i < rand.Length; i++)
        {
            rand[i] = Random.Range(range - 5, range);

            if (i % 2 == 0)
                range += 5;
            else
                range = 5;
        }
        Vector3 area1 = new Vector3(rand[0], 0, rand[4]);
        Vector3 area2 = new Vector3(rand[1], 0, rand[5]);
        Vector3 area3 = new Vector3(rand[2], 0, rand[7]);
        Vector3 area4 = new Vector3(rand[3], 0, rand[6]);

        // 좌표가 0,0 이거나 9,9 일때
        while ((area1 == zeroPos) || (area1 == finPos))
        {
            Debug.Log(area1);
            rand[0] = Random.Range(range - 5, range);
            rand[4] = Random.Range(range - 5, range);
        }
        while ((area2 == zeroPos) || (area2 == finPos))
        {
            Debug.Log(area2);
            rand[1] = Random.Range(range, range + 5);
            rand[5] = Random.Range(range - 5, range);
        }
        while ((area3 == zeroPos) || (area3 == finPos))
        {
            Debug.Log(area3);
            rand[2] = Random.Range(range - 5, range + 5);
            rand[7] = Random.Range(range, range);
        }
        while ((area4 == zeroPos) || (area4 == finPos))
        {
            Debug.Log(area4);
            rand[3] = Random.Range(range, range + 5);
            rand[6] = Random.Range(range, range + 5);
        }

        // 4구역에 각 1개씩 상점 생성 로직
        #region 4구역에 각 1개씩 상점 생성 로직
        // (0,0) <= (5,5) / rnd[0], rnd[4]
        // (5,0) <= (10, 5) / rnd[1], rnd[5]
        // (0,5) <= (10,5) / rnd[2], rnd[7]
        // (5,5) <= (10,10) / rnd[3], rnd[6]
        #endregion


        // 몬스터를 생성하는 칸 : 숲 늪 땅(random)

        // 시작, 도착, 숲, 늪, 땅
        for (int i = 0; i < cubes.GetLength(0); i++)
        {
            for (int j = 0; j < cubes.GetLength(1); j++)
            {
                CubeRandom = Random.Range(0, 3);

                Vector3 vec3 = new Vector3(j * xScale, 0, i * zScale);

                // 큐브 맵타일 생성
                MAPTILE = Instantiate(CubePrefab, vec3, Quaternion.identity);
                MAPTILE.Init(vec3, (MAPINFO)CubeRandom);
                Debug.Log(MAPTILE);
                // 큐브 좌표에 맵타일 넣기
                cubes[i, j] = MAPTILE;
                cubes[i, j].transform.SetParent(this.gameObject.transform);

                // 빈 오브젝트에 하위객체로 넣어줄것
                //cubes[i, j].transform.SetParent(EmptyBox.transform);

                // 빈 게임오브젝트 생성한 곳에 찾아서 넣어주기
                /*string cubeObject = (MAPINFO)CubeRandom + "BOX";
                EmptyBox = GameObject.Find(cubeObject);
                cubes[i, j].transform.SetParent(EmptyBox.transform);*/

                // 맵 크기만큼 돌려서 리스트에 넣고, 생성은 랜덤으로 20마리 생성

                // 시작점 출력
                if (j == 0 && i == 0)
                {
                    cubes[i, j].Init(vec3, (MAPINFO)4);
                    //MAPTILE.transform.parent = this.transform;
                    //cubes[i, j].transform.SetParent(EmptyBox.transform);
                }
                // 도착점 출력
                if (j == 9 && i == 9)
                {
                    cubes[i, j].Init(vec3, (MAPINFO)5);
                    //MAPTILE.transform.parent = this.transform;
                    //cubes[i, j].transform.SetParent(EmptyBox.transform);
                }
                
            }

        }

        // 상점 빈오브젝트 생성해서 담기
        /*string ShopObject = MAPINFO.SHOP + "BOX";
        EmptyBox = GameObject.Find(ShopObject);*/

        // 상점 4군데 위치 덮어쓰기
        cubes[(int)area1.z, (int)area1.x].Init(area1, MAPINFO.SHOP);
        cubes[(int)area1.z, (int)area1.x].transform.SetParent(this.gameObject.transform);

        cubes[(int)area2.z, (int)area2.x].Init(area2, MAPINFO.SHOP);
        cubes[(int)area2.z, (int)area2.x].transform.SetParent(this.gameObject.transform);

        cubes[(int)area3.z, (int)area3.x].Init(area3, MAPINFO.SHOP);
        cubes[(int)area3.z, (int)area3.x].transform.SetParent(this.gameObject.transform);

        cubes[(int)area4.z, (int)area4.x].Init(area4, MAPINFO.SHOP);
        cubes[(int)area4.z, (int)area4.x].transform.SetParent(this.gameObject.transform);
                

        //몬스터 생성 로직
        int mobOverLap = 0;
        for (int i = 0; i < MaxMobCount; i++)
        {
            Vector3 randvec = new Vector3(Random.Range(0, 9) * xScale, 0, Random.Range(0, 9) * zScale);
            MonsterRandom = Random.Range(0, 3);

            // 상점 예외처리
            if (cubes[(int)area1.z, (int)area1.x].transform.position == randvec) { --i; continue; }
            if (cubes[(int)area2.z, (int)area2.x].transform.position == randvec) { --i; continue; }
            if (cubes[(int)area3.z, (int)area3.x].transform.position == randvec) { --i; continue; }
            if (cubes[(int)area4.z, (int)area4.x].transform.position == randvec) { --i; continue; }
            // 시작 도착 예외처리
            if (zeroPos == randvec) { --i; continue; }
            if (finPos == randvec) { --i; continue; }
            // 중복 예외처리
            for (int j = 0; j < CurMobCount; j++)
            {
                mobOverLap = j;
                if (mobs[j].transform.position == randvec)
                { break; }
            }
            if (CurMobCount > mobOverLap + 1) { --i; continue; }
            // 이미 생성된 몬스터 위치랑 새로 생성된 몬스터 위치를 비교해야함.

            MOBS = Instantiate(MonsterPrefab, Vector3.zero, Quaternion.identity);
            mobs[CurMobCount] = MOBS;
            MOBS.Init(Vector3.zero, (MOBINFO)MonsterRandom);
            mobs[CurMobCount].transform.position = randvec;

            mobs[CurMobCount].transform.SetParent(gameObject.transform);
            CurMobCount++;
        }

        // 플레이어 생성
        /*PLAYER = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        Debug.Log("프리팹! 플레이어! 생성!");
        PLAYER.name = PlayerPrefab.name.Replace("(Clone)", "");
        PLAYER.transform.SetParent(gameObject.transform);*/
    }
    private void Start()
    {
     /*   Debug.Log(Player.INSTANCE.PlayerCam.gameObject.activeSelf);
        Player.INSTANCE.PlayerCam.gameObject.SetActive(false);*/        
    }
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(Player.INSTANCE.PlayerCam.gameObject.activeSelf);
            Player.INSTANCE.PlayerCam.gameObject.SetActive(true);
        }*/
    }
}
