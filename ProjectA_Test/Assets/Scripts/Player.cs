using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    public float HP;
    void Start()
    {
        
    }
    void Update() {
        Vector3 moveVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += moveVec * moveSpeed * Time.deltaTime;
        if (HP == 0)
        {
            print("À¸¾ÇÁ×À½");
        }
    }
}
