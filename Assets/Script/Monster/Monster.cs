using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public bool IsDead { get; set; }

    int AttackPower;

    MOBINFO mobInfo;
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

    // 공격받으면 들어온 데미지만큼 HP 감소
    public void HitMonster(int damage)
    {
        
    }
    public void IMDEAD()
    {
        IsDead = true;
        Destroy(this.gameObject);
        UIManager.INSTANCE.Check4WhoIsWin(false);
        // 2초 뒤 몬스터 잡았다는 창 띄우기
        Invoke("UIManager.INSTANCE.RESULTSCENE()", 2f);
        // 플레이어 경험치 증가
        Player.INSTANCE.ExpUpdate(10);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wepon")) 
        {
            HP -= Player.INSTANCE.PLAYERATTACKPOWER;
            if (HP <= 0)
            {
                IMDEAD();
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 플레이어에게 데미지 주기
            int rndDamage = Random.Range(0, 10);
            GameManager.INSTANCE.GetPlayer().SetHP(rndDamage);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        { 
            // 애니메이션 또는 이펙트 재생
        }
    }

}
      
