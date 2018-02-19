using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

namespace Com.MyCompany.MyGame
{
    public class shooter : Photon.MonoBehaviour
    {

        public GameObject bullet;
        public float bulletpower;
        private PhotonView m_photonView = null;


        private bool mineflag = false;
        private bool Remoteflag = false;

        //ネットワークを通じて共有する自身の座標、および発射フラグ
        Quaternion rotation;
        float smooth = 10f;
        public bool shotflag = false;

        void Awake()
        {
            m_photonView = GetComponent<PhotonView>();
        }


        private void Start()
        {
            if (!photonView.isMine)
            {

                rotation = transform.rotation;

                Remoteflag = true;

                StartCoroutine("Remote");

            }
            else
            {
                mineflag = true;
            }

        }
        
            



        // Update is called once per frame
        void Update()
        {

            if (Input.GetButtonDown("Fire1")) //このコード内でトリガーを引く場合
            {
                shotflag = true;
            }


            if (mineflag) //自分のキャラでないとmineflagが立たない。
            {
                Transform camera = Camera.main.transform;

                //Ray ray = new Ray(camera.position, camera.rotation * Vector3.forward); //カメラの向いた方向に撃つ場合。VRの時など。
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //マウスカーソルの位置に向けて撃つ場合。
                transform.rotation = Quaternion.LookRotation(ray.direction);

                
                if (shotflag) //このコード内でトリガーを引く場合
                {
                    m_photonView.RPC("shot", PhotonTargets.All, shotflag);

                    shotflag = false;
                }
            }
        }


        //自分のキャラクター以外のデータを同期
        IEnumerator Remote()
        {
            while (true)
            {
                //スムーズに補間させる
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);

                yield return null;
            }
        }


        // Observed Componentsに設定したｽｸﾘﾌﾟﾄで呼ばれるメソッド
        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // データの読み込み
            if (stream.isReading)
            {
                rotation = (Quaternion)stream.ReceiveNext();
            }
            else
            {
                stream.SendNext(transform.rotation);
            }
        }

        [PunRPC]
        public void shot(bool trigger)
        {
            if(trigger)
            {
                GameObject CastInstance = GameObject.Instantiate(bullet, transform.position, transform.rotation);
                CastInstance.GetComponent<Rigidbody>().AddForce(CastInstance.transform.forward * bulletpower);
                Destroy(CastInstance, 5);
            }
            
        }

    }
}

 
