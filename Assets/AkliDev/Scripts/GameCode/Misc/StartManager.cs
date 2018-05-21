using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{

    [SerializeField] private GameObject _GameManager;

    private void OnDisable()
    {
        _GameManager.SetActive(true);
    }
}
