//  []

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



enum EnemyStateMode
{
    IDLE,
    WALK,
    ATTACK,
    DEATH

}

public class EnemyStateMachine : MonoBehaviour
{

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Transform _moveTarget;
    [SerializeField]
    private float _limitNearTarget = 0.5f;
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private float _waitingTimeBeforeAttack = 1f;
    [SerializeField]
    private float _attackDuration = 0.2f;
    [SerializeField]
    private Transform _lastPositionX;
    [SerializeField]
    private GameObject _hitBox;
    [SerializeField]
    private IntVariable _dataInt;
    [SerializeField]
    private int _enemyHealth;





    // Private & Protected
    private EnemyStateMode _currentState;
    private bool _playerDetected = false;
    private float _attackTimer;
    //private Transform _moveTarget;
    //private float _lastPositionX;
    private SpriteRenderer _flip;
    private Collider2D _collider2D;
    private Vector2 _initialColliderOffset;
    private Vector2 _localScale;


    private void Awake()
    {
        _localScale = transform.localScale;
        _flip = GetComponentInChildren<SpriteRenderer>();
        _collider2D = gameObject.GetComponentInChildren<Collider2D>();
        _initialColliderOffset = _collider2D.offset;
        _hitBox.SetActive(false);
        _enemyHealth = _dataInt.enemie_health;
    }
    void Start()
    {
        TransitionToState(EnemyStateMode.IDLE);
        //_moveTarget = GameObject.Find("Player").transform; // GetComponent<Transform>();

    }

    void Update()
    {
        IsDead();
        OnStateUpdate();
        if (_enemyHealth > 0)
        {
            Vector3 scale = transform.localScale;
            if (_lastPositionX.position.x > _moveTarget.position.x)
            {
                scale.x = -1;
                // _flip.flipX = true;
                //_collider2D.offset = new Vector2(-_initialColliderOffset.x, _collider2D.offset.y);

            }
            else if (_lastPositionX.position.x < _moveTarget.position.x)
            {
                // _flip.flipX = false;
                //_collider2D.offset = new Vector2(_initialColliderOffset.x, _collider2D.offset.y);
                //transform.localScale = new Vector2(_localScale.x, _localScale.y);
                scale.x = 1;
            }
            transform.localScale = scale;
        }



    }

    void OnStateEnter()
    {
        switch (_currentState)
        {
            case EnemyStateMode.IDLE:
                _attackTimer = 0f;
                break;
            case EnemyStateMode.WALK:
                _animator.SetBool("isWalking", true);
                break;
            case EnemyStateMode.ATTACK:
                _hitBox.SetActive(true);
                _attackTimer = 0f;
                _animator.SetBool("isAttacking", true);
                break;

            case EnemyStateMode.DEATH:
                _animator.SetBool("isDead", true);
                break;

            default:
                break;
        }





    }

    void OnStateUpdate()
    {
        switch (_currentState)
        {
            case EnemyStateMode.IDLE:
                if (_playerDetected && !IsTargetNearLimit())
                {
                    TransitionToState(EnemyStateMode.WALK);

                }

                if (IsTargetNearLimit())
                {
                    _attackTimer += Time.deltaTime;
                    if (_attackTimer >= _waitingTimeBeforeAttack)
                    {
                        TransitionToState(EnemyStateMode.ATTACK);

                    }
                }
                break;

            case EnemyStateMode.WALK:

                transform.position = Vector2.MoveTowards(transform.position, _moveTarget.position, _moveSpeed * Time.deltaTime);
                //transform.position = Vector2.Lerp(transform.position, _moveTarget.position, Time.deltaTime);

                if (IsTargetNearLimit())
                {
                    TransitionToState(EnemyStateMode.IDLE);

                }
                if (!_playerDetected)
                {
                    TransitionToState(EnemyStateMode.IDLE);
                }

                break;
            case EnemyStateMode.ATTACK:
                _attackTimer += Time.deltaTime;
                if (_attackTimer >= _attackDuration)
                {
                    TransitionToState(EnemyStateMode.IDLE);
                }
                break;
            case EnemyStateMode.DEATH:
                break;
            default:
                break;
        }



    }

    void OnStateExit()
    {
        switch (_currentState)
        {
            case EnemyStateMode.IDLE:
                break;
            case EnemyStateMode.WALK:
                _animator.SetBool("isWalking", false);
                break;
            case EnemyStateMode.ATTACK:
                _hitBox.SetActive(false);
                _animator.SetBool("isAttacking", false);
                break;
            case EnemyStateMode.DEATH:
                Invoke("DestroyObject", 3);

                break;
            default:
                break;
        }


    }

    void TransitionToState(EnemyStateMode nextState)
    {
        OnStateExit();
        _currentState = nextState;
        OnStateEnter();

    }

    // Méthode pour savoir si le player est détecté pour l'ennemi
    void PlayerDetected()
    {
        //Debug.Log("Player Detected");
        _playerDetected = true;

    }

    // Méthode pour savoir si le player s'est échappé
    void PlayerEscaped()
    {
        //Debug.Log("PLayer Escaped");
        _playerDetected = false;


    }

    // Méthode pour définir la distance d'arret de l'ennemi par rapport au player
    bool IsTargetNearLimit()
    {
        return Vector2.Distance(transform.position, _moveTarget.position) < _limitNearTarget;

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("HitBoxPlayer"))
        {
            GetDamage(_dataInt.damagesToEnemies);
        }
    }




    public void GetDamage(int damage)
    {
        _enemyHealth -= damage;
    }

    void IsDead()
    {
        if (_enemyHealth <= 0)
        {
            TransitionToState(EnemyStateMode.DEATH);
        }
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}



