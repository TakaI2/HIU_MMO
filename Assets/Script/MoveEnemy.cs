using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MoveEnemy : MonoBehaviour {

    private CharacterController enemyController;
    private Animator animator;

    public GameObject blockPrefab1;
    public GameObject cube2;
    //public GameObject cube3;


    public float life;
    public Transform head; //砲台の位置パラメータ
    public float headRotationSmooth;
    public float bulletpower;
    public float longattackInterval;
    private float lastAttackTime; //レーザーの発射間隔

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
        Chase,
        Death
    };
    

    //Use this for initialization
    // Use this for initialization
    void Start () {
        enemyController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        setPosition = GetComponent<SetPosition>();
        setPosition.CreateRandomPosition();
        velocity = Vector3.zero;
        arrived = true; //生きているか？
        elapsedTime = 0f;
        SetState("wait");

	}


    public void Damage(int damage)
    {

        this.life -= damage;

        //_PlayerHealthSlider.value = hp;
        //_Health.text = hp.ToString("F2");

        if (this.life <= 0)
        {
            arrived = false;
            SetState("death");
        }
    }


    // Update is called once per frame
    void Update () {

        if(state == EnemyState.Walk || state == EnemyState.Chase)
        {
            //キャラクターを追いかける状態であれば、キャラクターの目的地を再設定
            if(state == EnemyState.Chase && arrived)
            {
                setPosition.SetDestination(playerTransform.position);

                // 砲台をプレイヤーの方向に向ける
                Quaternion headRotation = Quaternion.LookRotation(playerTransform.position - head.position);

                head.rotation = Quaternion.Slerp(head.rotation, headRotation, Time.deltaTime * headRotationSmooth);


                // 一定間隔で弾丸を発射する


                if (Time.time > lastAttackTime + longattackInterval)
                {
                    //炸薬により実体弾を打ち出すタイプ。
                    GameObject laserInstance = GameObject.Instantiate(blockPrefab1, head.position, head.rotation);
                    laserInstance.GetComponent<Rigidbody>().AddForce(laserInstance.transform.forward * bulletpower);
                    Destroy(laserInstance, 5);

                    lastAttackTime = Time.time;

                }

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
            if(Vector3.Distance(transform.position, setPosition.GetDestination()) < 3f)
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
            //Instantiate(cube1, transform.position, Quaternion.identity);
            elapsedTime = 0f;
            state = EnemyState.Walk;
            setPosition.CreateRandomPosition();

        }else if(mode == "chase"){
            //Instantiate(cube2, transform.position, Quaternion.identity);

            state = EnemyState.Chase;
            //追いかける対象をセット
            playerTransform = obj;
    


        }
        else if(mode == "wait")
        {
            elapsedTime = 0f;
            state = EnemyState.Wait;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
        else if(mode == "death")
        {

            state = EnemyState.Death;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Death", true);

            Destroy(gameObject, 12.0f);

        }
        
    }

    public EnemyState GetState()
    {
        return state;
    }


}
