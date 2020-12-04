using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Explosion : MonoBehaviour
{
    public Item[] sources = new Item[0];
    public int count = 8;

    public float velocity = 4f;
    public float velocityVariation = 0.5f;

    public float timeMax = .6f;
    public float timeMaxVariation = 0.5f;

    public bool explodeOnDestroy = false;

    public bool hideClonesInHierarchy = true;

    public void Explode() {
        
        if (sources.Length == 0) {
            Debug.Log("Pas de sources, pas d'explosion !");
            return;
        }

        for (int index = 0; index < count; index++) {
            
            int sourceIndex = Random.Range(0, sources.Length);
            Item source = sources[sourceIndex];

            float rotationZ = Random.Range(0f, 360f);
            
            Item shard = Instantiate(source, transform.position, Quaternion.Euler(0f, 0f, rotationZ));
            shard.gameObject.hideFlags = hideClonesInHierarchy ? HideFlags.HideInHierarchy : HideFlags.None;

            float shardVelocity = velocity * (1f + Random.Range(-velocityVariation, velocityVariation));
            float shardVelocityX = shardVelocity * Mathf.Cos(rotationZ * Mathf.Deg2Rad);
            float shardVelocityY = shardVelocity * Mathf.Sin(rotationZ * Mathf.Deg2Rad);
            shard.velocity = new Vector3(shardVelocityX, shardVelocityY, 0f);

            shard.timeMax = timeMax * (1f + Random.Range(-timeMaxVariation, timeMaxVariation));
        }
    }

    bool applicationQuitting = false;
    void OnApplicationQuit() {
        applicationQuitting = true;
    }
    void OnDestroy() {
        if (explodeOnDestroy && Application.isPlaying && !applicationQuitting) {
            Explode();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Explosion))]
    class MyEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Explode!")) {
                (target as Explosion).Explode();
            }
        }
    }
#endif
}
