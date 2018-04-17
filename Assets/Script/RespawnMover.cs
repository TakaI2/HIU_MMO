using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RespawnMover : MonoBehaviour {


    private float speed = 20;
    private float rotationSmooth = 2;

    public GameObject[] targets;
    public float[] DistArray = new float[4];

    public float NearDist;
    public GameObject NearOne;

    private Transform player;


    // Use this for initialization
    void Start () {
		


	}


    void LateUpdate()
    {
        //Playerタグがついているオブジェクトを全て取得。
        targets = GameObject.FindGameObjectsWithTag("Player");

        //targetsの中身を順番にチェックし、最も距離が近い物を選択する。


        for (int i = 0; i < targets.Length; i++)
        {
            float dist = Vector3.Distance(targets[i].transform.position, transform.position);

            DistArray[i] = dist;


            //ここで、DistArrayの最小値
            if (dist <= DistArray.Where(x => x > 0).Min())
            {
                NearDist = dist; //最も近いプレイヤーとの距離を取得する。
                NearOne = targets[i]; //最も近いプレイヤー

                player = targets[i].transform;
            }

        }

    }


    // Update is called once per frame
    void Update () {

        float sqrDistanceToPlayer = Vector3.SqrMagnitude(transform.position - player.position);


        // プレイヤーの方向を向く
        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);

        // 前方に進む
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //owner.cCon.Move(Vector3.forward * owner.speed * Time.deltaTime);


    }
}
