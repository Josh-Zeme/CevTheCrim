using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sylvester : MonoBehaviour
{
    [SerializeField] private ParticleSystem _Projectile;
    [SerializeField] private BoxCollider2D _HeadCollider2D;
    [SerializeField] private Gem _BlueGem;
    private GameLogic _GameLogic;
    private BossHealthUI _UI;
    private GemController _GemController;
    private SoundController _SoundController;
    private int _Health = 5;
    private bool _IsAttacking = false;
    private bool _IsStarted = false;
    private bool _IsHurt = false;
    public Animator _Animator;
    private float _TimeAttackStart = 15f;
    private float _TimeAttackRemaining = 0;
    private float _TimeIdleStart = 5f;
    private float _TimeIdleRemaining = 0;
    private float _StartingRotationSpeed = 0.5f;
    private float _RotationSpeed = 0.5f;

    void Awake()
    {
        _Animator = GetComponent<Animator>();
        _TimeIdleRemaining = Random.Range(2, _TimeIdleStart);
        _Projectile.Stop();
        _SoundController = GetComponent<SoundController>();
        _GemController = FindObjectOfType<GemController>();
        _GameLogic = FindObjectOfType<GameLogic>();
        _UI =_GameLogic.GetBossUI();
    }

    private void FixedUpdate()
    {
        if (!_IsStarted)
            return;

        if (_IsHurt)
            return;

        if(_IsAttacking)
            RotateProjectiles();

        if (_IsAttacking && _TimeAttackRemaining < 0)
        {
            AttackComplete();
        }

        if (!_IsAttacking && _TimeIdleRemaining < 0)
        {
            Attack();
        }

        _TimeIdleRemaining -= Time.deltaTime;
        _TimeAttackRemaining -= Time.deltaTime;
    }

    void RotateProjectiles()
    {
        if (!_IsAttacking)
            return;

        _Projectile.transform.Rotate(new Vector3(0, 0, _RotationSpeed), Space.Self);
    }

    private void Attack()
    {
        var _direction = Random.Range(0, 1);
        _RotationSpeed = _direction == 0 ? _RotationSpeed : _RotationSpeed * -1;
        _SoundController.PlaySandStart();
        _Animator.SetTrigger("Attack");
        _IsAttacking = true;
        _Projectile.Play();
        _HeadCollider2D.enabled = false;
        _TimeAttackRemaining = _TimeAttackStart;
        _SoundController.PlaySand();
    }

    private void AttackComplete()
    {
        _SoundController.StopSand();
        _Animator.SetTrigger("AttackComplete");
        _HeadCollider2D.enabled = true;
        _IsAttacking = false;
        _Projectile.Stop();
        _TimeIdleRemaining = _TimeIdleStart;
    }

    private void Hurt()
    {
        if (_IsHurt)
            return;

        _Animator.SetTrigger("Hurt");
        _HeadCollider2D.enabled = false;
        _IsHurt = true;
        _Health--;
        _RotationSpeed = Mathf.Abs(_RotationSpeed) + 0.35f;
        _SoundController.PlayBossHurt();
        _UI.LoseHealth(_Health);

        if (_Health == 0)
        {
            var _gem = Instantiate(_BlueGem);
            var _mockPosition = transform.position;
            _mockPosition.y += 4.4f;
            _gem.transform.position = _mockPosition;
            if(_GemController == null)
            {
                _GemController = FindObjectOfType<GemController>();
            }

            _GemController.SetDestructableGem(_gem);
            Die();
            _UI.gameObject.SetActive(false);
        }
    }

    public void HurtComplete()
    {
        if (_Health == 0)
            return;

        _HeadCollider2D.enabled = true;
        _IsHurt = false;
        Attack();
    }

    public void Pause()
    {
        _IsStarted = false;
        if (_IsAttacking)
        {
            _Animator.SetTrigger("AttackComplete");
            _Projectile.Stop();
            _SoundController.StopSand();
        }
    }

    public void Resume()
    {
        _IsStarted = true;
        if (_IsAttacking)
        {
            Attack();
        }
    }

    void Die()
    {
        StartCoroutine(TriggerDie());
    }

    IEnumerator TriggerDie()
    {
        _SoundController.PlayBossDefeat();
        yield return new WaitForSecondsRealtime(2f);
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (_IsAttacking)
            return;

        if (collision.tag == "Player")
        {
            Hurt();
        }
    }

    public void Restart()
    {
        if (_IsAttacking)
        {
            _Animator.SetTrigger("AttackComplete");
        }
        _RotationSpeed = _StartingRotationSpeed;
        _TimeIdleRemaining = Random.Range(2, _TimeIdleStart);
        _TimeAttackRemaining = 0;
        _Health = 5;
        _IsAttacking = false;
        _IsStarted = false;
        _IsHurt = false;
    }
}