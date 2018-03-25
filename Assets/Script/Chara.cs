using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class Chara : MonoBehaviour
    {

        public GameObject target;

       // public Slider _PlayerHealthSlider;
       // public float hp = 30;        //体力 



        private Animator animator;
        private CharacterController cCon;
        private float x;
        private float y;
        private Vector3 velocity;
        private float rotation;

        private int center;
        private float stair;


        //キャラ移動関係関数
        [SerializeField]private float walkSpeed = 1.5f;
        [SerializeField]private float runSpeed = 2.5f;
        [SerializeField] private float jumpPower = 5f;


        //カメラ制御用変数
        [SerializeField] private float distance = 4.0f; //ターゲットとカメラの距離1
        [SerializeField] private float polarAngle = 45.0f; // カメラのy軸角度
        [SerializeField] private float azimuthalAngle = 45.0f; //カメラのx軸角度

        [SerializeField] private float minDistance = 1.0f;
        [SerializeField] private float maxDistance = 7.0f;
        [SerializeField] private float minPolarAngle = 5.0f;
        [SerializeField] private float maxPolarAngle = 75.0f;
        [SerializeField] private float mouseXSensitivity = 5.0f;
        [SerializeField] private float mouseYSensitivity = 5.0f;
        [SerializeField] private float scrollSensitivity = 5.0f;


        //移動しているか？
        private bool moveFlag = false;

        //走っているか？
        private bool runFlag = false;

        //キャラ視点のカメラ
        private Transform myCamera;

        //カメラの高さ
        public float height = 1.0f;

        public float heightSmoothLag = 0.3f;

        public Vector3 centerOffset = Vector3.zero;

        private float targetHeight = 1.0f;

        // Represents the current velocity, this value is modified by SmoothDamp() every time you call it.
        private float heightVelocity = 0.0f;



        //キャラクター視点のカメラで回転できる限度
        [SerializeField]
        private float cameraRotateLimit = 30f;

        //カメラの上下の移動方法。マウスを上で上を向く場合はtrue,マウスを↑で下を向く場合はfalseを設定
        [SerializeField]
        private bool cameraRotForward = true;

        //カメラの角度の初期値
        private Quaternion initCameraRot;

        //キャラクター、カメラ（視点）の回転スピード
        [SerializeField]
        private float rotateSpeed = 2f;

        //マウス移動のスピード
        [SerializeField]
        private float mouseSpeed = 2f;

        //キャラ加えるに回転方向の力
        private Quaternion charaRotate;

        //キャラが回転中かどうか？
        private bool charaRotFlag = false;


        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            cCon = GetComponent<CharacterController>();
            myCamera = Camera.main.transform;//GetComponentInChildren<Camera>().transform; //Camera.main.transform//ここでmainのカメラを取得するようにしたらうまくいくのでは？
            velocity = Vector3.zero;
            charaRotate = transform.localRotation;
            center = (Screen.width) / 2;

            //_PlayerHealthSlider = GameObject.Find("Slider").GetComponent<Slider>();
            

        }

        // Update is called once per frame
        void Update()
        {

            RotateChara2();
            //RotateCamera();


            //　地面に接地してる時は初期化
            if (cCon.isGrounded)
            {
                velocity = Vector3.zero;

                velocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
                //velocity = (transform.forward * Input.GetAxis("Vertical"));

                //float rotation = Input.GetAxis("Horizontal");

                //var rotVec = Quaternion.Euler(0f, rotation, 0f);

               // velocity = rotVec * velocity;

                //走るか歩くかでスピードを変える。
                float speed = 0f;

                if(velocity != Vector3.zero)
                {
                    moveFlag = true;
                }
                else
                {
                    moveFlag = false;
                }


                if (Input.GetButton("Run"))
                {
                    runFlag = true;
                    speed = runSpeed;

                }
                else
                {
                    runFlag = false;
                    speed = walkSpeed;
                }
                velocity *= speed;

                if (velocity.magnitude > 0f || charaRotFlag)
                {
                    if (runFlag && !charaRotFlag)
                    {
                        animator.SetFloat("Speed", 2.1f);
                    }
                    else
                    {
                        animator.SetFloat("Speed", 1f);
                    }

                }
                else
                {
                    animator.SetFloat("Speed", 0f);
                }

                if (Input.GetButtonDown("Fire1")
                    && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
                    && !animator.IsInTransition(0)
                )
                {
                    animator.SetTrigger("Attack");

                }

                //ジャンプキーを押したらY方向へのジャンプ力を足す
                if(Input.GetButtonDown("Jump") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                {
                    animator.SetBool("Jump", true);
                    velocity.y += jumpPower;
                }


            }

      


            velocity.y += Physics.gravity.y * Time.deltaTime;
            cCon.Move(velocity * Time.deltaTime);

        }


        void LateUpdate()
        {

           
            if(moveFlag)
            {
                stair = (Input.mousePosition.x - center) / center * Time.deltaTime * rotateSpeed;
            }
            else
            {
                stair = 0;
            }
                
            

            updateAngle(Input.GetAxis("Mouse X") + stair, Input.GetAxis("Mouse Y"));
            
            updateDistance(Input.GetAxis("Mouse ScrollWheel"));

            var lookAtPos = transform.position + centerOffset;　　　//変数lookAtPosに、targetに指定したオブジェクトの位置にオフセットを足した値を入力
            updatePosition(lookAtPos);                                     //ターゲットの座標lookAtPosを計算。
            myCamera.transform.LookAt(lookAtPos);                                   //カメラの角度をターゲットの座標lookAtPosに向ける。

        }

        void updateAngle(float x, float y) //カメラの角度を常にターゲットに向ける関数
        {
            x = azimuthalAngle - x * mouseXSensitivity;                     //現在のカメラのx軸角度から、マウスのx方向の移動量だけ角度を動かす。
            azimuthalAngle = Mathf.Repeat(x, 360);                          //カメラのx軸角度は　360より大きくはならず、その間をループする。

            y = polarAngle + y * mouseYSensitivity;                         //現在のカメラのy軸角度から、マウスのy方向の移動量だけ角度を動かす。
            polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);      //カメラのy軸角度はminPolarAngle～maxPolarAngle間に制限する。


        }

        void updateDistance(float scroll) //カメラとターゲットの距離をマウススクロールで変化させるための関数。
        {
            scroll = distance - scroll * scrollSensitivity;                 //変数scrollの値を、カメラとターゲットとの距離からマウスのスクロール量を引いた値にする。
            distance = Mathf.Clamp(scroll, minDistance, maxDistance);       //変数distanceの値を、minDistance, maxDistanceの間に制限する。
        }

        void updatePosition(Vector3 lookAtPos) //
        {
            var da = azimuthalAngle * Mathf.Deg2Rad;                        //x軸方向の角度を°からラジアンに変換
            var dp = polarAngle * Mathf.Deg2Rad;                            //y軸方向の角度を°からラジアンに変換
            myCamera.transform.position = new Vector3(
                lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),     //ターゲットが移動した際に、角度とlookAtPosのx方向にに　距離 * 
                lookAtPos.y + distance * Mathf.Cos(dp),
                lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
        }
    




        //キャラクターの角度を変更
        void RotateChara()
        {
            //  横の回転値を計算
            float yRotate = Input.GetAxis("Mouse X") * mouseSpeed;

            charaRotate *= Quaternion.Euler(0f, yRotate, 0f);

            //キャラは回転しているか？
            if(yRotate != 0f)
            {
                charaRotFlag = true;
            }
            else
            {
                charaRotFlag = false;
            }


            // キャラクターの回転を実行
            transform.localRotation = Quaternion.Slerp(transform.localRotation, charaRotate, rotateSpeed * Time.deltaTime);
        }


        //キャラクターの向き変更関数（改良)
        void RotateChara2()
        {


            float rotation = Input.GetAxis("Mouse X");

            //var rotVec = Quaternion.Euler(0f, rotation, 0f);
            //var rotVec = new Vector3(0, rotation, 0)*100;
            // velocity = rotVec * velocity;



            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 Horizontal_direction = ray.direction;

            var charaRotation = Quaternion.LookRotation(Horizontal_direction);
            charaRotation.x = 0;
            charaRotation.z = 0;

            transform.rotation = charaRotation;


            //transform.rotation = Quaternion.Slerp(transform.localRotation, charaRotation, rotateSpeed * Time.deltaTime);
        }

        /*
        [PunRPC]
        public void Damage(int damage)
        {
            Debug.Log("敵に" + damage + "ポイント与えた");
            this.hp -= damage;

            _PlayerHealthSlider.value = hp;

            if (this.hp <= 0)
            {
                Dead();
            }
        }

        void Dead()
        {
            Debug.Log("敵を倒した");
            Destroy(gameObject);
        }
        */

    }

}

