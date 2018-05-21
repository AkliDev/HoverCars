using UnityEngine;
using TMPro;

public class LapTimeUI : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI[] _LapTimeLabels;
    [SerializeField] private TextMeshProUGUI _FinalTimeLabel;

    public TextMeshProUGUI[] LapTimeLabels { get { return _LapTimeLabels; } }
    public TextMeshProUGUI FinalTimeLabel { get { return _FinalTimeLabel; } }

    void Awake()
    {
        for (int i = 0; i < _LapTimeLabels.Length; i++)
        {
            _LapTimeLabels[i].text = "";
        }

        _FinalTimeLabel.text = "";
    }

    public void SetLapTime(int lapNumber, float lapTime)
    {
        if (lapNumber >= _LapTimeLabels.Length)
        {
            return;
        }

        _LapTimeLabels[lapNumber].text = ConvertTimeToString(lapTime);
    }

    public void SetFinalTime(float lapTime)
    {
        _FinalTimeLabel.text = ConvertTimeToString(lapTime);
    }

    public string ConvertTimeToString(float time)
    {
        int minutes = (int)(time / 60);
        float seconds = time % 60f;

        string output = minutes.ToString("00") + "." + seconds.ToString("00.000");
        return output;
    }
}
