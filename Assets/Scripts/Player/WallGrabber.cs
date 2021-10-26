using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrabber : MonoBehaviour
{
    [SerializeField] Player _Player;
    [SerializeField] BoxCollider2D _WallGrabber;

    public void Update()
    {
        var _isTouchingGround = _WallGrabber.IsTouchingLayers(LayerMask.GetMask("Ground"));
        _Player.UpdateWallJump(_isTouchingGround);
    }
}