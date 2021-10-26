using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemController : MonoBehaviour
{
    private int _GemCount = 0;
    private int _GemsNeededToWin = 4;
    Gem _DestructableGem;
    [SerializeField] List<Gem> _Gems;
    [SerializeField] List<Image> _GemImages;

    public void CollectGem(int number)
    {
        _GemImages[number].enabled = true;
        _GemCount += 1;

        if (_GemCount == _GemsNeededToWin)
            CompleteGame();
    }

    public void Restart()
    {
        _GemCount = 0;

        foreach (var _gem in _Gems)
        {
            _gem.Restart();
        }

        foreach (var _image in _GemImages)
        {
            _image.enabled = false;
        }

        if (_DestructableGem != null)
        {
            _DestructableGem.DestroySelf();
        }
    }

    public void SetDestructableGem(Gem gem)
    {
        _DestructableGem = gem;
        _DestructableGem.Generate(3);
    }

    public void CompleteGame()
    {
        StartCoroutine(TriggerCompleteGame());
    }

    IEnumerator TriggerCompleteGame()
    {
        FindObjectOfType<Player>().StartCutscene("Got all of my loot. Lets blow this joint");
        yield return new WaitForSecondsRealtime(3f);
        FindObjectOfType<Player>().StartCutscene("Thank you for playing!");
        yield return new WaitForSecondsRealtime(2f);
        FindObjectOfType<GameLogic>().FullRestart();
    }
}