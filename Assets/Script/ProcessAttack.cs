using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class ProcessAttack : MonoBehaviour
    {

        public Collider col;

        void AttackStart()
        {
            col.enabled = true;
        }

        void AttackEnd()
        {
            col.enabled = false;
        }

    }
}


