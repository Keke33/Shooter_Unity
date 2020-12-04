using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

namespace Sequencer {
    
    public class Sequencer : MonoBehaviour {

        public float triggerSize = 30f;
        public float velocity = 2f;
        public float timeScale = 1f;

        public Color gizmosColor = new Color(1f, 0.5f, 1f);
        public bool hidePassedSpawner = true;

        public float scroll = 0;

        void Update() {

            scroll += velocity * timeScale * Time.deltaTime;

            foreach(var sequence in GetComponentsInChildren<Sequence>()) {
                sequence.transform.localPosition = sequence.initialPosition + Vector3.left * scroll;
            }
        }

        public void Jump(float destination) {

            scroll = destination;
        }

        public void Jump(SequenceTrigger trigger) => Jump(-trigger.sequencerPosition.x);

        public void Jump(string name) {

            var sequenceTrigger = GetComponentsInChildren<SequenceTrigger>()
                .FirstOrDefault(x => x.gameObject.name == name);

            if (sequenceTrigger != null) {

                Jump(sequenceTrigger);

            } else {

                Debug.LogFormat("Il n'y a pas d'objet <SequenceTrigger> qui s'appelle \"{0}\".", name);
            }
        }

        void DrawArrow(Vector3 position, float size) {
            Gizmos.DrawLine(position, position + new Vector3(size, size, 0f));
            Gizmos.DrawLine(position, position + new Vector3(size, -size, 0f));
        }

        void OnDrawGizmos() {
            Gizmos.color = gizmosColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0f, triggerSize + 2f, 2f));
            
            int max = Mathf.FloorToInt(triggerSize / 2f / 2f);
            for (int i = -max; i <= max; i++) {
                DrawArrow(new Vector3(0f, i * 2f, 0f), 0.5f);
            }

            Gizmos.color = new Color(gizmosColor.r, gizmosColor.g, gizmosColor.b, 0.25f);
            Gizmos.DrawCube(Vector3.zero, new Vector3(0f, triggerSize + 2f, 2f));
        }
    }
}
