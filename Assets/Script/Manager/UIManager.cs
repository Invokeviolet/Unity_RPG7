using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Windows;

public class UIManager : MonoBehaviour
{
    // UI
    #region UI

    // Canvas UI
    #region Canvas
    [Header("[CANVAS]")]
    [SerializeField] Canvas TITLECANVAS;
    [SerializeField] Canvas GENERALCANVAS;
    [SerializeField] Canvas LOGINCANVAS;
    [SerializeField] Canvas QUESTIONCANVAS;
    [SerializeField] Canvas STORECANVAS;
    [SerializeField] Canvas GAMECLEARCANVAS;
    [SerializeField] Canvas GAMEOVERCANVAS;
    [SerializeField] Canvas WINCANVAS;
    [SerializeField] Canvas EXITCANVAS;
    #endregion


    // PlayerInfo UI
    #region PlayerInfo UI
    [Header("[PLAYER INFO]")]
    [SerializeField] public Text Player_Lv;
    [SerializeField] public Text Player_ID;
    [SerializeField] public Text Player_Gold;
    [SerializeField] public Text Player_Potion;
    [SerializeField] public Text Player_Attack;

    private string playerName = null;

    [SerializeField] public Slider Player_Staminar;
    [SerializeField] public Slider Player_Exp;
    [SerializeField] public Image[] Player_HP;
    #endregion

    [Header("[MAP UI]")]
    [SerializeField] GameObject MapInfoPrefab;
    [SerializeField] Text MapInfo;

    [Header("[MONSTER UI]")]
    [SerializeField] GameObject MonsterInfoPrefab;
    [SerializeField] Text MonsterInfo;

    [Header("[INPUT UI]")]
    [SerializeField] GameObject objInputName; // �̸� �Է� UI
    [SerializeField] Text inputName; // �Է¹��� �̸�

    [Header("[GENERAL UI]")]
    [SerializeField] Button StartButton;
    [SerializeField] Button ExitButton;
    [SerializeField] Button QuitButton;
    [SerializeField] Button QuitCancleButton;
    [SerializeField] Button RunButton; // �ӽ� ��ư

    [Header("[STORE UI]")]
    [SerializeField] Button BackButton;
    [SerializeField] Button BuyButton;
    [SerializeField] Text StorePotionValue;
    [SerializeField] Text StoreGoldValue;


    [Header("[WIN UI]")]
    [SerializeField] Button ReturnButton;
    #endregion
    


    // �̱���
    #region �̱���

