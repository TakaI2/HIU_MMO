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


            if (col.gameObject.tag == target)
            {
                //Destroy(col.collider.gameObject);
                if (target == "Player")
                {
                    m_photonView = col.gameObject.GetComponent<PhotonView>();
                    m_photonView.RPC("Damage", PhotonTargets.AllBuffered, attackPower/playerCount);
                    //col.gameObject.GetComponent<Status>().Damage(power);
                }
                else if(target == "Enemy")
                {
                   // col.gameObject.GetComponent<EnemyBoss>().Damage(attackPower);
                    m_photonView = col.gameObject.GetComponent<PhotonView>();
                    m_photonView.RPC("Damage", PhotonTargets.AllBuffered, attackPower / playerCount);
                }


                Destroy(gameObject);

            }


            Destroy(gameObject);

        }


    }
}



