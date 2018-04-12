using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour {

    private PhotonView m_photonView = null;
    private int playerCount;
    public GameObject beamEffect;
    //public Collider shield_mass;
    public int attackPower;

    void OnTriggerEnter(Collider col) //OnTriggerEnter(Collider col)とどっちがいい?
    {
        playerCount = PhotonNetwork.room.PlayerCount;


        GameObject effect = Instantiate(beamEffect) as GameObject;
        effect.transform.position = col.transform.position;

        if (col.gameObject.tag == "Player")
        {
            m_photonView = col.gameObject.GetComponent<PhotonView>();
            m_photonView.RPC("Damage", PhotonTargets.All, (attackPower / playerCount));
        }



    }

}
