using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    [SerializeField] private AudioClip _Song;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameSettings.GameLogic.StartNewSong(_Song);
    }
}