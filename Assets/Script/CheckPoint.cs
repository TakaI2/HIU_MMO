using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class CheckPoint : MonoBehaviour
    {

        //public ActiveMessagePanel AMP;

        //void Start()
        //{
        //    AMP = GetComponent<ActiveMessagePanel>();
        //}


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                //AMP.messageflag = true;
                ActiveMessagePanel.messageflag = true;
                ActiveMessagePanel.point++;

                Destroy(gameObject);

            }
        }

    }
}


