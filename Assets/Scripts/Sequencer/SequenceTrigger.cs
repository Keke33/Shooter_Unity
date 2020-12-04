using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sequencer {

    [ExecuteAlways]
    public class SequenceTrigger : MonoBehaviour {

        public float radius = 0.5f;
        public float offset = 0;

        public Sequencer sequencer;
        protected Sequence sequence;
        public Vector3 sequencerPosition;
        protected Vector3 sequencerPositionOld;

        public bool showLabel = true;
        public bool shortLabel = false;

        public OptionColor customColor = new OptionColor();

        protected virtual string GetName() => gameObject.name;
        protected virtual string GetHandleLabel() => GetName();

        public bool IsValid() => sequence != null && sequencer != null;
        
        void Init() {

            gameObject.name = GetName();

            sequencer = GetComponentInParent<Sequencer>();
            sequence = GetComponentInParent<Sequence>();

            if (IsValid() == false) {
                return;
            }

            sequencerPositionOld = transform.localPosition + sequence.transform.localPosition;
            
            // clean at runtime
            if (Application.isPlaying) {
                while (transform.childCount > 0) {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
            }
        }

        void Start() {
            Init();
        }

        void Update() {

#if UNITY_EDITOR
            if (Application.isPlaying == false) {
                Init();
            }
#endif
            
            sequencerPosition = sequencer?.transform.InverseTransformPoint(transform.position + transform.right * offset) ?? Vector3.zero;

            float max = sequencer?.triggerSize / 2 ?? 0f;
            if (sequencerPosition.y >= -max && sequencerPosition.y <= max) {
                if (sequencerPosition.x <= radius && sequencerPositionOld.x > radius) {
                    if (Application.isPlaying) {
                        Trigger();
                        SendMessage("OnTriggerSequence", this, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }

        void LateUpdate() {
            sequencerPositionOld = sequencerPosition;
        }

        protected virtual void Trigger() {

        }

        public void DrawGizmos() {
#if UNITY_EDITOR

            bool hidePassedSpawner = sequencer?.hidePassedSpawner ?? false;
            var gizmosColor = sequencer?.gizmosColor ?? Color.red;

            if (!hidePassedSpawner || sequencerPosition.x >= 0) {

                Gizmos.color = customColor.active ? customColor.color : gizmosColor;
                
                bool selected = Utils.GetSelected(transform);
                var discPosition = transform.position + transform.right * offset;

                Gizmos.DrawLine(transform.position, discPosition);
                Gizmos.DrawSphere(transform.position, selected ? 0.15f : 0.1f);
                foreach (var (A, B) in Utils.ChordAround(discPosition, radius)) {
                    Gizmos.DrawLine(A, B);
                }
                if (showLabel) {
                    Vector3 cameraPos = Camera.current.WorldToScreenPoint(transform.position);
                    if (cameraPos.z < 10f) {
                        string label = GetHandleLabel();
                        if (shortLabel) {
                            label = Utils.CapitalsOnly(label);
                        }
                        GUIStyle style = new GUIStyle();
                        style.normal.textColor = Gizmos.color;
                        Handles.Label(transform.position, label, style);
                    }
                }

                Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, selected ? 0.1f : 0f);
                Gizmos.DrawMesh(Utils.disc, discPosition, Quaternion.identity, Vector3.one * radius / 0.5f);
            }
#endif
        }

        void OnDrawGizmos() {
            DrawGizmos();
        }
    }
}
