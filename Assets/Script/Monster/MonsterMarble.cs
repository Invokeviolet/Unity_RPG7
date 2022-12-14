using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMarble : MonoBehaviour
{

    MOBINFO mobInfo;

    int AttackPower;

    // 좌표 받아오기
    public Vector3 pos { get; set; }
    // 색상 받아오기
    public Color prefabCol { get; set; }
    // 몬스터 받아오기
    public string monster { get; set; }
    // 체력 받아오기
    public int HP { get; set; }
    // 경험치 받아오기
    public int EXP { get; set; }

    // 몹 정보 
    public MOBINFO MOBINFO { get { return mobInfo; } set { mobInfo = value; } }


    void Update()
    {

    }
    

    // 생성
    public void Init(Vector3 pos, MOBINFO info)
    {
        // 큐브마다 랜덤한 몬스터 출력
        this.pos = pos;
        this.mobInfo = info;

        // 내 컴포넌트의 Meterial을 가져와서 색상 변경
        Material myM = GetComponent<MeshRenderer>().material;

        if (info == MOBINFO.EASY) //0
        {
            // this.gameObject.tag = "Easy";
            prefabCol = Color.cyan;
            HP = 50;
            EXP = 50;
            AttackPower = 10;
        }
        if (info == MOBINFO.NORMAL) //1
        {
            // this.gameObject.tag = "Normal";
            prefabCol = Color.blue;
            HP = 100;
            EXP = 100;
            AttackPower = 30;
        }
        if (info == MOBINFO.HARD) //2
        {
            // this.gameObject.tag = "Hard";
            prefabCol = Color.magenta;
            HP = 180;
            EXP = 180;
            AttackPower = 50;
        }
        if (info == MOBINFO.BOSS) //3
        {
            // this.gameObject.tag = "Boss";
            prefabCol = Color.black;
            HP = 300;
            EXP = 300;
            AttackPower = 100;
        }
        myM.color = prefabCol;
    }

}
      
