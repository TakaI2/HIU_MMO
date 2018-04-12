using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public GameObject gurdEffect;
    public Collider shield;
    public Collider shield_mass;
    public int attackPower;


    private void Update()
    {
        if (Input.GetButton("Run"))
        {
            shield.enabled = true;
            shield_mass.enabled = true;
        }
        else
        {
            
            shield.enabled = false;
            shield_mass.enabled = false;

        }
    }




    void OnTriggerEnter(Collider col) //OnTriggerEnter(Collider col)とどっちがいい?
    {

        GameObject effect = Instantiate(gurdEffect) as GameObject;
        effect.transform.position = transform.position;


        if (col.gameObject.tag == "Zako")
        {
            col.gameObject.GetComponent<MoveEnemy>().Damage(attackPower);
        }



    }

}
