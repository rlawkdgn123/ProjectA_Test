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
            print("��ü��" + player.HP);
            print("������" + enemy.stats.attackDamage);
            player.HP -= enemy.stats.attackDamage;
        }
    }
}
