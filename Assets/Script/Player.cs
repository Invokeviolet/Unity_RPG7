using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;


public class Player : MonoBehaviour
{
    // �̱���
    #region �̱���

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
                    // �÷��̾� ����
                    Debug.Log("�̱���! �÷��̾�! ����!");
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

    // �÷��̾� ��ġ �޾ƿ���
    private Transform SaveMyPos { get; set; }
    public Transform PlayerPos;
    public Transform ActionPlayerPos;

    Cube cube;
    // �÷��̾� ���� üũ
    public bool IsDead { get; set; }
    // �÷��̾� ���� ��
    float JumpForce;   
    
    // �÷��̾� ������ٵ�
    private Rigidbody PlayerRB;

    // �÷��̾� ī�޶� ��ġ
    [SerializeField] Camera PlayerCam = null;

    // �÷��̾� ���� 
    //
    // Player Info Property
    #region Player Info Property

    // ����
    public int GetLevel()
    {
        return PLAYERLEVEL;
    }
    public void SetLevel(int level)
    {
        this.PLAYERLEVEL = level;
    }
    // ü��
    public int GetHP()
    {
        return PLAYERCURHP;
    }
    public void SetHP(int hp)
    {
        this.PLAYERCURHP = hp;
    }
    // ����ġ
    public int GetEXP()
    {
        return PLAYERCUREXP;
    }
    public void SetEXP(int exp)
    {
        this.PLAYERCUREXP = exp;
    }
    // ���ݷ�
    public int GetAttackPower()
    {
        return PLAYERATTACKPOWER;
    }
    public void SetAttackPower(int attackpower)
    {
        this.PLAYERATTACKPOWER = attackpower;
    }
    // ���ݹ���
    public int SetAttackRange()
    {
        return PLAYERATTACKRANGE;
    }
    public void SetAttackRange(int attackrange)
    {
        this.PLAYERATTACKRANGE = attackrange;
    }
    // ����
    public int GetPotion()
    {
        return POTION;
    }
    public void SetPotion(int potion)
    {
        this.POTION += potion;
    }
    // ���
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


    // �÷��̾� ���� UI
    string MyLV = "";
    // �÷��̾� ���ݷ� UI
    string MyAttack = "";


    private void Start()
    {
        // �÷��̾� ���� ��ġ
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

        // �ΰ��� ���̰�, � �˸�â�� ���� �ʾ��� ��
        if ((GameManager.INSTANCE.myPlayerInGame == true) && (GameManager.INSTANCE.IsWindowOpen == false))
        {
            PlayerMoveToIngame();
        }
        // ���� ���̰�, � �˸�â�� ���� �ʾ��� ��
        if ((GameManager.INSTANCE.myPlayerAction == true) && (GameManager.INSTANCE.IsWindowOpen == false))
        {
            // �ΰ��Ӿ� �÷��̾ �̵��Ǵ� ����
            PlayerMoveToAction();
        }
        PlayerCharge(); // ���� �Ծ��� ��
        UpdatePlayerInfo(); // UI ������Ʈ
    }
    #endregion 




    private void UpdatePlayerInfo() // ���߿� �Ⱦ��� ����
    {
        // �÷��̾� ���� ������Ʈ ���� - ������ ��,��
        UIManager.INSTANCE.GETPLAYERINFO(MyLV, MyAttack);
    }



