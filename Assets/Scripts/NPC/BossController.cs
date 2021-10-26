using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    bool _IsDisabled = false;
    Vector3 _BossStartingLocation;
    [SerializeField] Sylvester _Sylvester;
    [SerializeField] Sylvester _BossPrefab;
    [SerializeField] BossHealthUI _BossHealthUI;

    public void Awake()
    {
        _BossStartingLocation = _Sylvester.transform.position;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (_IsDisabled)
            return;

        if (_Sylvester == null)
        {
            _IsDisabled = true;
            return;
        }

        if (collision.tag == "Player")
        {
            _BossHealthUI.gameObject.SetActive(true);
            _Sylvester.Resume();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (_IsDisabled)
            return;

        if (_Sylvester == null)
        {
            _IsDisabled = true;
            Destroy(gameObject);

            return;
        }

        if (collision.tag == "Player")
        {
            _BossHealthUI.gameObject.SetActive(false);
            _Sylvester.Pause();
        }
    }

    public void Restart()
    {
        _IsDisabled = false;
        _BossHealthUI.gameObject.SetActive(true);
        _BossHealthUI.Restart();
        _BossHealthUI.gameObject.SetActive(false);
        if (_Sylvester == null)
        {
            _Sylvester = Instantiate(_BossPrefab);
            _Sylvester.transform.position = _BossStartingLocation;
        } else
        {
            _Sylvester.Restart();
        }
    }
}