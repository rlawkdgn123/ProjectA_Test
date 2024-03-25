using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]  // 인스펙터에 보이는 구조체로 정의
public struct Stats
{
    public float maxHP; // 최대 체력
    public float curHP; // 현재 체력
    //public float MoveSpeed;

    public float attackDamage;      // 공격력
    public float attackCoolTime;    // 공격 딜레이
    public float attackRange;       // 공격 거리

    public float detectRadius;
}
public class MonsterBase : MonoBehaviour
{
    #region 수치값
    public Stats stats;
    protected float targetDistance;
    #endregion

    #region 상태값
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

    #region 기타
    [SerializeField] Collider[] colls;
    [SerializeField]protected Transform target;
    public Transform monster; // 가독성을 위한 변수 선언
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
            print("공격");
        }
        isAttack = false;
    }
    protected void Dead() {
        if(stats.curHP <= 0 && !isDie)
        {
            print("몬스터 죽음");
            StopAllCoroutines();
            nav.speed = 0;
        }
    }
    protected void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(monster.position, stats.detectRadius); // 감지 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(monster.position, stats.attackRange); // 공격 범위
    }
}
