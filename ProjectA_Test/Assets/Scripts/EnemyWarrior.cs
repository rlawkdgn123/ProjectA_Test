using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWarrior : EnemyBase
{
    public GameObject weapon;
    override protected void Awake() {
        base.Awake();
    }
    void Update() {
        Anim();
        base.DetectTarget();
        base.Death();
    }
    public override void Chase() {

        if (isChase)
        {
            targetDistance = Vector3.Distance(enemy.position, target.position);
            base.FreezeVelocity();
            enemy.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));

            nav.SetDestination(target.position);

            if (targetDistance < 2f)
            {
                isWalk = false;
                nav.isStopped = true;
            }
            else
            {
                isWalk = true;
                nav.isStopped = false;
            }


            if (targetDistance < stats.attackRange)
            {
                if (!isAttack)
                {
                    StartCoroutine("Attack");
                }
            }
        }
    }
    public override IEnumerator Attack() {
        isAttack = true;
        //Player player = target.GetComponent<Player>();
        weapon.SetActive(true);
        anim.SetBool("IsAttack", true);
        yield return new WaitForSeconds(attackTime); // 애니메이션 시간 + HasExitTime을 고려하여 입력해주어야한다.
        anim.SetBool("IsAttack", false);
        weapon.SetActive(false);
        yield return new WaitForSeconds(stats.attackCoolTime); 
        isAttack = false;
    }
    public void Anim() {
        if (isWalk && isChase && !isDeath && !nav.isStopped)
        {
            anim.SetBool("IsWalk", true);
        }else
        {
            anim.SetBool("IsWalk", false);
        }
            
    }
}