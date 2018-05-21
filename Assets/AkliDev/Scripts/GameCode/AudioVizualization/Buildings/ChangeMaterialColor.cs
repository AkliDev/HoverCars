using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{
    [SerializeField] private Color _Color;
    private Material _Mat;

    public Color Color { get { return _Color; } }

    void Awake()
    {
        _Mat = GetComponent<Renderer>().material;
        _Mat.color = _Color;
        _Mat.SetColor("_EmissionColor", _Color);
    }
}
