using UnityEngine;
using TMPro;
using System.Collections.Generic;

public static class GameSettings
{
    public static GameLogic GameLogic = Object.FindObjectOfType<GameLogic>();
    public static InputController InputController = new InputController();
    public static string BestTime = "Best Time: ";
    public static string RunTime = "Run Time: ";
}