    //
    // �÷��̾� ����ġ �� ������
    #region �÷��̾� ����ġ �� ������
    public void ExpUpdate(int exp) // ���� ��� ������ ȣ��
    {
        UIManager.INSTANCE.Player_Exp.value += exp;
        if (PLAYERCUREXP >= PLAYERMAXEXP)
        {
            // ����ġ�� �ִ��� �� ������
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
    // �÷��̾� �ΰ��� �̵�
    #region �÷��̾� �ΰ��� �̵�
    void PlayerMoveToIngame()
    {
        // WASD Ű�� �̵�
        if (Input.GetKeyDown((KeyCode.T))) // ��
        {
            transform.position += Vector3.forward;
            if (transform.position.z >= 9)
            {
                transform.position = new Vector3(transform.position.x, 0.5f, 9);
            }
        }
        else if (Input.GetKeyDown((KeyCode.F))) // �� 
        {
            transform.position += Vector3.left;
            if (transform.position.x <= 0)
            {
                transform.position = new Vector3(0, 0.5f, transform.position.z);
            }
        }
        else if (Input.GetKeyDown((KeyCode.G))) // ��
        {
            transform.position += Vector3.back;
            if (transform.position.z <= 0)
            {
                transform.position = new Vector3(transform.position.x, 0.5f, 0);
            }
        }
        else if (Input.GetKeyDown((KeyCode.H))) // ����
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
    // �÷��̾� ������ �̵�
    #region �÷��̾� ������ �̵�

    protected float hAxis = 0f;
    protected float vAxis = 0f;
    protected bool isMove = false;
    protected Vector3 moveDir;

    void PlayerMoveToAction()
    {
        PlayerCam.gameObject.SetActive(true);

        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // isMove�� hAxis�� 0�� �ٻ簪�� �� false�� ��.
        isMove = !Mathf.Approximately(hAxis, 0f) || !Mathf.Approximately(vAxis, 0f);

        moveDir = new Vector3(hAxis, 0, vAxis).normalized;

        if (isMove)
        {
            PlayerRB.velocity += PLAYERSPEED * Time.deltaTime * moveDir;
            // ���콺�� ȸ���ϸ� �÷��̾ �ٶ󺸴� ���⵵ ���� ȸ��
            // PlayerRB.MovePosition(Time.deltaTime * moveDir * PlayerSpeed);
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.2f);

        }

        // ����
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // �޸���
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }

        // ����
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
    // �÷��̾� ����
    #region �÷��̾� ����
    void Attack()
    {

        // GameManager.INSTANCE.GetMonster().HitMonster(10);
        // ���¹̳ʷ� ���� Ƚ���� ������ �� �ʿ䰡 ����
        // UIManager.INSTANCE.StaminarCheck();            

    }
    #endregion
    //


    // �÷��̾� ü��
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
            if (PLAYERCURHP >= PLAYERMAXHP) // HP�� �̹� �������� �� ���� ��� ����
            {
                PLAYERCURHP = PLAYERMAXHP;
                POTION = curPotion;
            }
        }
    }
    #endregion



    //
    // �÷��̾� ��ġ ���� ���
    #region �÷��̾� ��ġ ���� ���

    private void OnCollisionEnter(Collision collision)
    {
        // �ʿ� �ִ� ��ť��� �浹�ϸ� �� ���� ���
        if (collision.gameObject.TryGetComponent<Cube>(out cube) == true) // (collision.collider.CompareTag("Cube"))
        {
            Debug.Log(cube.MAPINFO);
            UIManager.INSTANCE.GETMAPINFO(cube.MAPINFO); // UI�� �� ���� ���
        }
        if (collision.collider.CompareTag("Store"))
        {
            // �������� �̵��Ұ��� ����� â ����
            UIManager.INSTANCE.ONQUESTION();

        }
        if (collision.collider.CompareTag("End"))
        {
            Debug.Log("����!");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<MonsterMarble>(out monsterMarble) == true) //(collision.collider.CompareTag("Monster"))
        {
            // Debug.Log(monster.MOBINFO);

            PlayerPos.transform.position = transform.position;

            // �÷��̾� ���� ��ġ
            ActionPlayerPos.transform.position = new Vector3(2000, 0.5f, 0);

            // �浹���� �� �÷��̾� �׼Ǿ����� �̵�
            SCENEMANAGER.ACTIONFORESTSCENE();

            UIManager.INSTANCE.GETMOBINFO(monsterMarble); // UI�� ���� ���� ���
        }
    }

    #endregion
   

    //
    // �÷��̾� Ÿ�� �� ����
    #region �÷��̾� Ÿ�� �� ����
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
        // 1 �� �� �÷��̾� ���ٴ� â ����
        Invoke("UIManager.INSTANCE.RESULTSCENE()", 1f);

    }
    #endregion
}
