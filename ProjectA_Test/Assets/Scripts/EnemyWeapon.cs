using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public EnemyBase enemy;
    [SerializeField]Player player;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && other.TryGetComponent(out Player player) && enemy.isAttack)
        {
            print("플체력" + player.HP);
            print("에뎀지" + enemy.stats.attackDamage);
            player.HP -= enemy.stats.attackDamage;
        }
    }
}
