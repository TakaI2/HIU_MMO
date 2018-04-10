using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementMC : MonoBehaviour {

    [SerializeField]
    public static bool BossArive = true;

    public int PlayerNum;


 
    void LateUpdate()
    {


        //ここで、プレイヤー全員死んだら、ゲームオーバー画面に遷移する処理。


    }


    public void ReturnOP()
    {
        BossArive = true;
        SceneManager.LoadScene("OP");

    }

    public void GamePlay()
    {
        SceneManager.LoadScene("main");
    }


}
