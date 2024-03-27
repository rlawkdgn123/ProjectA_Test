using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBase : MonoBehaviour
{
    public enum DefaultType { defender = 1, warrior, gunner }
    #region 세부 수치값(Stats)
    [Header("Stats")]
    public int maxHp;
    public int currentHp;

    [SerializeField]public float damage;
    [SerializeField]public float attackRange;
    [SerializeField]public float attackCoolTime;

    [SerializeField]public float moveSpeed;
    #endregion

    #region 감지 관련값(Detect)
    [Header("Detect")]
    [SerializeField] float detect_Range;        //감지 범위(반지름)
    [SerializeField] Vector3 size;              //피직스 오버랩스피어의 사이즈
    [SerializeField] Vector3 offset;            //피직스 오버랩스피어의 위치
    [SerializeField] Collider[] cols;           //피직스 오버랩스피어로 감지한 Collider 저장소
    [SerializeField] LayerMask target_layer;    //특정 레이어를 지닌 오브젝트를 감지하기 위한 레이어 마스크
    #endregion

    #region 상태 표시값(State)
    [Header("State")]
    bool isChase;
    bool isAttack;
    bool isDead;
    #endregion

    #region 기타 변수(Other)
    [Header("Other")]
    GameObject player;
    LayerMask enemyDetectedMask;
    protected NavMeshAgent agent;
    protected Animation anim;
    protected float distanse;
    #endregion

    private void Awake() {
        
    }

    void Update() {
        Chase();
    }
    void DetectEnemy() {
        cols = Physics.OverlapSphere(transform.position, detect_Range);
    }
    void Chase() {
        agent.SetDestination(player.transform.position);
    }
}
