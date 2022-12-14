using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneManager : MonoBehaviour
{
    // �̱���
    #region �̱���

    private static ChangeSceneManager Instance = null;
    public static ChangeSceneManager INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<ChangeSceneManager>();
               
            }
            return Instance;
        }
    }
    #endregion



    public bool Title { get; set; } = false;
    public bool Ingame { get; set; } = false;
    public bool Action { get; set; } = false;
    public bool Store { get; set; } = false;
    public bool Npc { get; set; } = false;

    public void Start()
    {
        //GameObject.Find("START").GetComponentInChildren<Text>().text = "START_text";        
        TITLESCENE();
    }

    public void TITLESCENE() // Ÿ��Ʋ
    {
        SceneManager.LoadScene("00_TITLE_Scene");
        UIManager.INSTANCE.TITLESCENE();
        GameManager.INSTANCE.myPlayerInGame = false;
        Title = true;
        Ingame = false;
        Action = false;
        Store = false;
        Npc = false;
    }

    public void INGAMESCENE() // �ΰ���
    {
        SceneManager.LoadScene("01_INGAME_Scene");
        UIManager.INSTANCE.GENERALSCENE();
        UIManager.INSTANCE.OFFQUESTION();
        GameManager.INSTANCE.myPlayerInGame = true;
        GameManager.INSTANCE.myPlayerAction = false;
        Debug.Log("�ΰ���?");
        Player.INSTANCE.transform.position = Player.INSTANCE.PlayerPos.transform.position;

        Title = false;
        Ingame = true;
        Action = false;
        Store = false; 
        Npc = false;
    }

    public void STORESCENE() // ����
    {
        SceneManager.LoadScene("02_STORE_Scene");
        UIManager.INSTANCE.STORESCENE();
        UIManager.INSTANCE.OFFQUESTION();
        GameManager.INSTANCE.myPlayerInGame = false;
        GameManager.INSTANCE.myPlayerAction = false;
        Title = false;
        Ingame = false;
        Action = false;
        Store = true;
        Npc = false;
    }


    public void ACTIONFORESTSCENE() // ����
    {
        SceneManager.LoadScene("03_ACTION_Scene");        
        UIManager.INSTANCE.ACTIONSCENE();
        UIManager.INSTANCE.OFFQUESTION();
        GameManager.INSTANCE.myPlayerInGame = false;
        GameManager.INSTANCE.myPlayerAction = true;

        Player.INSTANCE.transform.position = Player.INSTANCE.ActionPlayerPos.transform.position;

        Title = false;
        Ingame = false;
        Action = true;
        Store = false;
        Npc = false;
    }

    public void LOADINGSECNE()
    {
        SceneManager.LoadScene("10_LOADING_Scene");
        GameManager.INSTANCE.myPlayerInGame = false;
    }

    static public void Quit() // ���� ����
    {
        Application.Quit();
    }

}
