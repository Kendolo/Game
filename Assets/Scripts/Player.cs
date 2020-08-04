using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    FSM fsm;
    Rigidbody2D body;
    public float runSpeed;
    public bool canTalk;
    public bool talkOver;
    public bool hasKey;

    void Start()
    {
        fsm = GetComponent<FSM>();
        body = GetComponent<Rigidbody2D>();

        fsm.states = new List<State>();

        fsm.states.Add(new State("Play"));
        fsm.states.Add(new State("Pause"));
        fsm.states.Add(new State("Menu"));
        fsm.currentState = fsm.states[0];

        fsm.transitions = new List<Transition>();
        fsm.transitions.Add(new Transition("Play", "Pause", Play_Pause));
        fsm.transitions.Add(new Transition("Pause", "Play", Pause_Play));
        fsm.transitions.Add(new Transition("Pause", "Menu", Pause_Menu));
        fsm.transitions.Add(new Transition("Menu", "Play", Menu_Play));

        for (int i = 0; i < fsm.states.Count; i++)
        {
            if (fsm.states[i].name == "Play")
            {
                fsm.states[i].OnStay = OnStayPlay;

                fsm.states[i].hasSubStates = true;
                fsm.states[i].subStates = new List<State>();
                fsm.states[i].subTransitions = new List<Transition>();

                fsm.states[i].subStates.Add(new State("Idle"));
                fsm.states[i].subStates.Add(new State("RunLeft"));
                fsm.states[i].subStates.Add(new State("RunRight"));
                fsm.states[i].subStates.Add(new State("Talking"));
                fsm.states[i].subStates.Add(new State("Rewinding"));
                fsm.states[i].currentSubState = fsm.states[i].subStates[0];

                fsm.states[i].subTransitions.Add(new Transition("Idle", "RunLeft", Idle_RunLeft));
                fsm.states[i].subTransitions.Add(new Transition("RunLeft", "Idle", RunLeft_Idle));
                fsm.states[i].subTransitions.Add(new Transition("Idle", "RunRight", Idle_RunRight));
                fsm.states[i].subTransitions.Add(new Transition("RunRight", "Idle", RunRight_Idle));
                fsm.states[i].subTransitions.Add(new Transition("Idle", "Talking", Idle_Talking));
                fsm.states[i].subTransitions.Add(new Transition("Talking", "Idle", Talking_Idle));
                fsm.states[i].subTransitions.Add(new Transition("RunLeft", "Talking", RunLeft_Talking));
                fsm.states[i].subTransitions.Add(new Transition("RunRight", "Talking", RunRight_Talking));
                fsm.states[i].subTransitions.Add(new Transition("Idle", "Rewinding", Idle_Rewinding));
                fsm.states[i].subTransitions.Add(new Transition("Rewinding", "Idle", Rewinding_Idle));
                fsm.states[i].subTransitions.Add(new Transition("RunLeft", "Rewinding", RunLeft_Rewinding));
                fsm.states[i].subTransitions.Add(new Transition("RunRight", "Rewinding", RunRight_Rewinding));

                for (int j = 0; j < fsm.states[i].subStates.Count; j++)
                {
                    if (fsm.states[i].subStates[j].name == "Idle")
                    {
                        fsm.states[i].subStates[j].OnEnter = OnEnterIdle;
                    }
                    else if (fsm.states[i].subStates[j].name == "RunLeft")
                    {
                        fsm.states[i].subStates[j].OnStay = OnStayRunLeft;
                    }
                    else if (fsm.states[i].subStates[j].name == "RunRight")
                    {
                        fsm.states[i].subStates[j].OnStay = OnStayRunRight;
                    }

                    //and so on
                }
            }
            //and so on
        }
    }

    void Update()
    {
        //Debug.Log(fsm.currentState.name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            canTalk = true;
        }
        //...
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            canTalk = true;
            talkOver = false;
        }
        //...
    }

    //Conditions
    public bool Menu_Play()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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

    //Subonditions:
    public bool Idle_RunLeft()
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

    public bool RunLeft_Idle()
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

    public bool Idle_RunRight()
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

    public bool RunRight_Idle()
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

    public bool Idle_Talking()
    {
        if (Input.GetKey(KeyCode.T) && canTalk)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Talking_Idle()
    {
        if (talkOver)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RunLeft_Talking()
    {
        if (Input.GetKey(KeyCode.T) && canTalk)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RunRight_Talking()
    {
        if (Input.GetKey(KeyCode.T) && canTalk)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Idle_Rewinding()
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

    public bool Rewinding_Idle()
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

    public bool RunLeft_Rewinding()
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

    public bool RunRight_Rewinding()
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

    public void OnStayPlay()
    {

    }

    //Substate methods
    public void OnEnterIdle()
    {
        body.velocity = new Vector2(0f, 0f);
    }

    public void OnStayRunLeft()
    {
        //body.AddForce(new Vector2(-runSpeed, 0f));
        body.velocity = new Vector2(-runSpeed, 0f);
    }

    public void OnStayRunRight()
    {
        //body.AddForce(new Vector2(-runSpeed, 0f));
        body.velocity = new Vector2(runSpeed, 0f);
    }
}
