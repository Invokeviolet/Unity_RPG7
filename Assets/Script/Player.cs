using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;


public class Player : MonoBehaviour
{
    // 싱글톤
    #region 싱글톤

    // [SerializeField] GameObject myPlayer;

    private static Player Instance = null;
    public static Player INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<Player>();
                if (Instance == null)
                {
                    // 플레이어 생성
                    Debug.Log("싱글턴! 플레이어! 생성!");
                    Instance = Instantiate(Resources.Load<Player>("Player"));
                    Instance.name = Instance.name.Replace("(Clone)", "");
                    //Instance = new GameObject("PlayerCam").AddComponent<PlayerCamera>();                   
                }
            }
            DontDestroyOnLoad(Instance.gameObject);
            return Instance;
        }
    }
    #endregion

    ChangeSceneManager SCENEMANAGER;
    MonsterMarble monsterMarble;

    // 플레이어 위치 받아오기
    private Transform SaveMyPos { get; set; }
    public Transform PlayerPos;
    public Transform ActionPlayerPos;

    Cube cube;
    // 플레이어 죽음 체크
    public bool IsDead { get; set; }
    // 플레이어 점프 힘
    float JumpForce;   
    
    // 플레이어 리지드바디
    private Rigidbody PlayerRB;

    // 플레이어 카메라 위치
    [SerializeField] Camera PlayerCam = null;

    // 플레이어 정보 
    //
    // Player Info Property
    #region Player Info Property

    // 레벨
    public int GetLevel()
    {
        return PLAYERLEVEL;
    }
    public void SetLevel(int level)
    {
        this.PLAYERLEVEL = level;
    }
    // 체력
    public int GetHP()
    {
        return PLAYERCURHP;
    }
    public void SetHP(int hp)
    {
        this.PLAYERCURHP = hp;
    }
    // 경험치
    public int GetEXP()
    {
        return PLAYERCUREXP;
    }
    public void SetEXP(int exp)
    {
        this.PLAYERCUREXP = exp;
    }
    // 공격력
    public int GetAttackPower()
    {
        return PLAYERATTACKPOWER;
    }
    public void SetAttackPower(int attackpower)
    {
        this.PLAYERATTACKPOWER = attackpower;
    }
    // 공격범위
    public int SetAttackRange()
    {
        return PLAYERATTACKRANGE;
    }
    public void SetAttackRange(int attackrange)
    {
        this.PLAYERATTACKRANGE = attackrange;
    }
    // 물약
    public int GetPotion()
    {
        return POTION;
    }
    public void SetPotion(int potion)
    {
        this.POTION += potion;
    }
    // 골드
    public int GetGold()
    {
        if (GOLD <= 0)
        {
            GOLD = 0;
            Shop4BuyAvailable = false;
        }
        return GOLD;
    }
    public void SetGold(int gold)
    {
        this.GOLD -= gold;
    }

    private int PLAYERSPEED;
    private int PLAYERLEVEL;
    public int PLAYERCURHP;
    private int PLAYERMAXHP = 100;
    private int PLAYERCUREXP;
    private int PLAYERMAXEXP = 1000;
    public int PLAYERATTACKPOWER;
    private int PLAYERATTACKRANGE;
    private int POTION;
    private int GOLD;

    public float CURSTAMINAR { get; set; }
    public float MAXSTAMINAR { get; set; }

    public bool Shop4BuyAvailable = true;

    #endregion


    private void Awake()
    {
        monsterMarble = FindObjectOfType<MonsterMarble>();       
        PlayerRB = this.gameObject.GetComponent<Rigidbody>();
        PLAYERSPEED = 8;

        SCENEMANAGER = new ChangeSceneManager();

        PLAYERLEVEL = 1;
        PLAYERMAXHP = 0;
        PLAYERMAXHP = 100;
        PLAYERCURHP = 1 / PLAYERMAXHP;
        PLAYERATTACKPOWER = 1;
        PLAYERATTACKRANGE = 1;
        PLAYERCUREXP = 0;
        PLAYERMAXEXP = 1000;
        POTION = 1;
        GOLD = 100;
        CURSTAMINAR = 0;
        MAXSTAMINAR = 0.5f;
    }


    // 플레이어 레벨 UI
    string MyLV = "";
    // 플레이어 공격력 UI
    string MyAttack = "";


    private void Start()
    {
        // 플레이어 시작 위치
        PlayerPos.transform.position = new Vector3(0, 0.5f, 0);
        this.transform.position = PlayerPos.transform.position;

        PlayerRB = GetComponent<Rigidbody>();
        if (PlayerRB == null)
        {
            this.gameObject.AddComponent<Rigidbody>();
        }
    }


    // Update
    #region Update
    private void Update()
    {
        MyLV = "LV ." + PLAYERLEVEL.ToString();
        MyAttack = PLAYERATTACKPOWER.ToString() + " / " + PLAYERATTACKRANGE.ToString();

        // 인게임 씬이고, 어떤 알림창도 뜨지 않았을 때
        if ((GameManager.INSTANCE.myPlayerInGame == true) && (GameManager.INSTANCE.IsWindowOpen == false))
        {
            PlayerMoveToIngame();
        }
        // 전투 씬이고, 어떤 알림창도 뜨지 않았을 때
        if ((GameManager.INSTANCE.myPlayerAction == true) && (GameManager.INSTANCE.IsWindowOpen == false))
        {
            // 인게임씬 플레이어가 이동되는 문제
            PlayerMoveToAction();
        }
        PlayerCharge(); // 물약 먹었을 때
        UpdatePlayerInfo(); // UI 업데이트
    }
    #endregion 




    private void UpdatePlayerInfo() // 나중에 안쓰면 삭제
    {
        // 플레이어 정보 업데이트 시점 - 전투씬 전,후
        UIManager.INSTANCE.GETPLAYERINFO(MyLV, MyAttack);
    }



    //
    // 플레이어 경험치 및 레벨업
    #region 플레이어 경험치 및 레벨업
    public void ExpUpdate(int exp) // 몬스터 잡는 곳에서 호출
    {
        UIManager.INSTANCE.Player_Exp.value += exp;
        if (PLAYERCUREXP >= PLAYERMAXEXP)
        {
            // 경험치가 최대일 때 레벨업
            PLAYERLEVEL++;
            PLAYERCUREXP = 0;
            PLAYERMAXHP = PLAYERMAXHP * 2;
            PLAYERCURHP = PLAYERMAXHP;
            PLAYERATTACKPOWER = PLAYERATTACKPOWER * 2;
        }
    }
    #endregion
    //


    //
    // 플레이어 인게임 이동
    #region 플레이어 인게임 이동
    void PlayerMoveToIngame()
    {
        // WASD 키로 이동
        if (Input.GetKeyDown((KeyCode.T))) // 앞
        {
            transform.position += Vector3.forward;
            if (transform.position.z >= 9)
            {
                transform.position = new Vector3(transform.position.x, 0.5f, 9);
            }
        }
        else if (Input.GetKeyDown((KeyCode.F))) // 왼 
        {
            transform.position += Vector3.left;
            if (transform.position.x <= 0)
            {
                transform.position = new Vector3(0, 0.5f, transform.position.z);
            }
        }
        else if (Input.GetKeyDown((KeyCode.G))) // 뒤
        {
            transform.position += Vector3.back;
            if (transform.position.z <= 0)
            {
                transform.position = new Vector3(transform.position.x, 0.5f, 0);
            }
        }
        else if (Input.GetKeyDown((KeyCode.H))) // 오른
        {
            transform.position += Vector3.right;
            if (transform.position.x >= 9)
            {
                transform.position = new Vector3(9, 0.5f, transform.position.z);
            }
        }

    }
    #endregion


    //
    //
    // 플레이어 전투씬 이동
    #region 플레이어 전투씬 이동

    protected float hAxis = 0f;
    protected float vAxis = 0f;
    protected bool isMove = false;
    protected Vector3 moveDir;

    void PlayerMoveToAction()
    {
        PlayerCam.gameObject.SetActive(true);

        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // isMove는 hAxis가 0의 근사값일 때 false가 됨.
        isMove = !Mathf.Approximately(hAxis, 0f) || !Mathf.Approximately(vAxis, 0f);

        moveDir = new Vector3(hAxis, 0, vAxis).normalized;

        if (isMove)
        {
            PlayerRB.velocity += PLAYERSPEED * Time.deltaTime * moveDir;
            // 마우스로 회전하면 플레이어가 바라보는 방향도 같이 회전
            // PlayerRB.MovePosition(Time.deltaTime * moveDir * PlayerSpeed);
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.2f);

        }

        // 점프
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // 달리기
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }

        // 공격
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    void Jump()
    {
        JumpForce = 1.5f;
        PlayerRB.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }
    void Run()
    {
        PLAYERSPEED = PLAYERSPEED * 2;
    }

    #endregion
    //


    //
    // 플레이어 공격
    #region 플레이어 공격
    void Attack()
    {

        // GameManager.INSTANCE.GetMonster().HitMonster(10);
        // 스태미너로 공격 횟수에 제한을 둘 필요가 있음
        // UIManager.INSTANCE.StaminarCheck();            

    }
    #endregion
    //


    // 플레이어 체력
    #region
    void PlayerCharge()
    {
        int curPotion = POTION;
        if (Input.GetKeyDown((KeyCode.I)))
        {
            POTION -= 1;
            PLAYERCURHP += 10;

            if (POTION <= 0)
            {
                POTION = 0;
            }
            if (PLAYERCURHP >= PLAYERMAXHP) // HP가 이미 꽉차있을 때 물약 사용 막기
            {
                PLAYERCURHP = PLAYERMAXHP;
                POTION = curPotion;
            }
        }
    }
    #endregion



    //
    // 플레이어 위치 정보 출력
    #region 플레이어 위치 정보 출력

    private void OnCollisionEnter(Collision collision)
    {
        // 맵에 있는 맵큐브랑 충돌하면 맵 정보 출력
        if (collision.gameObject.TryGetComponent<Cube>(out cube) == true) // (collision.collider.CompareTag("Cube"))
        {
            Debug.Log(cube.MAPINFO);
            UIManager.INSTANCE.GETMAPINFO(cube.MAPINFO); // UI에 맵 정보 출력
        }
        if (collision.collider.CompareTag("Store"))
        {
            // 상점으로 이동할건지 물어보는 창 띄우기
            UIManager.INSTANCE.ONQUESTION();

        }
        if (collision.collider.CompareTag("End"))
        {
            Debug.Log("도착!");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<MonsterMarble>(out monsterMarble) == true) //(collision.collider.CompareTag("Monster"))
        {
            // Debug.Log(monster.MOBINFO);

            PlayerPos.transform.position = transform.position;

            // 플레이어 시작 위치
            ActionPlayerPos.transform.position = new Vector3(2000, 0.5f, 0);

            // 충돌했을 때 플레이어 액션씬으로 이동
            SCENEMANAGER.ACTIONFORESTSCENE();

            UIManager.INSTANCE.GETMOBINFO(monsterMarble); // UI에 몬스터 정보 출력
        }
    }

    #endregion
   

    //
    // 플레이어 타격 및 죽음
    #region 플레이어 타격 및 죽음
    public void HitPlayer(int damage)
    {
        PLAYERCURHP -= damage;
        if (PLAYERCURHP <= 0)
        {
            IMDEAD();
        }
    }
    public void IMDEAD()
    {
        IsDead = true;
        UIManager.INSTANCE.Check4WhoIsWin(true);
        // 1 초 뒤 플레이어 졌다는 창 띄우기
        Invoke("UIManager.INSTANCE.RESULTSCENE()", 1f);

    }
    #endregion
}
