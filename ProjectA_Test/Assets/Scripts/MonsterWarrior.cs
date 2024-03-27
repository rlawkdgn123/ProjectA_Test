using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

public class MonsterWarrior : MonsterBase
{
    public GameObject weapon;
    void Awake() {
        weapon = GetComponent<GameObject>();
    }
    void Update() {
        base.DetectTarget();
    }
    
    public override IEnumerator Attack() {
        if (isAttack)
        {
            Player player = base.target.GetComponent<Player>();
            weapon.SetActive(true);
            
            yield return new WaitForSeconds(2f);
            weapon.SetActive(false);

            print("전사 공격");
        }
        isAttack = false;
    }
}