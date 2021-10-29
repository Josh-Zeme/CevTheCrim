using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    private GameLogic _GameLogic;
    [SerializeField] private TextMeshProUGUI[] _MenuItems;
    [SerializeField] private TextMeshProUGUI _RunTime;
    [SerializeField] private TextMeshProUGUI _BestTime;
    [SerializeField] private Color _SelectedColour;
    [SerializeField] private Color _UnselectedColour;
    [SerializeField] private Slider _MusicVolumeSlider;
    [SerializeField] private Slider _SoundVolumeSlider;
    private SoundController _SoundController;
    private int _SelectedMainItem = 0;
    private int _ResumeGameItemId = 0;
    private int _RestartGameItemId = 1;
    private int _MusicVolumeItemId = 2;
    private int _SoundVolumeItemId = 3;
    private int _QuitItemId = 4;


    public void Start()
    {
        _SoundController = GetComponent<SoundController>();
        _GameLogic = FindObjectOfType<GameLogic>();
    }

    public void DownArrow()
    {
        _MenuItems[_SelectedMainItem].color = _UnselectedColour;
        _SelectedMainItem++;
        _SelectedMainItem = _SelectedMainItem <= _MenuItems.Length - 1 ? _SelectedMainItem : 0;
        _MenuItems[_SelectedMainItem].color = _SelectedColour;
    }

    public void UpArrow()
    {
        _MenuItems[_SelectedMainItem].color = _UnselectedColour;
        _SelectedMainItem--;
        _SelectedMainItem = _SelectedMainItem >= 0 ? _SelectedMainItem : _MenuItems.Length - 1;
        _MenuItems[_SelectedMainItem].color = _SelectedColour;
    }

    public void RightArrow()
    {
        if (_SelectedMainItem == _MusicVolumeItemId)
        {
            _MusicVolumeSlider.value += .10f;
            _GameLogic.SetMusicVolume(_MusicVolumeSlider.value);
        }

        if (_SelectedMainItem == _SoundVolumeItemId)
        {
            _SoundVolumeSlider.value += .10f;
            _SoundController.PlayMenuSelect();
            _GameLogic.SetSoundVolume(_SoundVolumeSlider.value);
        }
    }

    public void LeftArrow()
    {
        if (_SelectedMainItem == _MusicVolumeItemId)
        {
            _MusicVolumeSlider.value -= .10f;

            _GameLogic.SetMusicVolume(_MusicVolumeSlider.value);
        }

        if (_SelectedMainItem == _SoundVolumeItemId)
        {
            _SoundVolumeSlider.value -= .10f;
            _SoundController.PlayMenuSelect();
            _GameLogic.SetSoundVolume(_SoundVolumeSlider.value);
        }
    }

    public void Action()
    {
        if (_SelectedMainItem == _ResumeGameItemId)
        {
            ResumeLevel();
        }

        if (_SelectedMainItem == _RestartGameItemId)
        {
            RestartGame();
        }

        if (_SelectedMainItem == _QuitItemId)
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        _GameLogic.RestartLevel();
        _GameLogic.ResumeLevel();
    }

    public void ResumeLevel()
    {
        _GameLogic.ResumeLevel();
    }

    public void SetSoundVolumeSlider(float value)
    {
        _SoundVolumeSlider.value = value;
    }

    public void SetMusicVolumeSlider(float value)
    {
        _MusicVolumeSlider.value = value;
    }

    public void SetRunTimer(string time)
    {
        _RunTime.text = GameSettings.RunTime + time;
    }

    public void SetBestTimer(string time)
    {
        if (time != null)
            _BestTime.text = GameSettings.BestTime + time;
        else
            _BestTime.text = GameSettings.BestTime + "N/A";
    }
}
