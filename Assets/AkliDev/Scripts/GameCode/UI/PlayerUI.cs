
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI _CurrentSpeedText;
    [SerializeField] private TextMeshProUGUI _CurrentLapText;

    void Awake()
    {
        _CurrentSpeedText.text = "";
        _CurrentLapText.text = "";
    }

    public void SetLapDisplay(int currentLap, int numberOfLaps)
    {
        if (currentLap > numberOfLaps)
        {
            return;
        }

        _CurrentLapText.text = "Lap " + currentLap + " I " + numberOfLaps;
    }

    public void SetSpeedDisplay(float currentSpeed)
    {
        int speed = (int)currentSpeed;
        _CurrentSpeedText.text = speed.ToString() + " KM H";
    }
}
