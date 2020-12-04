using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerCore : MonoBehaviour {

    public Color invincibleColor = Color.red;

    void Update() {

        Color color = Player.player.invincible ? invincibleColor : Color.white;
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = color;    
    }

    void OnTriggerEnter(Collider other) {

        if (Player.player.invincible) {
            return;
        }

        Destroy(transform.parent.gameObject);
        
        Player.player.RemoveOneHp();
    }
}
