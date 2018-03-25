using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class NetworkManager : MonoBehaviour
    {

        public Text text;
        public GameObject loginUI;
        public Dropdown roomList;
        public InputField roomName;
        public InputField playerName;
        public GameObject logoutUI;

        private int charaNo = 0;
        private Vector3 respawnpoint;

        // Use this for initialization
        void Start()
        {

            //ログをすべて表示する。
            PhotonNetwork.logLevel = PhotonLogLevel.Full;

            //ロビーに自動で入る
            PhotonNetwork.autoJoinLobby = true;

            //ゲームのバージョン設定
            PhotonNetwork.ConnectUsingSettings("0.1");


            //respawnpoint = GameObject.FindWithTag("Respawn").transform.position;
        }



        // Update is called once per frame
        void Update()
        {

            //サーバ接続状態を表示
            text.text = PhotonNetwork.connectionStateDetailed.ToString();

        }


        //マスターサーバに接続されたときに呼ばれる。
        void OnConnectedToMaster()
        {
            Debug.Log("マスターサーバに接続");
        }

        //ロビーに入った時に呼ばれる。
        void OnJoinedLobby()
        {
            Debug.Log("ロビーに入る");
            loginUI.SetActive(true);
        }

        //ログインボタンを押したときに実行するメソッド
        public void LoginGame()
        {
            //ルームオプションを設定
            RoomOptions ro = new RoomOptions();
            //ルームを見えるようにする
            ro.IsVisible = true;
            //部屋の入室最大人数
            ro.MaxPlayers = 10;

            if (roomName.text != "")
            {
                //部屋がない場合は作って入室
                PhotonNetwork.JoinOrCreateRoom(roomName.text, ro, TypedLobby.Default);

            }
            else
            {
                //部屋が存在すれば
                if (roomList.options.Count != 0)
                {
                    Debug.Log(roomList.options[roomList.value].text);
                    PhotonNetwork.JoinRoom(roomList.options[roomList.value].text);
                    //部屋が存在しなければDefaultRoomという名前で部屋を作成
                }
                else
                {
                    PhotonNetwork.JoinOrCreateRoom("DefaultRoom", ro, TypedLobby.Default);
                }
            }

        }

        //部屋が更新されたときの処理
        void OnReceivedRoomListUpdate()
        {
            Debug.Log("部屋更新");

            //部屋情報を取得する
            RoomInfo[] rooms = PhotonNetwork.GetRoomList();

            //ドロップダウンリストに追加する文字列用のリストを作成
            List<string> list = new List<string>();

            //部屋情報を部屋リストに表示
            foreach (RoomInfo room in rooms)
            {
                //部屋が満員でなければ追加
                if (room.PlayerCount < room.MaxPlayers)
                {
                    list.Add(room.Name);
                }
            }

            //ドロップダウンリストをリセット
            roomList.ClearOptions();


            //部屋が一つでもあればドロップダウンリストに追加
            if (list.Count != 0)
            {
                roomList.AddOptions(list);
            }

        }

        //部屋に入室したときに呼ばれるメソッド
        void OnJoinedRoom()
        {
            loginUI.SetActive(false);
            logoutUI.SetActive(true);
            Debug.Log("入室");

            respawnpoint = GameObject.FindWithTag("Respawn").transform.position;

            //InputFieldに入力した名前を設定
            if (playerName.text != "")
            {
                PhotonNetwork.player.NickName = playerName.text;
            }
            else
            {
                PhotonNetwork.player.NickName = "DefaultPlayer";
            }

            // ログインを監視する


            if (charaNo == 0)
            {
                StartCoroutine("SetPlayer", 0f);
            }
            else
            {
                StartCoroutine("SetPlayer2", 0f);
            }

        }


        // プレイヤーをゲームの世界に出現させる
        IEnumerator SetPlayer(float time)
        {
            yield return new WaitForSeconds(time);
            // ネットワークごしにキャラをインスタンス化 ここで、キャラを選択する。


            //GameObject player = PhotonNetwork.Instantiate("Yusha_rigify", Vector3.up, Quaternion.identity, 0);
            GameObject player = PhotonNetwork.Instantiate("Yusha_rigify", respawnpoint, Quaternion.identity, 0);

            player.GetPhotonView().RPC("SetName", PhotonTargets.AllBuffered, PhotonNetwork.player.NickName);
        }

        //この雑な書き方はあとで改めるべし
        IEnumerator SetPlayer2(float time)
        {
            yield return new WaitForSeconds(time);
            // ネットワークごしにキャラをインスタンス化 ここで、キャラを選択する。

            //GameObject player = PhotonNetwork.Instantiate("Sample_chan_repair", Vector3.up, Quaternion.identity, 0);
            GameObject player = PhotonNetwork.Instantiate("Sample_chan_repair", respawnpoint, Quaternion.identity, 0);


            player.GetPhotonView().RPC("SetName", PhotonTargets.AllBuffered, PhotonNetwork.player.NickName);
        }



        //部屋の入室に失敗した
        void OnPhotonJoinRoomFailed()
        {
            Debug.Log("入室に失敗");

            //ルームオプションを設定
            RoomOptions ro = new RoomOptions();
            //ルームを見えるようにする
            ro.IsVisible = true;
            //部屋の最大入室人数
            ro.MaxPlayers = 10;
            //入室に失敗したらDefaultRoomを作成し入室
            PhotonNetwork.JoinOrCreateRoom("DefaultRoom", ro, TypedLobby.Default);
        }


        //LogoutButtonを押したときの処理
        public void LogoutGame()
        {
            PhotonNetwork.LeaveRoom();
        }

        //部屋を退室したときの処理
        void OnLeftRoom()
        {
            Debug.Log("退室");
            logoutUI.SetActive(false);
        }


        //キャラクターを選択する。
        public void SelectSworder()
        {
            charaNo = 0;
        }

        public void SelectMage()
        {
            charaNo = 1;
        }


    }

}


