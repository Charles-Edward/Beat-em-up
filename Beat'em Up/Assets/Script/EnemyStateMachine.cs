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
    private GameObject _enemiesPrefab;
    [SerializeField]
    private GameObject _enemySpawn;



    // Private & Protected
    private EnemyStateMode _currentState;
    private bool _playerDetected = false;
    private float _attackTimer;
    //private Transform _moveTarget;
    //private float _lastPositionX;
    private SpriteRenderer _flip;
    private Collider2D _collider2D;
    private Vector2 _initialColliderOffset;
    private int _enemyCount;

    private void Awake()
    {
        _flip = GetComponentInChildren<SpriteRenderer>();
        _collider2D = gameObject.GetComponentInChildren<Collider2D>();
        _initialColliderOffset = _collider2D.offset;
        _hitBox.SetActive(false);
    }
    void Start()
    {
        TransitionToState(EnemyStateMode.IDLE);
        //_moveTarget = GameObject.Find("Player").transform; // GetComponent<Transform>();
    }

    void Update()
    {
        OnStateUpdate();
        /*_lastPositionX = transform.position.x;
        if (transform.position.x < 0)
        {
            _flip.flipX = false;

        }
        else if (transform.position.x > 0)
        {
            _flip.flipX = true;
        }*/
        

        if (_lastPositionX.position.x > _moveTarget.position.x)
        {
            //Debug.Log("je regarde vers la gauche");
            _flip.flipX = true;
            _collider2D.offset = new Vector2(-_initialColliderOffset.x, _collider2D.offset.y);
            
        }
        else if (_lastPositionX.position.x < _moveTarget.position.x )
        {
            //Debug.Log("je regarde vers la droite");
            _flip.flipX = false;
            _collider2D.offset = new Vector2(_initialColliderOffset.x, _collider2D.offset.y);

        }


    }

    private void FixedUpdate()
    {




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
                    if(_attackTimer >= _waitingTimeBeforeAttack)
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
                //_animator.SetBool("isAttacking", true);
                //_hitBox.SetActive(true);
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



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("HitBox"))
        {
            Debug.Log("player prend des dégats");

        }



    }


}



