using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    EnemyBase enemy;
    Player player;
    void Awake() {
        enemy = this.GetComponentInParent<EnemyBase>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && enemy.isAttack)
        {
            player = GetComponent<Player>();
            player.HP -= enemy.stats.attackDamage;
        }
    }
}
