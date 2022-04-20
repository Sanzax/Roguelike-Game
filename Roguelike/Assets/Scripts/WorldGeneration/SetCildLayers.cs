using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCildLayers : MonoBehaviour
{
    [SerializeField] string layerName;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Ground");
        }
    }
}
