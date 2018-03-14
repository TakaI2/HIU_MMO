﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MoveEnemy : MonoBehaviour {

    private CharacterController enemyController;
    private Animator animator;

    //スタート地点
    private Vector3 startPosition;
    //目的地
    private Vector3 destination;
    //歩くスピード
    [SerializeField]
    private float walkSpeed = 1.0f;
    //速度
    private Vector3 velocity;
    //移動方向
    private Vector3 direction;
    //到着フラグ
    private bool arrived;
    //SetPositionスクリプト
    private SetPosition setPosition;
    //待ち時間
    [SerializeField]
    private float waitTime = 5f;
    //経過時間
    private float elapsedTime;



    //敵の状態
    public EnemyState state;
    //追いかけるキャラクター
    private Transform playerTransform;
 
    
    public enum EnemyState
    {
        Walk,
        Wait,
        Chase
    };
    

    //Use this for initialization
    // Use this for initialization
    void Start () {
        enemyController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        setPosition = GetComponent<SetPosition>();
        setPosition.CreateRandomPosition();
        velocity = Vector3.zero;
        arrived = false;
        elapsedTime = 0f;
        SetState("wait");

	}
	
	// Update is called once per frame
	void Update () {

        if(state == EnemyState.Walk || state == EnemyState.Chase)
        {
            //キャラクターを追いかける状態であれば、キャラクターの目的地を再設定
            if(state == EnemyState.Chase)
            {
                setPosition.SetDestination(playerTransform.position);
            }

            if (enemyController.isGrounded)
            {
                velocity = Vector3.zero;
                animator.SetFloat("Speed", 2.0f);
                direction = (setPosition.GetDestination() - transform.position).normalized;
                transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));
                velocity = direction * walkSpeed;

            }
            //velocity.y += Physics.gravity.y * Time.deltaTime;
            //enemyController.Move(velocity * Time.deltaTime);

            //目的地に到着したかどうかの判定
            if(Vector3.Distance(transform.position, setPosition.GetDestination()) < 0.7f)
            {
                SetState("wait");
                animator.SetFloat("Speed", 0.0f);
            }

            // 到着していたら一定時間待つ
        }
        else if(state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;

            //　待ち時間を超えたら次の目的地を設定
            if(elapsedTime > waitTime)
            {
                //setPosition.CreateRandomPosition();
                //destination = setPosition.GetDestination();
                //arrived = false;
                //elapsedTime = 0f;
                SetState("walk");
            }
            
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        enemyController.Move(velocity * Time.deltaTime);


    }

    public void SetState(string mode, Transform obj = null)
    {
        if (mode == "walk")
        {
            arrived = false;
            elapsedTime = 0f;
            state = EnemyState.Walk;
            setPosition.CreateRandomPosition();

        }else if(mode == "chase"){
            state = EnemyState.Chase;
            arrived = false;
            //追いかける対象をセット
            playerTransform = obj;
        }else if(mode == "wait")
        {
            elapsedTime = 0f;
            state = EnemyState.Wait;
            arrived = true;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
        
    }

    public EnemyState GetState()
    {
        return state;
    }


}
