using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour {

    [Header("Editor")]
    public bool drawbounds;
    [Header("Layers")]
    public Transform TileLayer;
    public Transform PropsLayer;
    public Transform InteractiveLayer;
    public Transform EffectLayer;

    void OnDrawGizmosSelected() {

        if (drawbounds) {

            Gizmos.DrawWireCube(transform.position + new Vector3(0.5f, 0.5f, 0.5f), new Vector3(16, 16, 16));

        }

    }

}