    private static UIManager Instance = null;
    public static UIManager INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<UIManager>();
                /* if (Instance == null)
                 {
                     Instance = new GameObject("UIManager").AddComponent<UIManager>();
                 }*/
            }
            DontDestroyOnLoad(Instance.gameObject);
            return Instance;
        }
    }
    #endregion

    Action action;

    private void Awake()
    {
        action += BUYPOTION;
        BuyButton.onClick.AddListener(delegate () { action(); });

        Player.INSTANCE.CURSTAMINAR = Player.INSTANCE.MAXSTAMINAR;
        Player_Staminar.value = Player.INSTANCE.CURSTAMINAR;
        Player_Staminar.gameObject.SetActive(false);

        TITLECANVAS.gameObject.SetActive(false);
        GENERALCANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        QUESTIONCANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);
        WINCANVAS.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);
        EXITCANVAS.gameObject.SetActive(false);
        LOGINCANVAS.gameObject.SetActive(false);        
    }

    // �÷��̾��� ���¹̳� üũ
    /*public void StaminarCheck()
    {
        Player.INSTANCE.CURSTAMINAR -= Time.deltaTime;
        Player_Staminar.value = Player.INSTANCE.CURSTAMINAR;
        Player_Staminar.gameObject.SetActive(true);

        if (Player.INSTANCE.CURSTAMINAR <= 0)
        {
            Player_Staminar.gameObject.SetActive(false);
            Player.INSTANCE.CURSTAMINAR = Player.INSTANCE.MAXSTAMINAR;
            Player.INSTANCE.PLAYERATTACKPOWER = 0;
        }

    }*/

    public void Start()
    {
        TITLESCENE();
    }

    public void Update()
    {

    }

    //
    // UI SCENE 
    #region UI SCENE 


    // Ÿ��Ʋ ��
    public void TITLESCENE()
    {
        TITLECANVAS.gameObject.SetActive(true);//         
        GENERALCANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);
        WINCANVAS.gameObject.SetActive(false);
    }


    // �ΰ���, ���� ��
    public void GENERALSCENE()
    {
        GENERALCANVAS.gameObject.SetActive(true);

        if (inputName.text.Length >= 2 && inputName.text.Length <= 8) //&& UnityEngine.Input.GetKeyDown(KeyCode.Return)
        {
            PlayerInputName();
            //Debug.Log(Player_ID.text);
        }
        else if (inputName.text.Length < 2 && inputName.text.Length > 8)
        {
            Debug.Log("ID�� �ּ� 2�ڸ����� 8�ڸ����� �����մϴ�.");
        }
        UpdatePlayerInfo();

        MonsterInfo.gameObject.SetActive(false);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);
        WINCANVAS.gameObject.SetActive(false);
        LOGINCANVAS.gameObject.SetActive(false);
        Player_Staminar.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);

    }
    void PlayerInputName()
    {
        // playerName = inputName.text;
        //PlayerPrefs.SetString("CurrentPlayerName", inputName.text);
        Player_ID.text = inputName.text;
    }
    void UpdatePlayerInfo()
    {
        StoreGoldValue.text = Player.INSTANCE.GetGold().ToString();
        StorePotionValue.text = Player.INSTANCE.GetPotion().ToString();
        Player_Gold.text = Player.INSTANCE.GetGold().ToString();
        Player_Potion.text = Player.INSTANCE.GetPotion().ToString();
    }



    public void ACTIONSCENE()
    {
        GENERALCANVAS.gameObject.SetActive(true);
        MonsterInfo.gameObject.SetActive(true);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);
        WINCANVAS.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(true);
    }

    // ��ư�� �������� ����Ǵ� �Լ�
    public void ONQUESTION()
    {
        QUESTIONCANVAS.gameObject.SetActive(true);
        GameManager.INSTANCE.IsWindowOpen = true;
    }
    public void OFFQUESTION()
    {
        QUESTIONCANVAS.gameObject.SetActive(false);
        GameManager.INSTANCE.IsWindowOpen = false;
    }

    public void STORESCENE()
    {
        GENERALCANVAS.gameObject.SetActive(false);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(true);//
        UpdatePlayerInfo();
        QUESTIONCANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);
        WINCANVAS.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);
    }

    public void GAMECLEARSCENE()
    {
        GENERALCANVAS.gameObject.SetActive(false);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(true);//        
        GAMEOVERCANVAS.gameObject.SetActive(false);
        WINCANVAS.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);
    }
    public void GAMEOVERSCENE()
    {
        GENERALCANVAS.gameObject.SetActive(false);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(true);//        
        WINCANVAS.gameObject.SetActive(false);
        RunButton.gameObject.SetActive(false);
    }

    // ���� �׾����� ���� üũ�غ���
    // ���Ͱ� �׾��� ���� Player WIN â ����ְ�
    // �÷��̾ �׾��� ���� Game Over â ����ֱ�
    bool IsWin = false;
    public void RESULTSCENE()
    {
        if (IsWin == true) // �÷��̾� �¸�
        {
            GENERALCANVAS.gameObject.SetActive(false);
            TITLECANVAS.gameObject.SetActive(false);
            STORECANVAS.gameObject.SetActive(false);
            GAMECLEARCANVAS.gameObject.SetActive(false);
            GAMEOVERCANVAS.gameObject.SetActive(false);
            WINCANVAS.gameObject.SetActive(true); //
        }
        else
        {
            GENERALCANVAS.gameObject.SetActive(false);
            TITLECANVAS.gameObject.SetActive(false);
            STORECANVAS.gameObject.SetActive(false);
            GAMECLEARCANVAS.gameObject.SetActive(false);
            GAMEOVERCANVAS.gameObject.SetActive(true); //
            WINCANVAS.gameObject.SetActive(false);
        }
    }
    public bool Check4WhoIsWin(bool who)
    {
        // �÷��̾ ���׾��ٸ�? �÷��̾� �¸�
        if (Player.INSTANCE.IsDead == false) IsWin = true;
        // �÷��̾ �׾��ٸ�? �÷��̾� �й�
        else if (Player.INSTANCE.IsDead == true) IsWin = false;

        return who;
    }

    //
    // LOGIN SCENE
    #region LOGIN SCENE & INPUT PLAYER NAME
    public void ONLOGINSCENE()
    {
        LOGINCANVAS.gameObject.SetActive(true);
    }

    public void OFFLOGINSCENE()
    {
        LOGINCANVAS.gameObject.SetActive(false);

    }
    #endregion


    //
    // EXIT SCENE
    #region EXIT SCENE
    public void ONEXITSCENE()
    {
        EXITCANVAS.gameObject.SetActive(true);
        GameManager.INSTANCE.IsWindowOpen = true;
    }
    public void OFFEXITSCENE()
    {
        EXITCANVAS.gameObject.SetActive(false);
    }

    #endregion
    // 

    #endregion
    //

    private void ValueChanged(Slider slider)
    {
        int value = (int)slider.value;

    }

    //
    // GET INFO
    #region GET INFO
    public void GETPLAYERINFO(string Lv, string Attack) // �÷��̾�� �浹�ؼ� ���� ���� ���
    {
        //PlayerInfo = GENERALCANVAS.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //PlayerInfo.text = GameManager.INSTANCE.GetPlayer().GETMYINFO().ToString();
        Player_Lv.text = Lv.ToString();
        //Player_Gold.text = Gold.ToString();
        //Player_Potion.text = Potion.ToString();
        Player_Attack.text = Attack.ToString();
        //Player_ID.text = ID.ToString();

    }
    public void GETMAPINFO(MAPINFO mapinfo) // �÷��̾�� �浹�ؼ� ���� ���� ���
    {
        //MapInfo = GENERALCANVAS.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        MapInfo.text = mapinfo.ToString();
    }
    public void GETMOBINFO(MonsterMarble mobinfo) // �÷��̾�� �浹�ؼ� ���� ���� ���
    {
        // ���� HP �����̴� �߰�
        // ��ġ�� ���� ����
        MonsterInfo.text = mobinfo.GetComponent<MonsterMarble>().MOBINFO.ToString();
    }
    #endregion

    //
    // Button UI
    #region Button UI
    public void BUYPOTION()
    {
        if (Player.INSTANCE.Shop4BuyAvailable == false) // ��尡 ���� �� ���౸�� ����
        {
            Player.INSTANCE.SetPotion(0);
            Player.INSTANCE.SetGold(0);
        }
        else //if (Player.INSTANCE.Shop4BuyAvailable == true)
        {
            Debug.Log("2");
            Player.INSTANCE.SetPotion(1);
            Player.INSTANCE.SetGold(10);
        }
        UpdatePlayerInfo();
    }


    #endregion
    /*
        //------------------------------------------------------------------

        // �̸� �Է� ����
        #region �̸� �Է� ����

        public bool InputNameResult { get; private set; } = false;

        public void ShowInputName(bool show = true)
        {
            objInputName.SetActive(show);
        }

        public void OnClick_Confirm()
        {
            // �̸��� �ݵ�� �Է�����
            if (string.IsNullOrEmpty(inputName.text)) return;

            InputNameResult = true;
            PlayerPrefs.SetString("myname", inputName.text);
            SetPlayerName(inputName.text);
        }

        public void SetPlayerName(string name)
        {
            Player_ID.text = name; // ȭ�� UI�� ǥ������
        }
        #endregion // �̸� �Է� ����

        //------------------------------------------------------------------
    */


}
