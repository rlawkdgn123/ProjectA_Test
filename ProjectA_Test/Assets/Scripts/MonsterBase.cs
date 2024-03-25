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
    //public float MoveSpeed;

    public float attackDamage;      // ���ݷ�
    public float attackCoolTime;    // ���� ������
    public float attackRange;       // ���� �Ÿ�

    public float detectRadius;
}
public class MonsterBase : MonoBehaviour
{
    #region ��ġ��
    public Stats stats;
    protected float targetDistance;
    #endregion

    #region ���°�
    protected bool isIdle
    {
        get 
        {
            return isAttack == false && isChase == false;
        }
    }
    protected bool isAttack;
    protected bool isChase;
    protected bool isDie;

    protected bool isDetected;
    protected bool isEnteredAttackRange;
    protected bool isBattleCry;
    #endregion

    #region ��Ÿ
    [SerializeField] Collider[] colls;
    [SerializeField]protected Transform target;
    public Transform monster; // �������� ���� ���� ����
    NavMeshAgent nav;
    Rigidbody rb;
    Animation anim;
    LayerMask targetLayer;
    #endregion

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animation>();
        targetLayer = LayerMask.NameToLayer("Player");
    }
    void Update() {
        DetectTarget();
        Dead();
    }
    public void DetectTarget() {
        colls = Physics.OverlapSphere(monster.position, stats.detectRadius, targetLayer);
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
            targetDistance = Vector3.Distance(monster.position, target.position);
            monster.LookAt(target.position);
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
    public virtual IEnumerator Attack() {
        if (isAttack)
        {
            Player player = target.GetComponent<Player>();
            yield return new WaitForSeconds(5f);
            print("����");
        }
        isAttack = false;
    }
    protected void Dead() {
        if(stats.curHP <= 0 && !isDie)
        {
            print("���� ����");
            StopAllCoroutines();
            nav.speed = 0;
        }
    }
    protected void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(monster.position, stats.detectRadius); // ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(monster.position, stats.attackRange); // ���� ����
    }
}
