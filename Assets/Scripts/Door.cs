using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Player player;
    SpriteRenderer spriteRenderer;
    FSM fsm;
    public List<State> subStateHistory;
    int steps;
    public float rewindSpeed;
    float rSpeed;
    public float captureSpeed;
    float cSpeed;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fsm = GetComponent<FSM>();
        subStateHistory = new List<State>();

        fsm.states = new List<State>();
        fsm.states.Add(new State("Play"));
        fsm.states.Add(new State("Pause"));
        fsm.states.Add(new State("Menu"));
        fsm.states.Add(new State("Hold"));
        fsm.states.Add(new State("Rewinding"));
        fsm.states.Add(new State("Rerewinding"));
        fsm.currentState = fsm.states[0];

        fsm.transitions = new List<Transition>();
        fsm.transitions.Add(new Transition("Play", "Pause", Play_Pause));
        fsm.transitions.Add(new Transition("Pause", "Play", Pause_Play));
        fsm.transitions.Add(new Transition("Pause", "Menu", Pause_Menu));
        fsm.transitions.Add(new Transition("Menu", "Play", Menu_Play));
        fsm.transitions.Add(new Transition("Play", "Hold", Play_Hold));
        fsm.transitions.Add(new Transition("Hold", "Play", Hold_PLay));
        fsm.transitions.Add(new Transition("Hold", "Rewinding", Hold_Rewinding));
        fsm.transitions.Add(new Transition("Rewinding", "Hold", Rewinding_Hold));
        fsm.transitions.Add(new Transition("Hold", "Rerewinding", Hold_Rerewinding));
        fsm.transitions.Add(new Transition("Rerewinding", "Hold", Rerewinding_Hold));

        for (int i = 0; i < fsm.states.Count; i++)
        {
            if (fsm.states[i].name == "Play")
            {
                fsm.states[i].OnEnter = OnEnterPlay;
                fsm.states[i].OnStay = OnStayPlay;

                fsm.states[i].hasSubStates = true;
                fsm.states[i].subStates = new List<State>();
                fsm.states[i].subTransitions = new List<Transition>();

                fsm.states[i].subStates.Add(new State("Locked"));
                fsm.states[i].subStates.Add(new State("Closed"));
                fsm.states[i].subStates.Add(new State("Open"));
                fsm.states[i].currentSubState = fsm.states[i].subStates[0];

                fsm.states[i].subTransitions.Add(new Transition("Locked", "Closed", Locked_Closed));
                fsm.states[i].subTransitions.Add(new Transition("Closed", "Open", Closed_Open));
                fsm.states[i].subTransitions.Add(new Transition("Open", "Closed", Open_Closed));

                for (int j = 0; j < fsm.states[i].subStates.Count; j++)
                {
                    if (fsm.states[i].subStates[j].name == "Locked")
                    {
                        fsm.states[i].subStates[j].OnStay = OnStayLocked;
                    }
                    else if (fsm.states[i].subStates[j].name == "Closed")
                    {
                        fsm.states[i].subStates[j].OnEnter = OnEnterClosed;
                    }
                    else if (fsm.states[i].subStates[j].name == "Open")
                    {
                        fsm.states[i].subStates[j].OnEnter = OnEnterOpen;
                    }
                }
            }
            else if (fsm.states[i].name == "Rewinding")
            {
                fsm.states[i].OnStay = OnStayRewinding;
            }
            else if (fsm.states[i].name == "Rerewinding")
            {
                fsm.states[i].OnStay = OnStayRerewinding;
            }
        }
    }

    void Update()
    {
        Debug.Log(fsm.currentState.name);
    }

    //Conditions
    public bool Menu_Play()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Pause_Menu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Play_Pause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Pause_Play()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Play_Hold()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Hold_PLay()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Hold_Rewinding()
    {
        if (Input.GetKey(KeyCode.A))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Rewinding_Hold()
    {
        if (!Input.GetKey(KeyCode.A))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Hold_Rerewinding()
    {
        if (Input.GetKey(KeyCode.D))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Rerewinding_Hold()
    {
        if (!Input.GetKey(KeyCode.D))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Subconditions
    public bool Locked_Closed()
    {
        if (player.hasKey && Input.GetKeyDown(KeyCode.U))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Closed_Open()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Open_Closed()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Statemethods
    public void OnEnterPlay()
    {
        if (subStateHistory != null)
        {
            fsm.currentState.currentSubState = subStateHistory[steps - 1];
            fsm.currentState.currentSubState.OnEnter?.Invoke();
            for (int i = subStateHistory.Count - 1; i > steps; i--)
            {
                subStateHistory.RemoveAt(i);
            }
        }
    }

    public void OnStayPlay()
    {
        if (cSpeed > captureSpeed)
        {
            subStateHistory.Add(fsm.currentState.currentSubState);
            cSpeed = 0;
        }
        else
        {
            cSpeed += Time.deltaTime;
        }
        steps = subStateHistory.Count;
        Debug.Log(steps);
        Debug.Log(fsm.currentState.currentSubState.name);

    }

    public void OnStayRewinding()
    {
        if (rSpeed > rewindSpeed)
        {
            if (steps > 1)
            {
                steps--;
            }
            rSpeed = 0;
        }
        else
        {
            rSpeed += Time.deltaTime;
        }

        Debug.Log(steps);
    }

    public void OnStayRerewinding()
    {
        if (rSpeed > rewindSpeed)
        {
            if (steps < subStateHistory.Count)
            {
                steps++;
            }
            rSpeed = 0;
        }
        else
        {
            rSpeed += Time.deltaTime;
        }
        Debug.Log(steps);
    }

    //Substatemethods
    public void OnStayLocked()
    {
        spriteRenderer.color = Color.grey;
    }

    public void OnEnterClosed()
    {
        spriteRenderer.color = Color.yellow;
    }

    public void OnEnterOpen()
    {
        spriteRenderer.color = Color.white;
    }
}
