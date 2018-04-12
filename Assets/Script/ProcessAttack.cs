using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class ProcessAttack : MonoBehaviour
    {

        public AudioSource audioSource;
        public AudioClip[] se;

        public Collider col1;
        public Collider col2;


        void AttackStart()
        {
            col1.enabled = true;
            col2.enabled = true;
            audioSource.PlayOneShot(se[0]);
        }

        void AttackEnd()
        {
            col1.enabled = false;
            col2.enabled = false;
        }

    }
}


