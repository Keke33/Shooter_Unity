using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleIn : MonoBehaviour {

    public float scaleSpeed = 8f;
    Vector3 initialScale = Vector3.one;

    public float Scale {
        get => transform.localScale.x /initialScale.x;
        set => transform.localScale = initialScale * value;
    }

    void Start() {
        initialScale = transform.localScale;
        Scale = 0f;
    }

    void Update() {
        float newScale = Scale + (1f - Scale) * scaleSpeed * Time.deltaTime;
        Scale = Mathf.Clamp01(newScale);

        if (Scale > 0.9999f) {
            Scale = 1f;
            Destroy(this); // auto-destruction du composant (pas du gameobject !)
        }
    }
}
