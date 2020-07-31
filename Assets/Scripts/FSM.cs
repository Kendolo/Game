using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public State currentState;
    public List<State> states;
    public List<Transition> transitions;

    void Update()
    {
        currentState.OnStay();
        for (int i = 0; i < transitions.Count; i++)
        {
            if (transitions[i].currentState == currentState.name && transitions[i].Condition() == true)
            {
                currentState.OnExit();
                for(int j = 0; j < states.Count; j++)
                {
                    if (states[j].name == transitions[i].nextState)
                    {
                        currentState = states[j];
                        currentState.OnEnter();
                    }
                }
            }
        }
    }
}
