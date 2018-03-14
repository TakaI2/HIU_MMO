using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class EnemyBoss : Photon.MonoBehaviour
    {
        private int hp; //　敵の体力

        void Start()
        {
            hp = 500;
        }

        [PunRPC]
        public void Damage(int damage)
        {
            Debug.Log("敵に" + damage + "ポイント与えた");
            this.hp -= damage;
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


    }
}


