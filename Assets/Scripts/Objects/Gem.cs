using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gem : MonoBehaviour
{
    [SerializeField] Image _UIItem;
    SoundController _SoundController;
    GemController _GemController;
    SpriteRenderer _SpriteRenderer;
    BoxCollider2D _BoxCollider2D;
    [SerializeField] int _GemNumber;

    public void Start()
    {
        _GemController = FindObjectOfType<GemController>();
        _SoundController = GetComponent<SoundController>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Generate(int number)
    {
        _GemNumber = number;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _SoundController.PlayGem();

            if(_GemController == null)
            {
                _GemController = FindObjectOfType<GemController>();
            }

            _GemController.CollectGem(_GemNumber);
            _SpriteRenderer.enabled = false;
            _BoxCollider2D.enabled = false;
            if (_GemNumber == 3)
                DestroySelf();
        }
    }

    public void Restart()
    {
        _SpriteRenderer.enabled = true;
        _BoxCollider2D.enabled = true;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
