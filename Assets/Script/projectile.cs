using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class projectile : MonoBehaviour
    {

        public GameObject hitEffect;
        public int attackPower;
        public string target;

        public bool EnemyShot;
        public bool PlayerShot;

        private PhotonView m_photonView = null;
        private int playerCount;

        // Use this for initialization
        void OnCollisionEnter(Collision col)
        {
            playerCount = PhotonNetwork.room.PlayerCount;

            foreach (ContactPoint point in col.contacts)
            {
                GameObject effect = Instantiate(hitEffect) as GameObject;
                effect.transform.position = (Vector3)point.point;

            }

            if(PlayerShot == true && EnemyShot == false)
            {
                if (col.gameObject.tag == "Player")
                {

                    m_photonView = col.gameObject.GetComponent<PhotonView>();
                    m_photonView.RPC("Cure", PhotonTargets.All, 100 / playerCount);
                    //col.gameObject.GetComponent<Status>().Damage(power);
                }
                else if (col.gameObject.tag == "Enemy")
                {
                    // col.gameObject.GetComponent<EnemyBoss>().Damage(attackPower);
                    m_photonView = col.gameObject.GetComponent<PhotonView>();
                    m_photonView.RPC("Damage", PhotonTargets.All, attackPower / playerCount);
                }
                else if (col.gameObject.tag == "Zako")
                {
                    col.gameObject.GetComponent<MoveEnemy>().Damage(attackPower);

                }

            }
            else if (PlayerShot == false && EnemyShot == true)
            {
                if (col.gameObject.tag == "Player")
                {
                    m_photonView = col.gameObject.GetComponent<PhotonView>();
                    m_photonView.RPC("Damage", PhotonTargets.All, attackPower / playerCount);
                    //col.gameObject.GetComponent<Status>().Damage(power);
                }
            }




                Destroy(gameObject);

        }


    }
}



