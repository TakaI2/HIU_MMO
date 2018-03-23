using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessAttack_E : MonoBehaviour {

    private Enemy enemy;
    //private CapsuleCollider capsuleCollider;
    public CapsuleCollider capsuleCollider;
    private Animator animator;

	// Use this for initialization
	void Start () {
        enemy = GetComponent<Enemy>(); 
        //capsuleCollider =  GetComponentInChildren<CapsuleCollider>();
        animator = GetComponent<Animator>();
	}
	
    void AttackStart()
    {
        capsuleCollider.enabled = true;
    }

    void AttackEnd()
    {
        capsuleCollider.enabled = false;
    }

    void StateEnd()
    {
        enemy.SetState("freeze");
    }

	// Update is called once per frame
	void Update () {
		
	}
}
