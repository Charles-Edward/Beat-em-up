using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();


    }
    void Start()
    {
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
                break;
            case PlayerStateMode.BASICPUNCH:
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        float maxValue = Mathf.Max(Mathf.Abs(_vertical), Mathf.Abs(_horizontal));

        switch (_currentState)
        {
            case PlayerStateMode.IDLE:

                SwitchToSprint();

                break;
            case PlayerStateMode.WALK:
                _animator.SetFloat("MoveSpeed", maxValue);
                SwitchToSprint();

                break;
            case PlayerStateMode.SPRINT:
                if (Input.GetButtonUp("Fire3"))
                {
                    TransitionToState(PlayerStateMode.WALK);
                }
                break;
            case PlayerStateMode.JUMP:
                break;
            case PlayerStateMode.BASICPUNCH:
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
                break;
            case PlayerStateMode.BASICPUNCH:
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
#endregion

    private Animator _animator;
    private PlayerStateMode _currentState;
    private float _horizontal;
    private float _vertical;
}
