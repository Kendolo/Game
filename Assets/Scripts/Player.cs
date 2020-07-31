using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    FSM fsm;
    Rigidbody2D body;
    BoxCollider2D boxCollider;
    PlatformEffector2D platformEffector;

    void Start()
    {
        fsm = GetComponent<FSM>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        platformEffector = GetComponent<PlatformEffector2D>();

        fsm.states = new List<State>();
        fsm.states.Add(new State("Idle"));
        fsm.states.Add(new State("Run"));
        fsm.transitions.Add(new Transition("Idle", "Run", IdleRun));
        fsm.transitions.Add(new Transition("Run", "Idle", RunIdle));
    }

    void Update()
    {
        
    }

    //Conditions:

    public bool IdleRun()
    {
        return true; //test
    }

    public bool RunIdle()
    {
        return false; //test
    }
}
