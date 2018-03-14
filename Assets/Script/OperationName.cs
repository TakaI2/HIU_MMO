using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class OperationName : MonoBehaviour
    {
        private GameObject namePlate; // 名前を表示しているプレート
        public Text nameText; //名前を表示するテキスト

        // Use this for initialization
        void Start()
        {
            namePlate = nameText.transform.parent.gameObject;  //ネームプレートは親の位置に合わせて動くようにする。
        }


        void LateUpdate()
        {
            namePlate.transform.rotation = Camera.main.transform.rotation; //ネームプレートは
        }

        // Update is called once per frame
        [PunRPC]
        void SetName(string name)
        {
            nameText.text = name;
        }
    }

}

