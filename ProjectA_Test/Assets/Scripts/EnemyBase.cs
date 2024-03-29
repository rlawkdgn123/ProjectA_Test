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

    public float attackDamage;      // 공격력
    public float attackCoolTime;    // 공격 쿨타임
    public float attackRange;       // 공격 거리

    public float detectRadius;      // 감지 범위 (반지름)
}
public class EnemyBase : MonoBehaviour
{
    #region 수치값
    public Stats stats;             // Stats 구조체변수
    [SerializeField]protected float targetDistance; // 타겟(플레이어)와의 거리
    protected float moveSpeedBackUp;// 이동속도 백업 변수
    public float attackTime = 0f;// 공격 딜레이
    #endregion

    #region 상태값
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

    #region 기타
    [SerializeField]protected Collider[] colls; // 감지할 콜라이더들
    [SerializeField]protected Transform target; // 타겟(플레이어) 트랜스폼
    public Transform enemy;       // 가독성을 위한 변수 선언
    protected NavMeshAgent nav;     // 네비게이션
    protected Rigidbody rb;         // 리지드바디
    [SerializeField]protected Animator anim;        // 애니메이터
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

        colls = Physics.OverlapSphere(enemy.position, stats.detectRadius, targetLayer);   // Player 레이어 콜라이더 감지
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
            print("몬스터 죽음");
            StopAllCoroutines();
            nav.enabled = false;
        }
    }
    protected void FreezeVelocity() { // 추적 중 물리 효과 무시
        if(isChase)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    protected void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.position, stats.detectRadius); // 감지 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.position, stats.attackRange); // 공격 범위
    }
}
