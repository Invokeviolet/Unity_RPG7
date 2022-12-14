using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



// 전체 데이터 관리
// 출력을 UI매니저가 하게끔

// 몬스터 정보
public enum MOBINFO
{
    EASY,
    NORMAL,
    HARD,
    BOSS
}

// 맵 정보
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
    // 싱글톤
    #region 싱글톤

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

    // 플레이어가 인게임이니?
    public bool myPlayerInGame { get; set; }
    // 다른 윈도우 창이 켜져있나?
    public bool IsWindowOpen { get; set; }
    // 플레이어가 전투중이니?
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

