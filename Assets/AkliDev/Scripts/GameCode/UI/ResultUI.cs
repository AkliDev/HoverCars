
using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _LapTimeLabels;
    [SerializeField] private TextMeshProUGUI _FinalTimeLabel;

    public void SetLapTimeLabels(TextMeshProUGUI[] labels)
    {
        for (int i = 0; i < _LapTimeLabels.Length; i++)
        {
            _LapTimeLabels[i].text = "Lap " + (i + 1) + "  " + labels[i].text;
        }
    }

    public void SetFinalTimeLabel(TextMeshProUGUI labels)
    {
        _FinalTimeLabel.text = "Total Time " + labels.text;
    }
}
