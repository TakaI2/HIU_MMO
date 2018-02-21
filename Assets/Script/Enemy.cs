using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachineSample
{
    public enum EnemyState
    {
        Wander,
        Pursuit,
        Attack,
        Explode,
    }



    public class Enemy : StatefulObjectBase<Enemy,EnemyState>
    {

        public GameObject blockPrefab1;
        public GameObject blockPrefab2;
        public Transform muzzle;
        public float bulletpower;

        public int maxLife;




        private CharacterController cCon;


        private int life;
        private float RandomBiasX;
        private float RandomBiasZ;

        private Transform player;

        public float speed;
        public float rotationSmooth;
        public float attackInterval;

        public float pursuitSqrDistance;
        public float attackSqrDistance;
        public float margin;

        public float changeTargetSqrDistance;


        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {

            cCon = GetComponent<CharacterController>();

            RandomBiasX = transform.position.x;
            RandomBiasZ = transform.position.z;

            player = GameObject.FindWithTag("Player").transform;

            life = maxLife;

            stateList.Add(new StateWander(this));
            stateList.Add(new StatePursuit(this));
            stateList.Add(new StateAttack(this));
            stateList.Add(new StateExplode(this));

            stateMachine = new StateMachine<Enemy>();

            ChangeState(EnemyState.Wander);

        }

        public void TakeDamage()
        {
            life--;
            if (life <= 0)
            {
                ChangeState(EnemyState.Explode);
            }
        }



        #region States

        ///state:  徘徊

        private class StateWander : State<Enemy>
        {
            private Vector3 targetPosition;
            private bool wallcorid = false;

            public StateWander(Enemy owner) : base(owner) { }

            public override void Enter()
            {
                // 初めの目標地点をランダムに指定。
                targetPosition = GetRandomPosition();
            }

            public override void Execute()
            {

                // プレイヤーとの距離が小さければ、追跡ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer < owner.pursuitSqrDistance - owner.margin)
                {
                    owner.ChangeState(EnemyState.Pursuit);
                }

                // 目標地点との距離が小さければ、次のランダムな目標地点を設定する
                float sqrDistanceToTarget = Vector3.SqrMagnitude(owner.transform.position - targetPosition);
                if ((sqrDistanceToTarget < owner.changeTargetSqrDistance) || wallcorid == true)
                {
                    wallcorid = false;

                    targetPosition = GetRandomPosition();
                }

                //目標地点の方向を向く

                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);


                // 前方に進む
                owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);



            }

            public override void Exit()
            {
                //   Instantiate(owner.blockPrefab1, owner.transform.position, Quaternion.identity);
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

                // プレイヤーとの距離が小さければ攻撃ステートに遷移
                if (sqrDistanceToPlayer < owner.attackSqrDistance - owner.margin)
                {
                    owner.ChangeState(EnemyState.Attack);
                }


                // プレイヤーとの距離が大きければ、徘徊ステートに遷移
                if (sqrDistanceToPlayer > owner.pursuitSqrDistance + owner.margin)
                {
                    owner.ChangeState(EnemyState.Wander);
                }

                // プレイヤーの方向を向く
                Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

                // 前方に進む
                owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);

            }

            public override void Exit()
            {

            }



        }


        /// <summary>
        /// ステート: 攻撃
        /// </summary>
        private class StateAttack : State<Enemy>
        {
            private float lastAttackTime;

            public StateAttack(Enemy owner) : base(owner) { }

            public override void Enter()
            {
                //    Instantiate(owner.blockPrefab2, owner.muzzle.position, owner.muzzle.rotation);
            }

            public override void Execute()
            {
                // プレイヤーとの距離が大きければ、追跡ステートに遷移
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
                if (sqrDistanceToPlayer > owner.attackSqrDistance + owner.margin)
                {
                    owner.ChangeState(EnemyState.Pursuit);
                }


                // 一定間隔で弾丸を発射する
                if (Time.time > lastAttackTime + owner.attackInterval)
                {
                    //炸薬により実体弾を打ち出すタイプ。
                    GameObject laserInstance = GameObject.Instantiate(owner.blockPrefab1, owner.muzzle.position, owner.muzzle.rotation);
                    laserInstance.GetComponent<Rigidbody>().AddForce(laserInstance.transform.forward * owner.bulletpower);
                    Destroy(laserInstance, 5);


                    // Instantiate(owner.blockPrefab1, owner.muzzle.position, owner.muzzle.rotation);
                    lastAttackTime = Time.time;
                }
            }

            public override void Exit() { }
        }

        /// <summary>
        /// ステート: 爆発
        /// </summary>
        private class StateExplode : State<Enemy>
        {
            public StateExplode(Enemy owner) : base(owner) { }

            public override void Enter()
            {
                // ランダムな吹き飛ぶ力を加える
                Vector3 force = Vector3.up * 1000f + Random.insideUnitSphere * 300f;
                owner.GetComponent<Rigidbody>().AddForce(force);

                // ランダムに吹き飛ぶ回転力を加える
                Vector3 torque = new Vector3(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
                owner.GetComponent<Rigidbody>().AddTorque(torque);

                // 1秒後に自身を消去する
                Destroy(owner.gameObject, 1.0f);
            }

            public override void Execute() { }

            public override void Exit() { }
        }




        #endregion




    }



}


