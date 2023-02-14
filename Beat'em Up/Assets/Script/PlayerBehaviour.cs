using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Playables;

enum PlayerStateMode
{
    IDLE,
    WALK,
    SPRINT,
    JUMP,
    BASICPUNCH,
    DEATH
}

public class PlayerBehaviour : MonoBehaviour
{
    #region SerializeField & public
    [Header("Jump Settings")]
    [SerializeField] AnimationCurve _jumpCurve;
    [SerializeField] float _jumpHeight = 0.7f;
    [SerializeField] float _jumpDuration = 3f;

    [Header("Movements")]
    [SerializeField] private float _moveSpeed = 70f;
    [SerializeField] private float _runSpeed = 1.3f;

    [Header("Stats Player")]
    [SerializeField] HealthBar _healthBar;
    [SerializeField] IntVariable _dataInt;
    [SerializeField] int _currentHealth;
    [SerializeField] int _currentMana;
    [SerializeField] float invincibilityDuration = 1.0f;
    [SerializeField] float _waitingTimeBeforeAttack = 1f;

    private Vector2 _localScale;
    [SerializeField] private Collider2D _hitBoxFist;
    [SerializeField] private Collider2D _hitBoxPlayer;
    [SerializeField] private GameObject _buttonRetry;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        // ---- Gestion flip collider & sprites ----
        _collider = gameObject.GetComponent<Collider2D>();
        _localScale = transform.localScale;
        _hitBoxFist.GetComponent<Collider2D>().enabled = false;
        _initialColliderOffset = _collider.offset;
        flip = GetComponentInChildren<SpriteRenderer>();
        _graphics = transform.Find("Graphic");
        // -----------------------------------------

        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        // ---- Stats player ----
        _currentMana = _dataInt.player_mana;
        _currentHealth = _dataInt.player_health;
        _healthBar.SetMaxHealth(_dataInt.player_health);
        // _manaBar.SetMaxMana(_maxMana.m_mana);
        // ----------------------
        _buttonRetry.SetActive(false);
    }
    void Start()
    {
        // Etat idle au start 
        _buttonSprint = false;
        TransitionToState(PlayerStateMode.IDLE);
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire3"))
        {
            _buttonSprint = true;
        }
        if (Input.GetButtonUp("Fire3"))
        {
            _buttonSprint = false;
        }
        GetInputAndFlipSprite();
        SwitchToDeath();
        OnStateUpdate();

    }

    private void FixedUpdate()
    {
        MoveAndSprint();
    }
    #endregion

    #region Methods
    private void GetInputAndFlipSprite()
    {

        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");

        Flip();
    }

    private void Flip()
    {
        if (!_currentState.Equals(PlayerStateMode.DEATH))
        {

            if (_direction.x < 0)
            {
                // flip.flipX = true;
                _collider.offset = new Vector2(-_initialColliderOffset.x, _collider.offset.y); // flip du collider pour coller un peu plus au sprite 2d
                transform.localScale = new Vector2(-_localScale.x, _localScale.y);
            }
            else if (_direction.x > 0)
            {
                //flip.flipX = false;
                _collider.offset = new Vector2(_initialColliderOffset.x, _collider.offset.y);
                transform.localScale = new Vector2(_localScale.x, _localScale.y);
            }
        }
    }

    private void MoveAndSprint()
    {
        if (_currentState.Equals(PlayerStateMode.BASICPUNCH) || _currentState.Equals(PlayerStateMode.DEATH)) // Si on frappe on ne bouge pas
        {
            _rb2D.velocity = _direction.normalized * 0 * Time.fixedDeltaTime;

        }
        else
        {
            if (_currentState.Equals(PlayerStateMode.SPRINT)) // si on sprint on multiplie _moveSpeed pour aller plus vite
            {
                _rb2D.velocity = _direction.normalized * (_moveSpeed * _runSpeed) * Time.fixedDeltaTime;

            }
            else // sinon on avance normalement
            {
                _rb2D.velocity = _direction.normalized * _moveSpeed * Time.fixedDeltaTime;
            }
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("P collided with " + collision.otherCollider);
        if (collision.gameObject.tag == "HitBox" && Time.time >= lastDamageTime + invincibilityDuration)
        {
            if (collision.transform.parent.transform.parent.tag == "GreenEnemy")
            {
                TakeDamage(_dataInt.damageGreenEnemy);
            }
            if (collision.transform.parent.transform.parent.tag == "RedEnemy")
            {
                TakeDamage(_dataInt.damageRedEnemy);
            }
            if (collision.transform.parent.transform.parent.tag == "Boss")
            {
                TakeDamage(_dataInt.damageBoss);
            }
            if (collision.transform.parent.transform.parent.tag == "WhiteEnemy")
            {
                TakeDamage(_dataInt.damageWhiteEnmy);
            }
            lastDamageTime = Time.time;
        }
    }


    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _healthBar.SetHealth(_currentHealth);
    }
    #endregion

    #region States
    void OnStateEnter()
    {
        switch (_currentState)
        {
            case PlayerStateMode.IDLE:
                break;
            case PlayerStateMode.WALK:
                break;
            case PlayerStateMode.SPRINT:
                _animator.SetBool("isRunning", true);
                break;
            case PlayerStateMode.JUMP:
                _animator.SetBool("isJumping", true);
                break;
            case PlayerStateMode.BASICPUNCH:
                _animator.SetBool("isPunching", true);
                break;
            case PlayerStateMode.DEATH:
                _animator.SetBool("isDead", true);
                break;
            default:
                break;
        }
    }

    // l'utilisation de Mathf.abs permet d'avoir une valeur toujours positive, aller vers la gauche = valeur négative en X et on passe cette valeur négative en valeur positive
    // pour voir entrer dans les conditions de l'animator (ex : Transition si MoveSpeed > .1, si on va vers la gauche on aura une valeur négative et on entrera
    // jamais dans cette condition
    // l'utilisation de Mathf.Max permet de récupérer la valeur la plus grande entre X et Y, donc si on va vers le haut on aura 0,1,0 on n'utilise donc pas X alors on va se servir
    // du Y pour entrer dans la conditions de l'animator qui est actuellement : if(MoveSpeed > .1)
    void OnStateUpdate()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
        _magnitude = _rb2D.velocity.magnitude;
        float maxValue = Mathf.Max(Mathf.Abs(_direction.y), Mathf.Abs(_direction.x));

        switch (_currentState)
        {
            case PlayerStateMode.IDLE:

                _animator.SetFloat("MoveSpeed", _magnitude);
                SwitchToSprint();
                SwitchToJump();
                SwitchToBasicPunch();
                SwitchToDeath();

                break;

            case PlayerStateMode.WALK:
                _animator.SetFloat("MoveSpeed", _magnitude);
                SwitchToSprint();
                SwitchToJump();
                SwitchToBasicPunch();
                SwitchToDeath();
                break;

            case PlayerStateMode.SPRINT:

                if (Input.GetButtonUp("Fire3"))  // si on arrête d'appuyer sur sprint on repasse en walk
                {
                    TransitionToState(PlayerStateMode.WALK);
                }
                else if (_magnitude <= 1e-3f)
                {

                    TransitionToState(PlayerStateMode.IDLE);
                }

                else if (Input.GetButtonDown("Jump")) // permet de sauter dans l'état sprint
                {
                    TransitionToState(PlayerStateMode.JUMP);
                }
                break;

            case PlayerStateMode.JUMP:
                if (_jumpTimer < _jumpDuration)
                {
                    _jumpTimer += Time.deltaTime;
                    float y = _jumpCurve.Evaluate(_jumpTimer / _jumpDuration);
                    _graphics.localPosition = new Vector3(_graphics.localPosition.x, y * _jumpHeight, _graphics.localPosition.z);
                }
                else
                {
                    _jumpTimer = 0f;
                    if (_magnitude > 0) // si la vraie vitesse (magnitude) est supérieure à 0 on marche
                    {
                        TransitionToState(PlayerStateMode.WALK);
                    }
                    else if (Input.GetButtonDown("Fire3"))
                    {
                        TransitionToState(PlayerStateMode.SPRINT);
                    }
                    else
                    {
                        TransitionToState(PlayerStateMode.IDLE);
                    }
                }
                break;

            case PlayerStateMode.BASICPUNCH:
                timeSinceLastDisable += Time.deltaTime;

                if (timeSinceLastDisable >= _waitingTimeBeforeAttack)
                {
                    _hitBoxFist.enabled = !_hitBoxFist.enabled;
                    timeSinceLastDisable = 0;
                }

                if (Input.GetButtonUp("Fire1")) // si on arrête d'attaquer on passe en idle
                {
                    TransitionToState(PlayerStateMode.IDLE);
                    _hitBoxFist.enabled = false;
                }
                break;

            case PlayerStateMode.DEATH:
                timeSinceLastDisable += Time.deltaTime;
                if (timeSinceLastDisable >= .5)
                {
                _buttonRetry.SetActive(true);
                }
                break;
            default:
                break;
        }
    }



    void OnStateExit()
    {
        switch (_currentState)
        {
            case PlayerStateMode.IDLE:
                break;
            case PlayerStateMode.WALK:
                break;
            case PlayerStateMode.SPRINT:
                _animator.SetBool("isRunning", false);
                break;
            case PlayerStateMode.JUMP:
                _animator.SetBool("isJumping", false);
                break;
            case PlayerStateMode.BASICPUNCH:
                _animator.SetBool("isPunching", false);
                // _hitBoxFist.SetActive(false);
                break;
            case PlayerStateMode.DEATH:
                _animator.SetBool("isDead", false);
                break;
            default:
                break;
        }
    }

    private void TransitionToState(PlayerStateMode ToState)
    {
        OnStateExit();
        _currentState = ToState;
        OnStateEnter();
    }

    private void SwitchToSprint()
    {
        if (_buttonSprint && _magnitude > 0)
        {
            TransitionToState(PlayerStateMode.SPRINT);
        }


    }

    private void SwitchToJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            TransitionToState(PlayerStateMode.JUMP);
        }
    }

    private void SwitchToBasicPunch()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            TransitionToState(PlayerStateMode.BASICPUNCH);
        }
    }
    private void SwitchToDeath()
    {
        if (_currentHealth <= 0)
        {
            TransitionToState(PlayerStateMode.DEATH);
        }
    }
    #endregion

    #region Privates
    private PlayerStateMode _currentState;
    private Vector2 _direction;
    private Animator _animator;
    private Rigidbody2D _rb2D;
    private SpriteRenderer flip;
    private Transform _graphics;
    private float _jumpTimer;
    private Collider2D _collider;
    private Vector2 _initialColliderOffset;
    private bool _buttonSprint;
    private bool hasTakenDamage;
    private float lastDamageTime = 0f;
    private float _magnitude;
    private float _attackTimer;
    private float timeSinceLastDisable = 0;
    #endregion
}
