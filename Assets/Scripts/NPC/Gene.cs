using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gene : MonoBehaviour
{
    private Rigidbody2D _Rigidbody2D;
    private SoundController _SoundController;
    [SerializeField] private BoxCollider2D _FeetCollider2D;
    [SerializeField] private BoxCollider2D _AlertCollider2D;
    private bool _IsMoving = false;
    private bool _IsTouchingGround = true;
    private float _Direction = 1f;
    private float _MoveSpeed = 1f;
    public Animator _Animator;

    private float _TimeSleepStart = 5f;
    private float _TimeSleepRemaining = 0;

    private float _TimeWalkStart = 10f;
    private float _TimeWalkRemaining = 0;

    void Awake()
    {
        _SoundController = GetComponent<SoundController>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        _TimeWalkRemaining = Random.Range(0, _TimeWalkStart);
        _IsMoving = true;
    }

    private void Update()
    {
        _IsTouchingGround = _FeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private void FixedUpdate()
    {
        AnimateMovement();

        if (!_IsTouchingGround)
        {
            FlipSprite();
            _Direction *= -1;
        }

        if (_IsMoving)
        {
            _Rigidbody2D.velocity = new Vector2(_Direction * _MoveSpeed, 0);
            var _isSoundPlaying = _SoundController.IsStep1Playing() || _SoundController.IsStep2Playing();
            if (!_isSoundPlaying)
            {
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

        if(_IsMoving && _TimeWalkRemaining < 0)
        {
            _IsMoving = false;
            _AlertCollider2D.enabled = false;
            _TimeSleepRemaining = Random.Range(_TimeSleepStart /2, _TimeSleepStart);
        }

        if(!_IsMoving && _TimeSleepRemaining < 0)
        {
            _SoundController.PlaySnot();
            _IsMoving = true;
            _AlertCollider2D.enabled = true;
            _TimeWalkRemaining = _TimeWalkStart;
        }

        _TimeSleepRemaining -= Time.deltaTime;
        _TimeWalkRemaining -= Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_IsMoving)
            return;

        if (collision.tag == "Player")
        {
            _Animator.SetTrigger("Alert");
            _SoundController.PlayWhistle();
            collision.GetComponent<Player>().Found(_Direction);
        }
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(_Direction), 1);
    }

    void AnimateMovement()
    {
        var _isPlayerMoving = Mathf.Abs(_Rigidbody2D.velocity.x) > Mathf.Epsilon;
        UpdateAnimationState("IsRunning", _isPlayerMoving);
    }

    void UpdateAnimationState(string state, bool value)
    {
        var _state = _Animator.GetBool(state);

        if (_state != value)
            _Animator.SetBool(state, value);
    }
}