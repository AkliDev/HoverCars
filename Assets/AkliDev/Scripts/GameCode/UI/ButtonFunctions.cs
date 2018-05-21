using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public void Reset()
    {
        GameManager.instance.Restart();
    }

    public void Quit()
    {
        GameManager.instance.Quit();
    }
}
