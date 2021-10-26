using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundController : MonoBehaviour
{
    private Player _Player;
    private int _MaximumDistance = 10;
    private GameLogic _GameLogic;
    private AudioSource[] _Sounds;

    private AudioSource _BossDefeat;
    private AudioSource _BossHurt;
    private AudioSource _Dash;
    private AudioSource _Gem;
    private AudioSource _Jump;
    private AudioSource _Land;
    private AudioSource _MenuClose;
    private AudioSource _MenuOpen;
    private AudioSource _MenuSelect;
    private AudioSource _Sand;
    private AudioSource _SandStart;
    private AudioSource _Slide;
    private AudioSource _SliderClick;
    private AudioSource _Snot;
    private AudioSource _Step1;
    private AudioSource _Step2;
    private AudioSource _Ugh1;
    private AudioSource _Ugh2;
    private AudioSource _Ugh3;
    private AudioSource _WallJump;
    private AudioSource _WaterSlip;
    private AudioSource _Whistle;
    private AudioSource _Zap;

    private void Start()
    {
        _Player = FindObjectOfType<Player>();
        _GameLogic = FindObjectOfType<GameLogic>();
        _Sounds = GetComponents<AudioSource>();
        if (_Sounds.Count() > 0)
        {
            _Step1 = _Sounds[0];
            _BossHurt = _Sounds[0];
            _Gem = _Sounds[0];
            _MenuOpen = _Sounds[0];
        }
        if (_Sounds.Count() > 1)
        {
            _BossDefeat = _Sounds[1];
            _Step2 = _Sounds[1];
            _MenuClose = _Sounds[1];

        }
        if (_Sounds.Count() > 2)
        {
            _SandStart = _Sounds[2];
            _Land = _Sounds[2];
            _Snot = _Sounds[2];
            _MenuSelect = _Sounds[2];
        }
        if (_Sounds.Count() > 3)
        {
            _Sand = _Sounds[3];
            _Slide = _Sounds[3];
            _Whistle = _Sounds[3];
        }
        if (_Sounds.Count() > 4)
        {
            _Dash = _Sounds[4];
            
        }
        if (_Sounds.Count() > 5)
        {
            _Jump = _Sounds[5];
        }
        if (_Sounds.Count() > 6)
        {
            _Ugh1 = _Sounds[6];
        }
        if (_Sounds.Count() > 7)
        {
            _Ugh2 = _Sounds[7];
        }
        if (_Sounds.Count() > 8)
        {
            _Ugh3 = _Sounds[8];
        }
        if (_Sounds.Count() > 9)
        {
            _WallJump = _Sounds[9];
        }
        if (_Sounds.Count() > 10)
        {
            _WaterSlip = _Sounds[10];
        }
        if (_Sounds.Count() > 11)
        {
            _Zap = _Sounds[11];
        }
    }

    public void PlayBossHurt()
    {
        PlayAudioSource(_BossHurt);
    }
    public void PlayBossDefeat()
    {
        PlayAudioSource(_BossDefeat);
    }
    public void PlayDash()
    {
        PlayAudioSource(_Dash);
    }
    public void PlayGem()
    {
        PlayAudioSource(_Gem);
    }
    public void PlayJump()
    {
        PlayAudioSource(_Jump);
    }
    public void PlayLand()
    {
        PlayAudioSource(_Land);
    }
    public void PlayMenuClose()
    {
        PlayGlobalAudioSource(_MenuClose);
    }
    public void PlayMenuOpen()
    {
        PlayGlobalAudioSource(_MenuOpen);
    }
    public void PlayMenuSelect()
    {
        PlayGlobalAudioSource(_MenuSelect);
    }
    public void PlaySand()
    {
        PlayAudioSource(_Sand);
    }
    public void StopSand()
    {
        StopAudioSource(_Sand);
    }
    public void PlaySandStart()
    {
        PlayAudioSource(_SandStart);
    }
    public void PlaySlide()
    {
        PlayAudioSource(_Slide);
    }
    public void PlaySliderClick()
    {
        PlayAudioSource(_SliderClick);
    }
    public void PlaySnot()
    {
        PlayAudioSource(_Snot);
    }

    public void PlayStep1()
    {
        PlayAudioSource(_Step1);
    }

    public bool IsStep1Playing()
    {
        return  _Step1.isPlaying;
    }

    public void PlayStep2()
    {
        PlayAudioSource(_Step2);
    }

    public bool IsStep2Playing()
    {
        return _Step2.isPlaying;
    }
    public void PlayUgh1()
    {
        PlayAudioSource(_Ugh1);
    }
    public void PlayUgh2()
    {
        PlayAudioSource(_Ugh2);
    }
    public void PlayUgh3()
    {
        PlayAudioSource(_Ugh3);
    }
    public void PlayWallJump()
    {
        PlayAudioSource(_WallJump);
    }
    public void PlayWaterSlip()
    {
        PlayAudioSource(_WaterSlip);
    }
    public void StopWaterSlip()
    {
        StopAudioSource(_WaterSlip);
    }
    public void PlayWhistle()
    {
        PlayAudioSource(_Whistle);
    }
    public void PlayZap()
    {
        PlayAudioSource(_Zap);
    }

    public void PlayAudioSource(AudioSource source)
    {
        float _distance = Vector3.Distance(transform.position, _Player.transform.position);
        
        var _volume = _distance > _MaximumDistance ? 0 : _distance == 0 ? _GameLogic.SoundVolume : _GameLogic.SoundVolume - (_GameLogic.SoundVolume * _distance / _MaximumDistance);
        source.volume = _volume;
        source.Play();
    }

    public void PlayGlobalAudioSource(AudioSource source)
    {
        source.volume = _GameLogic.SoundVolume;
        source.Play();
    }

    public void StopAudioSource(AudioSource source)
    {
        source.Stop();
    }
}