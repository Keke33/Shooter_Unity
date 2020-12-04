using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour {

    public static Player player;

    void OnEnable() {
        player = this;
    }

    public GameObject prefab;
    public int hp = 3;
    public int hpMax = 5;
    public bool invincible = false;

    void Start() {
        
        if (prefab == null) {
            Debug.LogWarning("Player.prefab doit être renseigné !!!");
            Debug.Break();
        } else {
            StartCoroutine(Spawn());
        }
    }

    public void RemoveOneHp() {
        
        hp += -1;

        if (hp == 0) {
            // gameover
            SceneManager.LoadScene("Game_Over");
            Debug.Log("GameOver");

            
        } else {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Spawn() {

        invincible = true;
        
        Vector3 position = Stage.instance.Bottom + Vector3.down;
        GameObject player = Instantiate(prefab, position, Quaternion.identity);

        player.GetComponent<PlayerMove>().enabled = false;
        foreach (var gun in player.GetComponentsInChildren<SuperGun>()) {
            gun.enabled = false;
        }
        player.AddComponent<PlayerSpawnAnimation>();

        yield return new WaitForSeconds(1);

        player.GetComponent<PlayerMove>().enabled = true;
        foreach (var gun in player.GetComponentsInChildren<SuperGun>()) {
            gun.enabled = true;
        }
        Destroy(player.GetComponent<PlayerSpawnAnimation>());

        yield return new WaitForSeconds(2);

        invincible = false;
    }

    IEnumerator Respawn() {

        yield return new WaitForSeconds(2);
        yield return Spawn();
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Player))]
    class MyEditor : Editor {
        Player player => target as Player;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (player.prefab == null) {
                EditorGUILayout.HelpBox("Attention !\nLe prefab est null !!!", MessageType.Warning);
            }
        }
    }
#endif
}
