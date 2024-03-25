using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTanker : MonsterBase
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
            print("��Ӱ���!");
        }
        isAttack = false;
    }
}