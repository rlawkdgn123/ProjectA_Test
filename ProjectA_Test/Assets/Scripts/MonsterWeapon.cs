using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    public GameObject Monster;
    public
    void Awake() {
        Monster = this.GetComponentInParent<GameObject>();
    }
    private void OnCollisionEnter(Collision collision) {
        
    }
}
