using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class projectile : MonoBehaviour
    {

        public GameObject hitEffect;
        public int power;

        // Use this for initialization
        void OnCollisionEnter(Collision col)
        {

            foreach (ContactPoint point in col.contacts)
            {
                GameObject effect = Instantiate(hitEffect) as GameObject;
                effect.transform.position = (Vector3)point.point;

            }


            if (col.gameObject.tag == "Enemy")
            {
                //Destroy(col.collider.gameObject);
                col.gameObject.GetComponent<EnemyBoss>().Damage(power);
                // Destroy(gameObject);

            }


            Destroy(gameObject);

        }


    }
}



