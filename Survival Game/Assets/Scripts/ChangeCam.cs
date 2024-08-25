using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCam : MonoBehaviour
{
    public Camera mainCam;
    public Camera craftingCam;
    void Start()
    {
        mainCam.enabled = true;
        craftingCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mainCam.enabled = !mainCam.enabled;
            craftingCam.enabled = !craftingCam.enabled;
        }
        
    }
}
