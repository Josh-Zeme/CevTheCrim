using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameLogic : MonoBehaviour
{
    private static InputController _InputController;

    [SerializeField] private TextMeshProUGUI _RunTime;
    [SerializeField] private Player _Player;
    [SerializeField] private GemController _GemController;
    [SerializeField] private BossController _BossController;
    [SerializeField] private Conductor _Conductor;
    [SerializeField] private PauseMenu _PauseMenu;
    [SerializeField] private StartMenu _StartMenu;
    [SerializeField] private BossHealthUI _BossHealthUI;
    [SerializeField] private GameData _GameData;
  
    [SerializeField] private AudioClip _MenuSong;
    [SerializeField] private AudioClip _StartLevelSong;
    [SerializeField] private GameObject _TheGems;
    [SerializeField] private GameObject _MusicBoxes;

    [SerializeField] private bool _IsForceStart = false;
    private bool _IsStarted = false;
    private float _MusicVolume = 0.25f;
    [NonSerialized] public float SoundVolume = 0.15f;
    public bool IsLevelMenuActive = false;
    public bool IsStartMenuActive = true;
    public float RunTime;
    public float BestTime;

    private void Awake()
    {
        int _numberOfSession = FindObjectsOfType<GameLogic>().Length;
        if (_numberOfSession > 1)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        BestTime = _GameData.Load();
        var _bestTime = BestTime <= 0 ? null : TimerToString(BestTime);
        _StartMenu.SetBestTimer(_bestTime);
        _StartMenu.gameObject.SetActive(IsStartMenuActive);
        _InputController = GameSettings.InputController;
        _InputController.Menus.Action.performed += _ => Action();
        _InputController.Menus.Escape.performed += _ => Escape();
        _InputController.Menus.Movement.performed += ctx => Movement(ctx.ReadValue<Vector2>());

        _Conductor.EnableMusicElement();
    }

    // Update is called once per frame
    void Update()
    {
        if (_IsForceStart && !_IsStarted)
        {
            _MusicVolume = 0.1f;
            _Conductor.SetVolume(_MusicVolume);
            IsStartMenuActive = false;
            _Player.Restart();
            _StartMenu.gameObject.SetActive(IsStartMenuActive);
            _Conductor.SetSong(_MenuSong);
            _Conductor.StartSong();
            _IsStarted = true;
        }

        if (_IsStarted && !IsLevelMenuActive)
        {
            RunTime += Time.deltaTime;
            _RunTime.text = TimerToString(RunTime);
        }
    }

    private void OnEnable()
    {
        _InputController.Enable();
    }

    //private void OnDisable()
    //{
    //    _InputController.Disable();
    //}

    public void Action()
    {
        if (IsStartMenuActive)
        {
            _StartMenu.Action();

        } else if (IsLevelMenuActive)
        {
            _PauseMenu.Action();
        }
    }

    public void Escape()
    {
        if (IsLevelMenuActive)
        {
            ResumeLevel();
        } else 
        if (!IsStartMenuActive)
        {
            PauseLevel();
        }
    }

    public void Movement(Vector2 direction)
    {
        if (!IsStartMenuActive && !IsLevelMenuActive)
            return;

        if (direction.y > 0)
        {
            direction.y = 1;
            direction.x = 0;
            if(IsStartMenuActive)
                _StartMenu.UpArrow();
            if (IsLevelMenuActive)
                _PauseMenu.UpArrow();
        }

        if (direction.y < 0)
        {
            direction.y = -1;
            direction.x = 0;
            if (IsStartMenuActive)
                _StartMenu.DownArrow();
            if (IsLevelMenuActive)
                _PauseMenu.DownArrow();
        }

        if(direction.x < 0)
        {
            if (IsStartMenuActive)
                _StartMenu.LeftArrow();
            if (IsLevelMenuActive)
                _PauseMenu.LeftArrow();
        }

        if (direction.x > 0)
        {
            if (IsStartMenuActive)
                _StartMenu.RightArrow();
            if (IsLevelMenuActive)
                _PauseMenu.RightArrow();
        }
    }

    public BossHealthUI GetBossUI()
    {
        return _BossHealthUI;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        _Conductor.SetSong(_StartLevelSong);
        _Conductor.SetVolume(_MusicVolume);
        IsStartMenuActive = false;
        _StartMenu.gameObject.SetActive(IsStartMenuActive);
        _MusicBoxes.SetActive(true);
        _TheGems.SetActive(true);
        _RunTime.gameObject.SetActive(true);
        _Player.StartCutscene("Time to steal some stuff");
        _IsStarted = true;
    }

    public void InitializeStartMenu()
    {
        Time.timeScale = 0;
        _MusicVolume = 0.1f;
        _Conductor.SetVolume(_MusicVolume);
        _Conductor.SetSong(_MenuSong);
        _Conductor.StartSong();
    }

    public void StartNewSong(AudioClip song)
    {
        _Conductor.EndSong();
        _Conductor.SetSong(song);
        _Conductor.StartSong();
    }

    public void SetMusicVolume(float volume)
    {
        _MusicVolume = volume;
        _Conductor.SetVolume(_MusicVolume);
    }

    public void SetSoundVolume(float volume)
    {
        SoundVolume = volume;
    }

    public void PauseLevel()
    {
        Time.timeScale = 0;
        IsLevelMenuActive = true;
        _TheGems.SetActive(false);
        _RunTime.gameObject.SetActive(false);
        var _bestTime = BestTime <= 0 ? null : TimerToString(BestTime);
        _PauseMenu.SetBestTimer(_bestTime);
        _PauseMenu.gameObject.SetActive(IsLevelMenuActive);
        _PauseMenu.SetRunTimer(TimerToString(RunTime));
        _PauseMenu.SetSoundVolumeSlider(SoundVolume);
        _PauseMenu.SetMusicVolumeSlider(_MusicVolume);
    }

    public void ResumeLevel()
    {
        Time.timeScale = 1;
        IsLevelMenuActive = false;
        _TheGems.SetActive(true);
        _RunTime.gameObject.SetActive(true);
        _PauseMenu.gameObject.SetActive(IsLevelMenuActive);
    }

    public void RestartLevel()
    {
        _Player.Restart();
        _GemController.Restart();
        _BossController.Restart();
        RunTime = 0;
    }

    public void FullRestart()
    {
        if (BestTime <= 0 || RunTime < BestTime)
        {
            _GameData.Save();
            BestTime = RunTime;
             var _bestTime = BestTime <= 0 ? null : TimerToString(BestTime);
            _StartMenu.SetBestTimer(_bestTime);
        }
        RestartLevel();
        _IsStarted = false;
        Time.timeScale = 0;
        IsStartMenuActive = true;
        _StartMenu.gameObject.SetActive(IsStartMenuActive);
        _MusicBoxes.SetActive(false);
        _TheGems.SetActive(false);
        _RunTime.gameObject.SetActive(false);
        InitializeStartMenu();
        RunTime = 0;
    }

    private string TimerToString(float time)
    {
        string _hours = ((int)time / 3600).ToString("00");
        float _hoursFloat = time % 3600;
        string _minutes = ((int)_hoursFloat / 60).ToString("00");
        string _seconds = (_hoursFloat % 60).ToString("00");
        return $"{_hours}:{_minutes}:{_seconds}";
    }
}
