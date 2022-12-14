using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



// ��ü ������ ����
// ����� UI�Ŵ����� �ϰԲ�

// ���� ����
public enum MOBINFO
{
    EASY,
    NORMAL,
    HARD,
    BOSS
}

// �� ����
public enum MAPINFO
{
    FOREST,
    SWAMP,
    GROUND,
    SHOP,
    START,
    END
}


public class GameManager : MonoBehaviour
{
    // �̱���
    #region �̱���

    private static GameManager Instance = null;
    public static GameManager INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<GameManager>();
            }
            return Instance;
        }
    }
    #endregion

    // �÷��̾ �ΰ����̴�?
    public bool myPlayerInGame { get; set; }
    // �ٸ� ������ â�� �����ֳ�?
    public bool IsWindowOpen { get; set; }
    // �÷��̾ �������̴�?
    public bool myPlayerAction { get; set; }

    Player player;
    Monster monster;
    public Player GetPlayer() => player;
    public Monster GetMonster() => monster;
    //public GameObject PlayerCam=null;

    public int POTION;
    public int GOLD;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        monster = FindObjectOfType<Monster>();
        
    }
    
    private void Update()
    {
       
    }

    /*public void BUYPOTION(string potion, string gold)
    {        
        UIManager.INSTANCE.HaveAGold.text = "GOLD " + potion;
        UIManager.INSTANCE.HaveAPotion.text = "POTION " + gold;                
    }*/

}

