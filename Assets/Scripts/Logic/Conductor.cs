using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Conductor : MonoBehaviour
{
    private bool _IsStarted;

    [SerializeField] private AudioSource _Music;

    public static Conductor Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Update()
    {

    }

    public void EnableMusicElement()
    {
        _Music.gameObject.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        _Music.volume = volume;
    }


    public void StartSong()
    {
        _IsStarted = true;
        _Music.loop = true;
        _Music.Play();
    }

    public void SetSong(AudioClip song)
    {
        _Music.loop = true;
        _Music.Stop();
        _Music.clip = song;
    }

    public void Resume()
    {
        _Music.UnPause();
        _IsStarted = true;
    }

    public void Stop()
    {
        _Music.Stop();
        _IsStarted = false;
    }

    public void Restart()
    {
        Stop();
        StartSong();
    }

    public void EndSong()
    {
        _Music.Stop();
        _IsStarted = false;
    }

    public void Pause()
    {
        if (_IsStarted)
        {
            _Music.Pause();
            _IsStarted = false;
        }
    }

    public float GetVolume()
    {
        return _Music.volume;
    }
}
