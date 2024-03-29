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
    public float attackCoolTime;    // ���� ��Ÿ��
    public float attackRange;       // ���� �Ÿ�

    public float detectRadius;      // ���� ���� (������)
}
public class EnemyBase : MonoBehaviour
{
    #region ��ġ��
    public Stats stats;             // Stats ����ü����
    [SerializeField]protected float targetDistance; // Ÿ��(�÷��̾�)���� �Ÿ�
    protected float moveSpeedBackUp;// �̵��ӵ� ��� ����
    public float attackTime = 0f;// ���� ������
    #endregion

    #region ���°�
    protected bool isIdle;
    [SerializeField]public bool isAttack;
    [SerializeField]protected bool isWalk;
    protected bool isRun;
    [SerializeField]protected bool isChase;
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
    [SerializeField]protected Animator anim;        // �ִϸ�����
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
                isChase = false;
                isBattleCry = false;
            }
        }
    }
    public virtual void Chase() {

        if (isChase)
        {
            targetDistance = Vector3.Distance(enemy.position, target.position);
            FreezeVelocity();
            enemy.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));

            if (!isAttack)
            {
                nav.SetDestination(target.position);
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
    public virtual IEnumerator Attack() {
        isAttack = true;
        Player player = target.GetComponent<Player>();
        nav.isStopped = true;
        yield return new WaitForSeconds(attackTime);
        nav.isStopped = false;
        isAttack = false;
    }
    protected void Death() {
        if (stats.curHP <= 0 && !isDeath)
        {
            rb.freezeRotation = false;
            anim.SetTrigger("DoDeath");
            print("���� ����");
            StopAllCoroutines();
            nav.enabled = false;
        }
    }
    protected void FreezeVelocity() { // ���� �� ���� ȿ�� ����
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
