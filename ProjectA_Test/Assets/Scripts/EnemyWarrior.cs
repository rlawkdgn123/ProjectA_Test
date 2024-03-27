using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
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
    public override float AttackTimeCul(float attackTime) {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Attack":
                    attackTime = clip.length;
                    break;
            }
        }
        return attackTime;
    }
    public override IEnumerator Attack() {
        if (isAttack)
        {
            Player player = target.GetComponent<Player>();
            float attackTime = 0f;
            nav.speed = 0;
            AttackTimeCul(attackTime);
            weapon.SetActive(true);
            yield return new WaitForSeconds(attackTime);
            anim.SetTrigger("Attack");
            weapon.SetActive(false);
            nav.speed = moveSpeedBackUp;
        }
        isAttack = false;
    }
    public void Anim() {
        if(isDetected)
        {
            if (isWalk && !isRun)   // 걷기 애니메이션
            {
                anim.SetBool("isWalk", true);
                anim.SetBool("isRun", false);
            }
            else                    // 뛰기 애니메이션
            {
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", true);
            }
        }

        if (!isDetected)            // 기본 모션(Idle) 애니메이션
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
        }
    }
}