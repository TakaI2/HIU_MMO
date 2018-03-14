using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineSample
{
    public enum EnemyState
    {
        Wander,
        Pursuit,
        LongAttack,
        ShortAttack,
        //Explode,
    }



    public class Enemy : StatefulObjectBase<Enemy,EnemyState>
    {

        public GameObject blockPrefab1;
        //public GameObject blockPrefab2;
        //public Transform muzzle;
        public float bulletpower;

        //public int maxLife;



        private CharacterController cCon;


        private int life;
        private float RandomBiasX;
        private float RandomBiasZ;

        private Transform player;
        private Animator animator;
        public Transform head;



        private Vector3 velocity;

        public float speed;
        public float rotationSmooth;
        public float headRotationSmooth;

        public float longattackInterval;
        public float shortattackInterval;

        public float attackSqrDistance1;  //遠距離攻撃に至る距離
        public float pursuitSqrDistance; //追跡行動に至る距離
        public float attackSqrDistance2;  //近距離攻撃に至る距離
        public float margin;

        public float changeTargetSqrDistance;

        

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {

            cCon = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            RandomBiasX = transform.position.x;
            RandomBiasZ = transform.position.z;

            //player = GameObject.FindWithTag("Player").transform; 

            //life = maxLife;

            stateList.Add(new StateWander(this));
            stateList.Add(new StatePursuit(this));
            stateList.Add(new StateLongAttack(this));
            stateList.Add(new StateShortAttack(this));
            //stateList.Add(new StateExplode(this));

            stateMachine = new StateMachine<Enemy>();

            ChangeState(EnemyState.Wander);

       

        }




        #region States

        ///state:  立ち尽くす

        private class StateWander : State<Enemy>
        {
            private Vector3 targetPosition;
            private bool wallcorid = false;

            public StateWander(Enemy owner) : base(owner) { }

            public override void Enter()
            {
                // 初めの目標地点をランダムに指定。
                //targetPosition = GetRandomPosition();

                //owner.player = GameObject.FindWithTag("Player").transform;

                /*
                if (GameObject.FindWithTag("Player"))
                {
                    owner.player = GameObject.FindWithTag("Player").transform;
                }
                */

            }

            public override void Execute()
            {

                if (GameObject.FindWithTag("Player"))
                {
                    owner.player = GameObject.FindWithTag("Player").transform;
                }

                // プレイヤーとの距離が第1攻撃距離より小さければ、遠距距離攻撃ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer < owner.attackSqrDistance1 - owner.margin)
                {
                        owner.ChangeState(EnemyState.LongAttack);
                }
                


                // 目標地点との距離が小さければ、次のランダムな目標地点を設定する
                float sqrDistanceToTarget = Vector3.SqrMagnitude(owner.transform.position - targetPosition);
                if ((sqrDistanceToTarget < owner.changeTargetSqrDistance) || wallcorid == true)
                {
                    wallcorid = false;

                    //targetPosition = GetRandomPosition();
                }

                //目標地点の方向を向く

                //Quaternion targetRotation = Quaternion.LookRotation(targetPosition - owner.transform.position);
                //owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);




                // 前方に進む
                //owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);

                //owner.cCon.Move(Vector3.forward * owner.speed * Time.deltaTime);

                owner.animator.SetFloat("Speed", 0f);

            }

            public override void Exit()
            {
                //   Instantiate(owner.blockPrefab1, owner.transform.position, Quaternion.identity);
                //owner.animator.SetFloat("Speed", 0f);
            }


            public Vector3 GetRandomPosition()
            {
                float x = Random.Range(owner.RandomBiasX - 10.0f, owner.RandomBiasX + 10.0f);
                float z = Random.Range(owner.RandomBiasZ - 10.0f, owner.RandomBiasZ + 10.0f);
                Debug.Log("X,Z: " + x.ToString("F2") + ", " + z.ToString("F2"));
                return new Vector3(x, 0.0f, z);
            }



        }


        ///state:  追跡

        private class StatePursuit : State<Enemy>
        {
            public StatePursuit(Enemy owner) : base(owner) { }

            public override void Enter()
            {

            }

            public override void Execute()
            {
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);

                
                // プレイヤーとの距離が第2攻撃距離より小さければ近距離攻撃ステートに遷移
                if (sqrDistanceToPlayer < owner.attackSqrDistance2 - owner.margin)
                {
                    owner.ChangeState(EnemyState.ShortAttack);
                }
                

                // プレイヤーとの距離が第２攻撃距離より大きければ、遠距離攻撃ステートに遷移
                if (sqrDistanceToPlayer > owner.pursuitSqrDistance + owner.margin)
                {
                    owner.ChangeState(EnemyState.LongAttack);
                }

                // プレイヤーの方向を向く
                Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

                // 前方に進む
                owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
                //owner.cCon.Move(Vector3.forward * owner.speed * Time.deltaTime);
                owner.animator.SetFloat("Speed", 2.1f);


            }

            public override void Exit()
            {
                owner.animator.SetFloat("Speed", 0f);
            }



        }

        /// <summary>
        /// ステート: 遠距離攻撃
        /// </summary>
        private class StateLongAttack : State<Enemy>
        {
            private float lastAttackTime;

            public StateLongAttack(Enemy owner) : base(owner) { }

            public override void Enter()
            {
                //    Instantiate(owner.blockPrefab2, owner.muzzle.position, owner.muzzle.rotation);


            }


            public override void Execute()
            {

                owner.animator.SetFloat("Speed", 0f);

                // プレイヤーとの距離が第一攻撃距離より大きければ、待機ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if ((sqrDistanceToPlayer > owner.attackSqrDistance1 + owner.margin) || (lastAttackTime >= 15))
                {
                    owner.player = null;
                    owner.ChangeState(EnemyState.Wander);
                }


                // プレイヤーとの距離が追跡開始距離より小さければ、追跡ステートに遷移
                if (sqrDistanceToPlayer < owner.pursuitSqrDistance - owner.margin)
                {
                    owner.ChangeState(EnemyState.Pursuit);
                }


                // プレイヤーとの距離が近くなれば、追跡ステートに遷移


                // 身体をプレイヤーの向きに向ける、
                Quaternion bodyRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, bodyRotation, Time.deltaTime * owner.rotationSmooth);

                // 砲台をプレイヤーの方向に向ける
                Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.head.position);

                targetRotation.z = 0;
                owner.head.rotation = Quaternion.Slerp(owner.head.rotation, targetRotation, Time.deltaTime * owner.headRotationSmooth);

                // 一定間隔で弾丸を発射する
                if (Time.time > lastAttackTime + owner.longattackInterval)
                {
                    //炸薬により実体弾を打ち出すタイプ。
                    GameObject laserInstance = GameObject.Instantiate(owner.blockPrefab1, owner.head.position, owner.head.rotation);
                    laserInstance.GetComponent<Rigidbody>().AddForce(laserInstance.transform.forward * owner.bulletpower);
                    Destroy(laserInstance, 5);


                    // Instantiate(owner.blockPrefab1, owner.muzzle.position, owner.muzzle.rotation);
                    lastAttackTime = Time.time;

                }
            }

            public override void Exit() { }
        }




        /// <summary>
        /// ステート: 近距離攻撃
        /// </summary>
        private class StateShortAttack : State<Enemy>
        {
            private float lastAttackTime;

            public StateShortAttack(Enemy owner) : base(owner) { }

            public override void Enter()
            {
                //    Instantiate(owner.blockPrefab2, owner.muzzle.position, owner.muzzle.rotation);


            }


            public override void Execute()
            {

                owner.animator.SetFloat("Speed", 0f);

                // プレイヤーとの距離が第2攻撃距離より大きければ、追跡ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer > owner.attackSqrDistance2 + owner.margin)
                {
                    owner.ChangeState(EnemyState.Pursuit);
                }


                // プレイヤーとの距離が近くなれば、追跡ステートに遷移


                // 身体をプレイヤーの向きに向ける、
                Quaternion bodyRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, bodyRotation, Time.deltaTime * owner.rotationSmooth);

                // 一定間隔で剣を振り下ろす。
                if (Time.time > lastAttackTime + owner.shortattackInterval)
                {
                    owner.animator.SetBool("Attack", true);

                    // Instantiate(owner.blockPrefab1, owner.muzzle.position, owner.muzzle.rotation);
                    lastAttackTime = Time.time;
                }
                
                
            }

            public override void Exit() { }
        }




        #endregion




    }



}


