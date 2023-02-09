using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : StateMachineBehaviour
{
    private void Awake()
    {
        _rb2D = GameObject.Find("----Player----").GetComponent<Rigidbody2D>();
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var magnitude = _rb2D.velocity.magnitude;
        float maxValue = Mathf.Max(Mathf.Abs(_vertical), Mathf.Abs(_horizontal));
        animator.SetFloat("MoveSpeed", maxValue);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


    private void Move()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isStatic = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isStatic = false;
        }
        _direction.x = _horizontal = Input.GetAxis("Horizontal");
        _direction.y = _vertical = Input.GetAxis("Vertical");
    }

    private float _horizontal;
    private float _vertical;
    public Vector2 _direction;
    private Rigidbody2D _rb2D;
    private bool isStatic;

}
