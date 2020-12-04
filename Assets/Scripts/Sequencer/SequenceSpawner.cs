using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sequencer {

    public class SequenceSpawnerLink : MonoBehaviour {
        public SequenceSpawner spawner;
    }

    [ExecuteInEditMode]
    public class SequenceSpawner : SequenceTrigger {

        public Item prefab;

        protected override string GetName() => string.Format("Spawner:{0}", prefab?.gameObject.name ?? "None");
        protected override string GetHandleLabel() => prefab?.gameObject.name ?? "None";

        protected override void Trigger() {

            if (prefab != null) {
                var spawned = Instantiate(prefab, transform.position, Quaternion.identity);
                var link = spawned.gameObject.AddComponent<SequenceSpawnerLink>();
                link.spawner = this;
            }
        }
        
#if UNITY_EDITOR

        void OnDrawGizmos() {
            DrawGizmos();
        }

        [CustomEditor(typeof(SequenceSpawner))]
        class MyEditor : Editor {
            SequenceSpawner Target => target as SequenceSpawner;
            public override void OnInspectorGUI() {
                base.OnInspectorGUI();

                bool show = Target.transform.childCount == 0;
                string label = show ? "Show Prefab" : "Hide Prefab";
                if (show) {
                    var enabled = GUI.enabled;
                    GUI.enabled = Target.prefab != null;
                    if (GUILayout.Button(label)) {
                        var preview = Instantiate(Target.prefab, Target.transform.position, Quaternion.identity, Target.transform);
                        preview.gameObject.hideFlags = HideFlags.HideInHierarchy;
                    }
                    GUI.enabled = enabled;
                } else {
                    if (GUILayout.Button(label)) {
                        DestroyImmediate(Target.transform.GetChild(0).gameObject);
                    }
                }

                EditorGUILayout.LabelField(string.Format("Sequencer found: {0}", Target.sequencer));
            }
        }
#endif
    }
}
