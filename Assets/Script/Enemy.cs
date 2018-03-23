using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Enemy : MonoBehaviour {

    private CharacterController enemyController;
    private Animator animator;

    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;
    public GameObject cube5;


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


    //攻撃した後のフリーズ時間
    [SerializeField]
    private float freezeTime = 0.3f;


    //敵の状態
    public EnemyState state;
    //追いかけるキャラクター
    private Transform playerTransform;


    public enum EnemyState
    {
        Walk,
        Wait,
        Chase,
        Attack,
        Freeze
    };


    //Use this for initialization
    // Use this for initialization
    void Start()
    {
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
    void Update()
    {

        if (state == EnemyState.Walk || state == EnemyState.Chase)
        {
            //キャラクターを追いかける状態であれば、キャラクターの目的地を再設定
            if (state == EnemyState.Chase)
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


            //目的地に到着したかどうかの判定
            if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 0.7f)
            {
                SetState("wait");
                animator.SetFloat("Speed", 0.0f);
            }      
            else if (state == EnemyState.Chase)
            {
                //攻撃する距離だったら攻撃
                if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 2.0f)
                {
                    SetState("attack");
                }
            }
        }
        else if (state == EnemyState.Wait)  // 到着していたら一定時間待つ
        {
        
            elapsedTime += Time.deltaTime;

            //　待ち時間を超えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState("walk");
            }

        }
        else if(state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime > freezeTime)
            {
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
            //Instantiate(cube1, transform.position, Quaternion.identity);
            arrived = false;
            elapsedTime = 0f;
            state = EnemyState.Walk;
            setPosition.CreateRandomPosition();

        }
        else if (mode == "chase")
        {
            //Instantiate(cube2, transform.position, Quaternion.identity);
            state = EnemyState.Chase;
            arrived = false;
            //追いかける対象をセット
            playerTransform = obj;
        }
        else if (mode == "wait")
        {
            //Instantiate(cube3, transform.position, Quaternion.identity);
            elapsedTime = 0f;
            state = EnemyState.Wait;
            arrived = true;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
        else if(mode == "attack")
        {
            //Instantiate(cube4, transform.position, Quaternion.identity);
            state = EnemyState.Attack;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack", true);
        }
        else if(mode == "freeze")
        {
            Instantiate(cube5, transform.position, Quaternion.identity);
            elapsedTime = 0f;
            velocity = Vector3.zero;
            state = EnemyState.Freeze;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack", false);
        }


    }

    public EnemyState GetState()
    {
        return state;
    }

}
