﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class ActiveMessagePanel : MonoBehaviour
    {
        public static int point;
        public static bool messageflag;

        //　MessageUIに設定されているMessageスクリプトを設定
        [SerializeField]
        private Message messageScript;

        //　表示させるメッセージ
        private string message1 = "斬撃が届かない敵は私に任せて。\n"
                                 + "私の魔法ならば、遠くの敵を攻撃することができる。\n";

        private string message2 = "私も、この街道を通って王都に向かう予定だったの。\n"
                                 + "こんな形で足止めを食うなんて災難だったわ。";

        private string message3 = "この先の峠道には注意して。ゴーレムがいるわ。旅人や馬車はここで襲われるらしいの。\n"
                                 + "たぶん、遺跡の残骸が散らばっているせいでこんなところまで来ているのね。";


        private string message4 ="街道は塞がっているわ。\n"
                                +"怪物の仕業ね。これじゃあ、通れないわ・・・。\n"
                                + "素通りして逃げるってわけにはいかないわけね。";

        private string message5 = "ここが、山の遺跡。 中はゴーレムがウヨウヨしてるから気を付けて。";

        private string message6 = "イーストアイルではゴーレムは動けないって話、信じてたの？　\n"
                                + "見ての通り、遺跡の中までは聖女様の加護も届かないみたいね。"
                                + "怪物は巣を守るためにこいつらを利用しているようね。";

        private string message7 = "いたわ！！　\n"
                                + "気を付けて！　ゴーレムとは比べ物にならない強さよ。 ";
                               


        void Start()
        {
            point = 0;
            messageflag = false;
        }


        // Update is called once per frame
        void Update()
        {


            //ステージに配置されたチェックポイントを通過するごとに、メッセージを再生。

            if (Input.GetButtonDown("Fire2"))
            {
                messageScript.SetMessagePanel(message1);
            }

            if (point == 1 && messageflag == true)
            {
                messageScript.SetMessagePanel(message2);
                messageflag = false;
            }

            if(point == 2 && messageflag == true)
            {
                messageScript.SetMessagePanel(message3);
                messageflag = false;
            }

            if (point == 3 && messageflag == true)
            {
                messageScript.SetMessagePanel(message4);
                messageflag = false;
            }

            if (point == 4 && messageflag == true)
            {
                messageScript.SetMessagePanel(message5);
                messageflag = false;
            }

            if (point == 5 && messageflag == true)
            {
                messageScript.SetMessagePanel(message6);
                messageflag = false;
            }

            if (point == 6 && messageflag == true)
            {
                messageScript.SetMessagePanel(message7);
                messageflag = false;
            }

        }
    }
}

  
