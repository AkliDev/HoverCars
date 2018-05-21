using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class ShortCuts : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)|| XCI.GetButtonDown(XboxButton.Start))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
