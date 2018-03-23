using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour {


    public Slider _PlayerHealthSlider;
    public Text _Health;
    public float hp;        //体力 


    // Use this for initialization
    void Start () {

        _PlayerHealthSlider = GameObject.Find("Slider").GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [PunRPC]
    public void Damage(int damage)
    {
        Debug.Log("敵に" + damage + "ポイント与えた");
        this.hp -= damage;

        _PlayerHealthSlider.value = hp;
        _Health.text = hp.ToString("F2");

        if (this.hp <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        Debug.Log("敵を倒した");

        //ここに、死亡モーションの命令を入れる。


        Destroy(gameObject);
    }



}
