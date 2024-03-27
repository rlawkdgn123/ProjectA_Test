using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWarrior : MonsterBase
{
    void Start() {

    }
    void Update() {
        base.DetectTarget();
    }
    public override IEnumerator Attack() {
        if (isAttack)
        {
            Player player = base.target.GetComponent<Player>();
            yield return new WaitForSeconds(2f);
            player.HP--;
            print("전사 공격");
        }
        isAttack = false;
    }
}