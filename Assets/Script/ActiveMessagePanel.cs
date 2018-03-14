using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class ActiveMessagePanel : MonoBehaviour
    {

        //　MessageUIに設定されているMessageスクリプトを設定
        [SerializeField]
        private Message messageScript;

        //　表示させるメッセージ
        private string message = "斬撃が届かない敵は私にお任せください。\n"
                                 + "私の魔法ならば、遠くの敵を攻撃することができます。\n";

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire2"))
            {
                messageScript.SetMessagePanel(message);
            }
        }
    }
}

  
