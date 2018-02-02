using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class Chara : MonoBehaviour
    {

        private Animator animator;
        private CharacterController cCon;
        private float x;
        private float y;
        private Vector3 velocity;
        [SerializeField]
        private float walkSpeed = 1.5f;
        [SerializeField]
        private float runSpeed = 2.5f;

        //走っているか？
        private bool runFlag = false;

        //キャラ視点のカメラ
        private Transform myCamera;

        //キャラクター視点のカメラで回転できる限度
        [SerializeField]
        private float cameraRotationLimit = 30f;

        //カメラの上下の移動方法。マウスを上で上を向く場合はtrue,マウスを↑で下を向く場合はfalseを設定
        [SerializeField]
        private bool cameraRotForward = true;

        //カメラの角度の初期値
        private Quaternion initCameraRot;

        //キャラクター、カメラ（視点）の回転スピード
        [SerializeField]
        private float rotateSpeed = 2f;

        // カメラのx軸の角度変化値
        [SerializeField]
        private float xRotate;

        // キャラクターのY軸の角度変化値
        private float yRotate;

        //マウス移動のスピード
        [SerializeField]
        private float mouseSpeed = 2f;

        // キャラクターのY軸の角度
        private Quaternion charaRotate;
        // カメラのX軸の角度
        private Quaternion cameraRotate;

        //キャラが回転中かどうか？
        private bool charaRotFlag = false;


        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            cCon = GetComponent<CharacterController>();
            velocity = Vector3.zero;
            charaRotate = transform.localRotation;
            cameraRotate = myCamera.localRotation;
        }

        // Update is called once per frame
        void Update()
        {

            RotateChara();
            //RotateCamera();

            //　地面に接地してる時は初期化
            if (cCon.isGrounded)
            {
                velocity = Vector3.zero;

                velocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;

                //走るか歩くかでスピードを変える。
                float speed = 0f;

                if(Input.GetButton("Run"))
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

                if(velocity.magnitude > 0f || charaRotFlag)
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
            }


            velocity.y += Physics.gravity.y * Time.deltaTime;
            cCon.Move(velocity * Time.deltaTime);

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




    }

}

