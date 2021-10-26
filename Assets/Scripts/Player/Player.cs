using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TextMeshPro _Text;
    [SerializeField] private ParticleSystem _Dust;
    [SerializeField] float _RunSpeed = 8.0f;
    [SerializeField] float _DashSpeed = 20.0f;
    float _WallJumpSpeed = 10.0f;
    [SerializeField] float _JumpSpeed = 15.0f;
    [SerializeField] BoxCollider2D _FeetCollider2D;
    [SerializeField] BoxCollider2D _BodyCollider2D;
    private PlayerController _PlayerController;
    private SoundController _SoundController;
    private float _DeathDelay = 1f;
    private bool _IsCutScene = false;
    private bool _IsMoveAllowed = true;
    private bool _IsDashComplete = false;
    private bool _IsWallJumpComplete = false;
    private bool _IsTouchingGround = false;
    private bool _IsTouchingReverter = false;
    private bool _IsReverterActivated = false;
    private bool _IsWallJumpAllowed = false;
    private bool _IsWallJumpDeactivator = false;
    private bool _IsAlive = true;
    private bool _IsTouchingSlippy = false;
    private bool _IsStartedSlippy = false;
    private bool _IsMovedAfterJump = false;
    private Animator _Animator;
    private Rigidbody2D _Rigidbody2D;
    private int _GravityScale = 1;
    private int _DirectionScale = 1;
    private float _Direction = 1;
    private float _PreviousDirection = 1;

    private float _TimeCutsceneStart = 2f;
    private float _TimeCutsceneRemaining = 0;

    private float _TimeReverseStart = 0.15f;
    private float _TimeReverseStartRemaining = 0;
    private float _TimeReverseEndStart = 0.15f;
    private float _TimeReverseEndRemaining = 0;

    private float _TimeJumpPressedStart = 0.2f;
    private float _TimeJumpPressedRemaining = 0;
    private float _TimeCoyoteJumpStart = 0.1f;
    private float _TimeCoyoteJumpRemaining = 0f;
    private float _TimeWallJumpDeactivatorStart = 0.1f;
    private float _TimeWallJumpDeactivatorRemaining = 0f;
    private float _TimeDashStart = 0.25f;
    private float _TimeDuckStart = 0.20f;
    private float _TimeDashRemaining = 0f;
    private float _TimeWallJumpVelocityStart = 0.3f;
    private float _TimeWallJumpVelocityRemaining = 0f;
    private float _TimeMoveAfterWallJumpStart = 0.1f;
    private float _TimeMoveAfterWallJumpRemaining = 0f;

    private float _RunVelocity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _SoundController = GetComponent<SoundController>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _PlayerController = GetComponent<PlayerController>();
        _Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _IsTouchingGround = _FeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if(_IsTouchingSlippy && !_FeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Slippy")))
        {
            _IsStartedSlippy = false;
            _SoundController.StopWaterSlip();
        }
        _IsTouchingSlippy = _FeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Slippy"));
        _IsTouchingReverter = _BodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Reverter"));

        if (_IsTouchingGround)
        {
            if (_Animator.GetBool("IsJumping"))
            {
                _SoundController.PlayLand();
            }
            UpdateAnimationState("IsJumping", false);
            _TimeCoyoteJumpRemaining = _TimeCoyoteJumpStart;
            _IsDashComplete = false;
            _IsWallJumpComplete = false;
            var _isSoundPlaying = _SoundController.IsStep1Playing() || _SoundController.IsStep2Playing();
            if (Mathf.Abs(_Rigidbody2D.velocity.x) > 0.1 && _TimeDashRemaining < 0 && !_isSoundPlaying)
            {
                CreateDust();
                var _sound = Random.Range(0, 2);
                switch (_sound)
                {
                    default:
                    case 0:
                        _SoundController.PlayStep1();
                        break;
                    case 1:
                        _SoundController.PlayStep2();
                        break;
                }
            }
        }

        Die();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_IsAlive)
            return;

        if(_TimeCutsceneRemaining < 0 && _IsCutScene)
        {
            EndCutscene();
            return;
        }

        if (_IsCutScene)
        {
            _TimeCutsceneRemaining -= Time.deltaTime;
            var _scale = transform.localScale;
            _scale.x = -1;
            transform.localScale = _scale;
            _Rigidbody2D.velocity = new Vector2(0, 0);
            return;
        }

        if (_IsTouchingReverter && !_IsReverterActivated && _TimeReverseStartRemaining < 0)
        {
            _TimeReverseStartRemaining = _TimeReverseStart;
        }
        else if(!_IsTouchingReverter && _IsReverterActivated && _TimeReverseEndRemaining < 0)
        {
            _TimeReverseEndRemaining = _TimeReverseEndStart;
        }

        if (_IsTouchingSlippy)
        {
            if(!_IsStartedSlippy)
                _SoundController.PlayWaterSlip();
            _IsStartedSlippy = true;
            _RunVelocity = transform.localScale.x * -1;
        }

        var _tempRunVelocity = _TimeMoveAfterWallJumpRemaining > 0 ? 0 : _RunVelocity * _DirectionScale;
        // reverse jumps are fucking stupid and they go the wrong way?
        var _jumpVelocity = _TimeWallJumpVelocityRemaining > 0 ? Mathf.Lerp(0, _WallJumpSpeed * _Direction, _TimeWallJumpVelocityRemaining / _TimeWallJumpVelocityStart) : 0;
        var _dashVelocity = _TimeDashRemaining > 0 ?  Mathf.Lerp(0, _DashSpeed * transform.localScale.x * -1, _TimeDashRemaining / _TimeDashStart) : 0;
        var _horizontalVelocity = Mathf.Abs(_jumpVelocity) > Mathf.Abs((_tempRunVelocity * _RunSpeed)) ? _jumpVelocity : ((_tempRunVelocity * _RunSpeed) + _dashVelocity);
        
        
        
        var _velocity = new Vector2(_horizontalVelocity, _Rigidbody2D.velocity.y);

        _Rigidbody2D.velocity = _velocity;
        var _direction = Mathf.Abs(_Rigidbody2D.velocity.x) < 1 ? _PreviousDirection : _Rigidbody2D.velocity.x;
        FlipSprite(_direction);

        _TimeJumpPressedRemaining -= Time.deltaTime;
        _TimeCoyoteJumpRemaining -= Time.deltaTime;
        _TimeDashRemaining -= Time.deltaTime;
        _TimeWallJumpDeactivatorRemaining -= Time.deltaTime;
        _TimeWallJumpVelocityRemaining -= Time.deltaTime;
        _TimeReverseStartRemaining -= Time.deltaTime;
        _TimeReverseEndRemaining -= Time.deltaTime;
        _TimeMoveAfterWallJumpRemaining -= Time.deltaTime;

        if (_TimeReverseStartRemaining < 0 && !_IsReverterActivated && _IsTouchingReverter)
        {
            _IsReverterActivated = true;
            _DirectionScale = -1;
        }

        if (_TimeReverseEndRemaining < 0 && _IsReverterActivated && !_IsTouchingReverter)
        {
            _IsReverterActivated = false;
            _DirectionScale = 1;
        }

        if (_TimeWallJumpVelocityRemaining < 0 && _IsWallJumpComplete)
        {
            _IsMovedAfterJump = false;
            _IsWallJumpComplete = false;
        }

        if(_TimeWallJumpDeactivatorRemaining < 0 && _IsWallJumpDeactivator)
        {
            _IsWallJumpDeactivator = false;
            _IsWallJumpAllowed = false;           
        }

        UpdateAnimationState("IsGrabbing", _IsWallJumpAllowed && !_IsTouchingGround);
    }

    #region Movement Region

    public void Move(float direction)
    {
        if (!_IsMoveAllowed || _IsTouchingSlippy)
            return;

        if (_TimeWallJumpDeactivatorRemaining > 0)
            _IsMovedAfterJump = true;

        _RunVelocity = Mathf.Abs(direction) == 1 ? direction : 0;
        _Direction = _RunVelocity;
        if (_Direction != 0)
            _PreviousDirection = _RunVelocity;
    }

    public void UpdateWallJump(bool isWallJumpAllowed)
    {
        if (!isWallJumpAllowed && _IsWallJumpAllowed)
        {
            UpdateAnimationState("IsGrabbing", false);
            _IsWallJumpDeactivator = true;
            _TimeWallJumpDeactivatorRemaining = _TimeWallJumpDeactivatorStart;
        }

        _IsWallJumpAllowed = isWallJumpAllowed;
    }

    public void StopMove()
    {
        _RunVelocity = 0f;
    }

    public void Jump()
    {
        if (!_IsAlive)
            return;
        var _isTouchingGround = _FeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (_IsWallJumpAllowed && !_isTouchingGround && !_IsWallJumpComplete)
        {
            _SoundController.PlayWallJump();
            Vector2 _jumpVelocity = new Vector2(0, (_JumpSpeed * _GravityScale));
            _Rigidbody2D.velocity = _jumpVelocity;
            _IsWallJumpComplete = true;
            _TimeWallJumpVelocityRemaining = _TimeWallJumpVelocityStart;
            _TimeMoveAfterWallJumpRemaining = _TimeMoveAfterWallJumpStart;
            UpdateAnimationState("IsJumping", true);
            _Direction = transform.localScale.x;
            FlipSprite(_Direction);
            CreateDust();
            return;
        }

        var _isLogicalTouchingGround = _TimeCoyoteJumpRemaining > 0;

        _TimeJumpPressedRemaining = _TimeJumpPressedStart;

        if (_TimeJumpPressedRemaining > 0 )
        {
            _TimeJumpPressedRemaining = 0;
            if (!_isLogicalTouchingGround && !_IsDashComplete && _Rigidbody2D.velocity.y != 0 && !_isTouchingGround)
            {
                _SoundController.PlayDash();
                _TimeDashRemaining = _TimeDashStart;
                _IsDashComplete = true;
                _Animator.SetTrigger("JumpDash");
                CreateDust();
                return;
            }

            if (_isLogicalTouchingGround)
            {
                _SoundController.PlayJump();
                CreateDust();
                _TimeCoyoteJumpRemaining = 0;
                Vector2 _jumpVelocity = new Vector2(0f, (_JumpSpeed * _GravityScale));
                _Rigidbody2D.velocity += _jumpVelocity;
                UpdateAnimationState("IsJumping", true);
            }
        }
    }

    public void Duck()
    {
        if (!IsAllowedToDuck())
            return;

        _SoundController.PlaySlide();
        _TimeDashRemaining = _TimeDuckStart;
        _Animator.SetTrigger("Duck");
    }

    public void Found(float direction)
    {
        Vector2 _jumpVelocity = new Vector2(0, (_JumpSpeed * _GravityScale));
        _Rigidbody2D.velocity = _jumpVelocity;
        _IsWallJumpComplete = true;
        _TimeWallJumpVelocityRemaining = _TimeWallJumpVelocityStart;
        _TimeMoveAfterWallJumpRemaining = _TimeMoveAfterWallJumpStart;
        _Direction = direction;
        FlipSprite(direction);
        return;
    }

    public bool IsAllowedToDuck()
    {
        var _isAbleToDuck = true;

        if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ducking"))
            _isAbleToDuck = false;

        if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
            _isAbleToDuck = false;

        if(!_IsTouchingGround)
            _isAbleToDuck = false;

        return _isAbleToDuck;
    }

    #endregion

    public void Restart()
    {
        // shouldn't actually be here but fuck it
        _IsAlive = true;
        _IsMoveAllowed = true;
        _Rigidbody2D.velocity = new Vector2(0, 0);
        _PlayerController.Restart();
        _PlayerController.EndCutscene();
        _Animator.SetTrigger("ForceRestart");
    }

    public void StartCutscene(string text)
    {
        _PlayerController.StartCutscene();
        _TimeCutsceneRemaining = _TimeCutsceneStart;
        _IsCutScene = true;
        _Text.gameObject.SetActive(true);
        _Text.text = text;
        UpdateAnimationState("IsCutscene", true);
        _SoundController.PlayMenuSelect();
    }

    public void EndCutscene()
    {
        _PlayerController.EndCutscene();
        _IsCutScene = false;
        _Text.gameObject.SetActive(false);
        UpdateAnimationState("IsCutscene", false);
        _SoundController.PlayMenuSelect();
    }

    void FlipSprite(float direction)
    {
        var _isPlayerMoving = Mathf.Abs(_Rigidbody2D.velocity.x) > Mathf.Epsilon;
        transform.localScale = new Vector2(Mathf.Sign(-direction), _GravityScale);
        UpdateAnimationState("IsRunning", _isPlayerMoving);
    }

    void Die()
    {
        if (!_IsAlive)
            return;

        // Hazard (Always)
        var _isTouchingHazard = false;

        if (_BodyCollider2D.IsTouchingLayers(LayerMask.GetMask("HazardUgh"))
    || _FeetCollider2D.IsTouchingLayers(LayerMask.GetMask("HazardUgh")))
        {
            _isTouchingHazard = true;
        }

        if (_BodyCollider2D.IsTouchingLayers(LayerMask.GetMask("HazardZap"))
            || _FeetCollider2D.IsTouchingLayers(LayerMask.GetMask("HazardZap")))
        {
            _SoundController.PlayZap();
            _isTouchingHazard = true;
        }

        if (_isTouchingHazard)
        {
            var _sound = Random.Range(0, 3);
            switch (_sound)
            {
                default:
                case 0:
                    _SoundController.PlayUgh1();
                    break;
                case 1:
                    _SoundController.PlayUgh2();
                    break;
                case 2:
                    _SoundController.PlayUgh3();
                    break;
            }

            _Rigidbody2D.drag = 0f;
            StartCoroutine(TriggerDie());
        }
    }

    IEnumerator TriggerDie()
    {
        _IsAlive = false;
        _IsMoveAllowed = false;
        _Animator.SetTrigger("Dying");
        _Rigidbody2D.velocity = new Vector2(_Direction * _RunSpeed / 3, 0);
        yield return new WaitForSecondsRealtime(_DeathDelay);
        _Animator.SetTrigger("ForceRestart");
        _IsMoveAllowed = true;
        _IsAlive = true;
    }

    void UpdateAnimationState(string state, bool value)
    {
        var _state = _Animator.GetBool(state);

        if (_state != value)
            _Animator.SetBool(state, value);
    }

    private void CreateDust()
    {
        _Dust.Play();
    }
}
