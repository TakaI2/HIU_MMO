using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;


namespace Com.MyCompany.MyGame
{
    public class NetworkPlayerCheck : Photon.PunBehaviour
    {

        //同期するデータ
        Vector3 position;
        Quaternion rotation;
        float hp;
        Animator animator;
        float speed;
        bool attack;

        //スムーズ度合
        float smooth = 10f;

        // Use this for initialization
        void Start()
        {

            animator = GetComponent<Animator>();

            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();  //CameraWorkが認識されないことがあるが、Unityを立ち上げ直したら治ってる。

            /*
            if (_cameraWork != null)
            {
                if(photonView.isMine)
                {
                    _cameraWork.OnStartFollowing();
                }


            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }*/


            //自分で操作する以外のキャラの機能を使えないようにする。
            if (!photonView.isMine)
            {
                GetComponent<Chara>().enabled = false;
                //GetComponent<ProcessMyAttack>().enabled = false;
                GetComponent<CameraWork>().enabled = false;
                //GetComponentInChildren<AttackSword>().enabled = false;


                // 初期値を設定
                position = transform.position;
                rotation = transform.rotation;
                //hp = GetComponent<Status>().GetHp();

                speed = animator.GetFloat("Speed");
                attack = animator.GetBool("Attack");

                // 自分のキャラクター以外のデータを同期
                StartCoroutine("UpdateMove");

            }
        }

        
        //自分のキャラクター以外のデータを同期
        IEnumerator UpdateMove()
        {
            while (true)
            {
                //スムーズに補間させる
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smooth);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);
                animator.SetFloat("Speed", speed);
                animator.SetBool("Attack", attack);

                yield return null;
            }
        }
        
        
        // Observed Componentsに設定したｽｸﾘﾌﾟﾄで呼ばれるメソッド
        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // データの読み込み
            if (stream.isReading)
            {
                position = (Vector3)stream.ReceiveNext();
                rotation = (Quaternion)stream.ReceiveNext();
                hp = (float)stream.ReceiveNext();
                speed = (float)stream.ReceiveNext();
                attack = (bool)stream.ReceiveNext();
            }
            else
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(hp);
                stream.SendNext(animator.GetFloat("Speed"));
                stream.SendNext(animator.GetBool("Attack"));
            }
        }
        //Update is called once per frame
        void Update()
        {

        }


    }

}

