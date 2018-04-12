using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class Attack_E : MonoBehaviour
    {
        private PhotonView m_photonView = null;
        public GameObject hitEffect;
        public int attackPower; //攻撃力

        private int playerCount;


        void OnTriggerEnter(Collider col)
        {
            playerCount = PhotonNetwork.room.PlayerCount;

            GameObject effect = Instantiate(hitEffect) as GameObject;
            effect.transform.position = transform.position;

            if (col.gameObject.tag == "Player")
            {
                m_photonView = col.gameObject.GetComponent<PhotonView>();
                m_photonView.RPC("Damage", PhotonTargets.All, (attackPower/playerCount));
                //col.gameObject.GetComponent<Status>().Damage(attackPower);
            }
            

        }
    }

}


