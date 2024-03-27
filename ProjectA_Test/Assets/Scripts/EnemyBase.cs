using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBase : MonoBehaviour
{
    public enum DefaultType { defender = 1, warrior, gunner }
    #region ���� ��ġ��(Stats)
    [Header("Stats")]
    public int maxHp;
    public int currentHp;

    [SerializeField]public float damage;
    [SerializeField]public float attackRange;
    [SerializeField]public float attackCoolTime;

    [SerializeField]public float moveSpeed;
    #endregion

    #region ���� ���ð�(Detect)
    [Header("Detect")]
    [SerializeField] float detect_Range;        //���� ����(������)
    [SerializeField] Vector3 size;              //������ ���������Ǿ��� ������
    [SerializeField] Vector3 offset;            //������ ���������Ǿ��� ��ġ
    [SerializeField] Collider[] cols;           //������ ���������Ǿ�� ������ Collider �����
    [SerializeField] LayerMask target_layer;    //Ư�� ���̾ ���� ������Ʈ�� �����ϱ� ���� ���̾� ����ũ
    #endregion

    #region ���� ǥ�ð�(State)
    [Header("State")]
    bool isChase;
    bool isAttack;
    bool isDead;
    #endregion

    #region ��Ÿ ����(Other)
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
