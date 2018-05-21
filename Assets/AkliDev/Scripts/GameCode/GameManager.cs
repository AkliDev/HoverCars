using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Settings")]
    [SerializeField] private int _NumberOfLaps = 3;
    [SerializeField] private CarManager _CarManager;
    [SerializeField] private CarManager _GhostCarManager;

    [Header("UI References")]
    [SerializeField] private PlayerUI _PlayerUI;
    [SerializeField] private LapTimeUI _LapTimeUI;
    [SerializeField] private GameObject _FinishUI;
    [SerializeField] private ResultUI _ResultsUI;

    [Header("Other References")]
    [SerializeField] private AudioSource _GlobalMusic;
    [SerializeField] private AudioHolder _GlobalSounds;


    float[] _LapTimes;
    int _CurrentLap = 0;
    bool _IsGameOver;
    bool _RaceHasBegun;

     void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        UpdateUI_LapNumber();

        yield return new WaitForSeconds(.1f);

        _CarManager.Behaviour.enabled = true;
        _GhostCarManager.Behaviour.enabled = true;
        _LapTimes = new float[_NumberOfLaps];
        _RaceHasBegun = true;
    }

    void Update()
    {
        UpdateUI_Speed();

        if (IsActiveGame())
        {
            _LapTimes[_CurrentLap] += Time.deltaTime;
            UpdateUI_LapTime();
        }

        if (_IsGameOver)
        {
            if (!_GlobalMusic.isPlaying)
            {
                _GlobalMusic.clip = _GlobalSounds._Sounds[1];
                _GlobalMusic.loop = true;
                _GlobalMusic.Play();
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || XCI.GetButtonDown(XboxButton.Start) || XCI.GetButtonDown(XboxButton.A))
            {
                _ResultsUI.SetLapTimeLabels(_LapTimeUI.LapTimeLabels);
                _ResultsUI.SetFinalTimeLabel(_LapTimeUI.FinalTimeLabel);

                _PlayerUI.gameObject.SetActive(false);
                _LapTimeUI.gameObject.SetActive(false);
                _FinishUI.gameObject.SetActive(false);          
                _ResultsUI.gameObject.SetActive(true);
            }
        }
    }

    public void PlayerCompletedLap()
    {
        if (_IsGameOver)
        {
            return;
        }
            

        _CurrentLap++;

        UpdateUI_LapNumber();

        if (_CurrentLap >= _NumberOfLaps)
        {
            _IsGameOver = true;

            UpdateUI_FinalTime();

            Change_Car_State();

            _FinishUI.GetComponent<Animator>().enabled = true;
            _FinishUI.GetComponent<Image>().enabled = true;

            _GlobalMusic.clip = _GlobalSounds._Sounds[0];
            _GlobalMusic.loop = false;
            _GlobalMusic.Play();
        }
    }

    void UpdateUI_LapTime()
    {
        if (_LapTimeUI != null)
        {
            _LapTimeUI.SetLapTime(_CurrentLap, _LapTimes[_CurrentLap]);
        }
    }

    void UpdateUI_FinalTime()
    {
        if (_LapTimeUI != null)
        {
            float total = 0f;

            for (int i = 0; i < _LapTimes.Length; i++)
            {
                total += _LapTimes[i];
            }

            _LapTimeUI.SetFinalTime(total);
        }
    }
    void Change_Car_State()
    {
        _CarManager.Behaviour.SwitchState(new Auto(_CarManager.Behaviour));
    }
        void UpdateUI_LapNumber()
    {
        
        if (_PlayerUI != null)
        {
            _PlayerUI.SetLapDisplay(_CurrentLap + 1, _NumberOfLaps);
        }
    }

    void UpdateUI_Speed()
    {
        if (_CarManager != null && _PlayerUI != null)
        {
            _PlayerUI.SetSpeedDisplay(Mathf.Abs(_CarManager.Behaviour.LocalVelocity.z));
        }            
    }

    public bool IsActiveGame()
    {
        return _RaceHasBegun && !_IsGameOver;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
