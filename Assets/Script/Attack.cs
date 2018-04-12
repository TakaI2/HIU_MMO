using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class Attack : MonoBehaviour
    {
        public GameObject hitEffect;
        public int attackPower; //攻撃力

        private PhotonView m_photonView = null;

        private int playerCount;

        void OnTriggerEnter(Collider col) //OnTriggerEnter(Collider col)とどっちがいい?
        {
            playerCount = PhotonNetwork.room.PlayerCount;
            
 
            GameObject effect = Instantiate(hitEffect) as GameObject;
            effect.transform.position = transform.position;
            

            if (col.gameObject.tag == "Enemy")
            {
                m_photonView = col.gameObject.GetComponent<PhotonView>();
                m_photonView.RPC("Damage", PhotonTargets.AllBuffered, (attackPower / playerCount));
                col.gameObject.GetComponent<MoveEnemy>().Damage(attackPower);

            }
            else if(col.gameObject.tag == "Zako")
            {
                col.gameObject.GetComponent<MoveEnemy>().Damage(attackPower);
            }

            /*
            if (col.gameObject.tag == "Player")
            {
                col.gameObject.GetComponent<Chara>().Damage(attackPower);
            }
            */

        }



    }
}


