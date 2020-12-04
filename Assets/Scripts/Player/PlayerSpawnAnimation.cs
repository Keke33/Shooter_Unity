using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnAnimation : MonoBehaviour {

    void Update() {

        Vector3 target = Stage.instance.Bottom + Vector3.up * 2f;

        transform.position = Vector3.Lerp(transform.position, target, 2f * Time.deltaTime);
    }
}
