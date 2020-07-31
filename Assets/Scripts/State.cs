using UnityEngine;

public class State
{
    public State(string newName)
    {
        name = newName;
    }
    public string name;
    public delegate void DelegateVoid();
    public DelegateVoid OnEnter;
    public DelegateVoid OnStay;
    public DelegateVoid OnExit;
}

