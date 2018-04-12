using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


    public class Status : MonoBehaviour
    {

        public bool Arrive = true;
        public bool Dying = false;
        public GameObject cureEffect;
        public GameObject nameplate;
        public Slider _PlayerHealthSlider;
        public Text _Health;
        public float hp;        //体力 
        public float hpmax;


        // Use this for initialization
        void Start()
        {
            hpmax = hp;
            _PlayerHealthSlider = GameObject.Find("Slider").GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void LateUpdate()
        {
            nameplate.transform.rotation = Camera.main.transform.rotation; //ネームプレートは

            if(hp <= hpmax/2 )
            {
                Dying = true;
            }

        }

        [PunRPC]
        public void Damage(int damage)
        {
            if(hp >= 0 && hp >= damage)
            {
                Debug.Log("敵に" + damage + "ポイント与えた");
                this.hp -= damage;

                _PlayerHealthSlider.value = hp;
                _Health.text = hp.ToString("F2");
            }
            else if(hp < damage)
            {
                hp = 0;

                _PlayerHealthSlider.value = hp;
                _Health.text = hp.ToString("F2");
                Arrive = false;
            }
        }

        [PunRPC]
        public void Cure(int cure)
        {

            GameObject effect = Instantiate(cureEffect) as GameObject;
            effect.transform.position = transform.position;

            if (hp <= hpmax && cure >= hp)
            {
                this.hp += cure;

                _PlayerHealthSlider.value = hp;
                _Health.text = hp.ToString("F2");

                Arrive = true;
            }
            else
            {
            //this.hp += (hpmax - hp);
                _PlayerHealthSlider.value = hp;
                _Health.text = hp.ToString("F2");

                hp = hpmax; 
            }
            
                
            
        }

        void DeadEnd()
        {
            Debug.Log("敵を倒した");

            //ここに、死亡モーションの命令を入れる。
            SceneManager.LoadScene("ED");

            //Destroy(gameObject);
        }



    }




