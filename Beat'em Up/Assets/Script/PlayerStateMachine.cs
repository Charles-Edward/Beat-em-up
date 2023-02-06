using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

enum PlayerStateMode
{
    IDLE,
    WALK,
    SPRINT,
    JUMP,
    BASICPUNCH
}

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] AnimationCurve _jumpCurve;
    [SerializeField] float _jumpHeight = 0.7f;
    [SerializeField] float _jumpDuration = 3f;
    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _graphics = transform.Find("Graphic");

    }
    void Start()
    {
        // Etat idle au start 
        TransitionToState(PlayerStateMode.IDLE);
    }

    void Update()
    {
        OnStateUpdate();
    }
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
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        var magnitude = _rb2D.velocity.magnitude;
        Debug.Log(magnitude);
        // l'utilisation de Mathf.abs permet d'avoir une valeur toujours positive, aller vers la gauche = valeur négative en X et on passe cette valeur négative en valeur positive
        // pour voir entrer dans les conditions de l'animator (ex : Transition si MoveSpeed > .1, si on va vers la gauche on aura une valeur négative et on entrera
        // jamais dans cette condition
        // l'utilisation de Mathf.Max permet de récupérer la valeur la plus grande entre X et Y, donc si on va vers le haut on aura 0,1,0 on n'utilise donc pas X alors on va se servir
        // du Y pour entrer dans la conditions de l'animator qui est actuellement : if(MoveSpeed > .1)
        float maxValue = Mathf.Max(Mathf.Abs(_vertical), Mathf.Abs(_horizontal));

        switch (_currentState)
        {
            case PlayerStateMode.IDLE:

                _animator.SetFloat("MoveSpeed", maxValue);
                SwitchToSprint();
                SwitchToJump();
                SwitchToBasicPunch();

                break;
            case PlayerStateMode.WALK:
                _animator.SetFloat("MoveSpeed", maxValue);
                SwitchToSprint();
                SwitchToJump();
                SwitchToBasicPunch();
                break;
            case PlayerStateMode.SPRINT:
                if (Input.GetButtonUp("Fire3") /*|| maxValue < 0.1*/)
                {
                    TransitionToState(PlayerStateMode.WALK);
                }
                SwitchToJump();
                break;
            case PlayerStateMode.JUMP:
                if (_jumpTimer < _jumpDuration)
                {
                    _jumpTimer += Time.deltaTime;
                    float y = _jumpCurve.Evaluate(_jumpTimer / _jumpDuration);
                    _graphics.localPosition = new Vector3(_graphics.localPosition.x, y * _jumpHeight, _graphics.localPosition.z);
                }
                else if (_jumpTimer >= _jumpDuration)
                {
                    _jumpTimer = 0f;
                    TransitionToState(PlayerStateMode.IDLE);
                }
                break;
            case PlayerStateMode.BASICPUNCH:
                if (Input.GetButtonUp("Fire1"))
                {
                    TransitionToState(PlayerStateMode.IDLE);
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
        if (Input.GetButtonDown("Fire3"))
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
    #endregion

    private Animator _animator;
    private Rigidbody2D _rb2D;
    private PlayerStateMode _currentState;
    private float _horizontal;
    private float _vertical;
    Transform _graphics;
    private float _jumpTimer;
    private bool _isStatic;
}
