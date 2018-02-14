using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class shooter : MonoBehaviour
    {

        public GameObject bullet;
        public float bulletpower;



        // Update is called once per frame
        void Update()
        {

            Transform camera = Camera.main.transform;

            //Ray ray = new Ray(camera.position, camera.rotation * Vector3.forward); //カメラの向いた方向に撃つ場合。VRの時など。
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //マウスカーソルの位置に向けて撃つ場合。
            transform.rotation = Quaternion.LookRotation(ray.direction);


            if (Input.GetButtonDown("Fire1"))
            {

                Invoke("shot", 0.2f);

            }

        }

        void shot()
        {

            GameObject CastInstance = GameObject.Instantiate(bullet, transform.position, transform.rotation);
            CastInstance.GetComponent<Rigidbody>().AddForce(CastInstance.transform.forward * bulletpower);
            Destroy(CastInstance, 5);

        }

    }
}

 
