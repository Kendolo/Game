using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public State(string newName)
    {
        name = newName;
    }
    public string name;
    public bool hasSubStates;
    public delegate void DelegateVoid();
    public DelegateVoid OnEnter;
    public DelegateVoid OnStay;
    public DelegateVoid OnExit;

    public State currentSubState;
    public List<State> subStates;
    public List<Transition> subTransitions;

    public void SubMachine()
    {
        currentSubState.OnStay?.Invoke();
        for (int i = 0; i < subTransitions.Count; i++)
        {
            if (subTransitions[i].currentState == currentSubState.name && subTransitions[i].Condition() == true)
            {
                currentSubState.OnExit?.Invoke();
                for (int j = 0; j < subStates.Count; j++)
                {
                    if (subStates[j].name == subTransitions[i].nextState)
                    {
                        currentSubState = subStates[j];
                        currentSubState.OnEnter?.Invoke();
                        break;
                    }
                }
                break;
            }
        }    
    }
}

