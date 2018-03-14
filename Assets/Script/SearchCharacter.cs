using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : MonoBehaviour {

    public string tagg;

    void OnTriggerStay(Collider col)
    {
        tagg = col.tag;

        //プレイヤーを発見
        if(col.tag == "Player")
        {
            //敵キャラの状態を取得
            MoveEnemy.EnemyState state = GetComponent<MoveEnemy>().GetState();
            //敵キャラが追いかける状態でなければ追いかける設定に変更




            GetComponent<MoveEnemy>().SetState("chase", col.transform); 
            /*
            if (state != MoveEnemy.EnemyState.Chase)
            {
                Debug.Log("プレイヤー発見");
                GetComponent<MoveEnemy>().SetState("chase", col.transform);
            }*/
        }
    }


    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            Debug.Log("見失う");
            GetComponentInParent<MoveEnemy>().SetState("wait");
        }
    }

}
