using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Transition(string newCurrentState, string newNextState, DelegateBool newCondition)
    {
        currentState = newCurrentState;
        nextState = newNextState;
        Condition = newCondition;
    }
    public string currentState;
    public string nextState;
    public delegate bool DelegateBool();
    public DelegateBool Condition;
}
