﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;




    public class SearchCharacter : MonoBehaviour
    {




    public bool sign = false;
    public Transform targetpos;

        void OnTriggerStay(Collider col)
        {


            //プレイヤーを発見
            if (col.tag == "Player")
            {
                

                //敵キャラの状態を取得
                Enemy.EnemyState state = GetComponentInParent<Enemy>().GetState();

                //敵キャラが追いかける状態でなければ追いかける設定に変更

                
                if (state != Enemy.EnemyState.Chase)
                {
                    Debug.Log("プレイヤー発見");
                    GetComponentInParent<Enemy>().SetState("chase", col.transform);



                }
                
            }
        }


        void OnTriggerExit(Collider col)
        {
            if (col.tag == "Player")
            {
                
                Debug.Log("見失う");
                GetComponentInParent<Enemy>().SetState("wait");
                

            }
        }


        public Transform GetPlayerTransform()
        {
            return targetpos;
        }



    }




