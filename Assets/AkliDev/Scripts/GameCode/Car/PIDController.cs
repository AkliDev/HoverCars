using UnityEngine;

[System.Serializable]
public class PIDController
{
   
    public float _P;
    public float _I;
    public float _D;
    public float _Minimum;
    public float _Maximum;

    private float _Iintegral;
    private float _LastProportional;

  
    public float Seek(float seekValue, float currentValue)
    {
        float deltaTime = Time.fixedDeltaTime;
        float proportional = seekValue - currentValue;

        float derivative = (proportional - _LastProportional) / deltaTime;
        _Iintegral += proportional * deltaTime;
        _LastProportional = proportional;

        float value = _P * proportional + _I * _Iintegral + _D* derivative;
        value = Mathf.Clamp(value, _Minimum, _Maximum);

        return value;
    }

}
