using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    MonsterBase monster;
    Player player;
    void Awake() {
        monster = this.GetComponentInParent<MonsterBase>();
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player") && monster.isAttack)
        {
            player = GetComponent<Player>();
            player.HP -= monster.stats.attackDamage;
            print("Å¸°Ý!"+monster.stats.attackDamage);
        }
    }
}
