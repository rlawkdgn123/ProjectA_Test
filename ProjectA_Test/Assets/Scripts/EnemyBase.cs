using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]  // �ν����Ϳ� ���̴� ����ü�� ����
public struct Stats
{
    public float maxHP; // �ִ� ü��
    public float curHP; // ���� ü��

    public float attackDamage;      // ���ݷ�
    public float attackCoolTime;    // ���� ������
    public float attackRange;       // ���� �Ÿ�

    public float detectRadius;      // ���� ���� (������)
}
public class EnemyBase : MonoBehaviour
{
    #region ��ġ��
    public Stats stats;             // Stats ����ü����
    protected float targetDistance; // Ÿ��(�÷��̾�)���� �Ÿ�
    protected float moveSpeedBackUp;// �̵��ӵ� ��� ����
    #endregion

    #region ���°�
    protected bool isIdle
    {
        get 
        {
            return isAttack == false && isChase == false;
        }
    }
    public bool isAttack;
    protected bool isWalk;
    protected bool isRun;
    protected bool isChase;
    protected bool isDeath;
    protected bool isHit;

    protected bool isDetected;
    protected bool isEnteredAttackRange;
    protected bool isBattleCry;
    #endregion

    #region ��Ÿ
    [SerializeField]protected Collider[] colls; // ������ �ݶ��̴���
    [SerializeField]protected Transform target; // Ÿ��(�÷��̾�) Ʈ������
    public Transform enemy;       // �������� ���� ���� ����
    protected NavMeshAgent nav;     // �׺���̼�
    protected Rigidbody rb;         // ������ٵ�
    protected Animator anim;        // �ִϸ�����
    [SerializeField] private LayerMask targetLayer;
    #endregion

    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        targetLayer = LayerMask.GetMask("Player");
        moveSpeedBackUp = nav.speed;
    }
    void Update() {
        DetectTarget();
        Death();
    }
    public void DetectTarget() {
        colls = Physics.OverlapSphere(enemy.position, stats.detectRadius, targetLayer);   // Player ���̾� �ݶ��̴� ����
        foreach (Collider coll in colls)
        {
            if (coll.CompareTag("Player"))
            {
                isDetected = true;
                target = coll.transform;
                if (!isBattleCry)
                {
                    isBattleCry = true;
                }
                isChase = true;
                Chase();
            }
            else
            {
                /*
                if (coll.CompareTag("Monster"))
                {
                    MonsterBase mon = coll.GetComponent<MonsterBase>();
                }
                */
                isBattleCry = false;
            }
        }
    }
    void Chase() {
        if (isChase)
        {
            FreezeVelocity();
            targetDistance = Vector3.Distance(enemy.position, target.position);
            enemy.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));
            nav.SetDestination(target.position);
            if (targetDistance < stats.attackRange)
            {
                if (!isAttack)
                {
                    isAttack = true;
                    StartCoroutine("Attack");
                }
            }
        }
    }
    public virtual float AttackTimeCul(float attackTime) {
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
    public virtual IEnumerator Attack() {
        if (isAttack)
        {
            Player player = target.GetComponent<Player>();
            float attackTime = 0f;
            nav.speed = 0;
            AttackTimeCul(attackTime);
            yield return new WaitForSeconds(attackTime);
            anim.SetTrigger("Attack");
            nav.speed = moveSpeedBackUp;
        }
        isAttack = false;
    }
    protected void Death() {
        if (stats.curHP <= 0 && !isDeath)
        {
            rb.freezeRotation = false;
            anim.SetTrigger("isDeath");
            print("���� ����");
            StopAllCoroutines();
            nav.speed = 0;
        }
    }
    void FreezeVelocity() { // ���� �� ���� ȿ�� ����
        if(isChase)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    protected void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.position, stats.detectRadius); // ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.position, stats.attackRange); // ���� ����
    }
}
