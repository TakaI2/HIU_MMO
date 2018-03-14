using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class Attack : MonoBehaviour
    {

        public int attackPower; //攻撃力

        private PhotonView m_photonView = null;

        private int playerCount;

        void OnTriggerEnter(Collider col)
        {
            playerCount = PhotonNetwork.room.PlayerCount;

            if (col.gameObject.tag == "Enemy")
            {
                m_photonView = col.gameObject.GetComponent<PhotonView>();
                m_photonView.RPC("Damage", PhotonTargets.AllBuffered, (attackPower / playerCount));

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


