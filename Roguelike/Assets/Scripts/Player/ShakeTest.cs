using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ShakeTest : MonoBehaviour
{
    [SerializeField] float magnitude, roughness, fadeIn, fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeIn, fadeOut);
        }

    }
}
