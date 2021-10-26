using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] Sprite _FullHeart;
    [SerializeField] Sprite _EmptyHeart;

    [SerializeField] private List<Image> _HealthBars;
    
    public void LoseHealth(int health)
    {
        if (health < 0)
            return;

        if(health < _HealthBars.Count)
        {
            _HealthBars[health].sprite = _EmptyHeart;
        }
    }

    public void Restart()
    {
        foreach(var _heath in _HealthBars)
        {
            _heath.sprite = _FullHeart;
        }
    }
}