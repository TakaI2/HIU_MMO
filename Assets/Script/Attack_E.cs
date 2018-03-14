using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class Attack_E : MonoBehaviour
    {
        private PhotonView m_photonView = null;

        public int attackPower; //攻撃力

        private int playerCount;


        void OnTriggerEnter(Collider col)
        {
            playerCount = PhotonNetwork.room.PlayerCount;

            if (col.gameObject.tag == "Player")
            {
                m_photonView = col.gameObject.GetComponent<PhotonView>();
                m_photonView.RPC("Damage", PhotonTargets.AllBuffered, (attackPower/playerCount));
                //col.gameObject.GetComponent<Status>().Damage(attackPower);
            }
            

        }
    }

}


