using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class Attack : MonoBehaviour
    {

        public int attackPower; //攻撃力

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<EnemyBoss>().Damage(attackPower);
            }

        }

    }
}


