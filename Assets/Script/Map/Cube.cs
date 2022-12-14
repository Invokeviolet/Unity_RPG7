using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cube : MonoBehaviour
{
    MAPINFO myInfo;
    // 좌표 받아오기
    public Vector3 MAPPOSITION { get; set; }
    // 색상 받아오기
    public Color MAPCOLOR { get; set; }
    // 재질
    public MeshRenderer MAPMESH { get; set; }
    [SerializeField] Material Forest;
    [SerializeField] Material Ground;
    [SerializeField] Material Swamp;
    [SerializeField] Material Start;
    [SerializeField] Material Final;
    [SerializeField] Material Shop;

    // 이름 받아오기
    public string MAPNAME { get; set; }
    // 값 받아오기
    public int MAPNUM { get; set; }
    // 몬스터 받아오기
    public string MONSTER { get; set; }
    // 맵 정보 
    public MAPINFO MAPINFO { get { return myInfo; } set { myInfo = value; } }


    // 생성
    public void Init(Vector3 pos, MAPINFO info)
    {

        // 큐브마다 랜덤한 몬스터 출력
        this.MAPPOSITION = pos;
        this.myInfo = info;

        // 내 컴포넌트의 Meterial을 가져와서 색상 변경
        Material myM = GetComponent<MeshRenderer>().material;
        MAPMESH = GetComponent<MeshRenderer>();

        if (info == MAPINFO.FOREST) //0
        {
            MAPMESH.material = Forest;
            //MAPCOLOR = Color.green;
            MAPNAME = "FOREST";
            MAPNUM = 0;
            this.gameObject.tag = "Forest";
        }
        if (info == MAPINFO.SWAMP) //1
        {
            MAPMESH.material = Swamp;
            //MAPCOLOR = Color.blue;
            MAPNAME = "SWAMP";
            MAPNUM = 1;
            this.gameObject.tag = "Swamp";
        }
        if (info == MAPINFO.GROUND) //2
        {
            MAPMESH.material = Ground;
            //MAPCOLOR = Color.yellow;
            MAPNAME = "GROUND";
            MAPNUM = 2;
            this.gameObject.tag = "Ground";
        }
        if (info == MAPINFO.SHOP) //3
        {
            MAPMESH.material = Shop;
            //MAPCOLOR = Color.black;
            MAPNAME = "STORE";
            MAPNUM = 3;
            this.gameObject.tag = "Store";
            // 상점일 때 물약 구매
        }
        if (info == MAPINFO.START) //4
        {
            MAPMESH.material = Start;
            //MAPCOLOR = Color.white;
            MAPNAME = "START";
            MAPNUM = 4;
            this.gameObject.tag = "Start";
        }
        if (info == MAPINFO.END) //5
        {
            MAPMESH.material = Final;
            //MAPCOLOR = Color.white;
            MAPNAME = "ARRIVAL";
            MAPNUM = 5;
            this.gameObject.tag = "End";
        }

        myM = MAPMESH.material;
    }



}
