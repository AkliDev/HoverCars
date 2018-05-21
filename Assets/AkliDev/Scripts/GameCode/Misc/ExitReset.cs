using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitReset : MonoBehaviour
{


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSensor"))
            Application.LoadLevel(Application.loadedLevel);
    }
}
