using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _Player;
    private GameLogic _GameLogic;
    private InputController _InputController;
    private bool _IsMoving = false;
    private bool _IsCutscene = false;
    private static Vector3 _StartingLocation;

    void Awake()
    {
        _StartingLocation = transform.position;
        _Player = GetComponent<Player>();
        _GameLogic = FindObjectOfType<GameLogic>();
        _InputController = GameSettings.InputController;
        _InputController.Player.Movement.performed += ctx => MovePressed(ctx.ReadValue<float>());
        _InputController.Player.Jump.performed += ctx => Jump();
        _InputController.Player.Duck.performed += ctx => Duck();
        _InputController.Player.Movement.canceled += ctx => MoveReleased();
    }

    private void Jump()
    {
        if (_GameLogic.IsLevelMenuActive || _GameLogic.IsStartMenuActive || _IsCutscene)
            return;

        _Player.Jump();
    }

    private void Duck()
    {
        if (_GameLogic.IsLevelMenuActive || _GameLogic.IsStartMenuActive || _IsCutscene)
            return;

        _Player.Duck();
    }

    private void MovePressed(float direction)
    {
        if (_GameLogic.IsLevelMenuActive || _GameLogic.IsStartMenuActive || _IsCutscene)
            return;
        _Player.Move(direction);
    }

    private void MoveReleased()
    {
        if (_GameLogic.IsLevelMenuActive || _GameLogic.IsStartMenuActive || _IsCutscene)
            return;

        _Player.StopMove();
    }

    public void Restart()
    {
        transform.position = _StartingLocation;
    }

    public void StartCutscene()
    {
        _IsCutscene = true;
    }

    public void EndCutscene()
    {
        _IsCutscene = false;
    }
}
