using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    FSM fsm;
    SpriteRenderer spriteRenderer;
    public List<Texture2D> screenHistory;
    public Texture2D snapShot;
    public Sprite currentSprite;
    public int steps;
    public float rewindSpeed;
    float rSpeed;
    public float captureSpeed;
    float cSpeed;
    public bool grab;
    public int ppu;

    void Start()
    {
        fsm = GetComponent<FSM>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        spriteRenderer.color = Color.grey;

        fsm.states = new List<State>();
        fsm.states.Add(new State("Play"));
        fsm.states.Add(new State("Hold"));
        fsm.states.Add(new State("Rewinding"));
        fsm.states.Add(new State("Rerewinding"));
        fsm.currentState = fsm.states[0];

        fsm.transitions = new List<Transition>();
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
                fsm.states[i].OnExit = OnExitPlay;
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

    //Conditions
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
        if (Input.GetKey(KeyCode.LeftArrow))
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
        if (!Input.GetKey(KeyCode.LeftArrow))
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
        if (Input.GetKey(KeyCode.RightArrow))
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
        if (!Input.GetKey(KeyCode.RightArrow))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //State methods
    public void OnEnterPlay()
    {
        if (screenHistory != null)
        {
            for (int i = screenHistory.Count - 1; i >= steps; i--)
            {
                screenHistory.RemoveAt(i);
            }
        }

        spriteRenderer.enabled = false;
    }

    public void OnStayPlay()
    {
        if (cSpeed > captureSpeed)
        {
            grab = true;
            cSpeed = 0;
        }
        else
        {
            cSpeed += Time.deltaTime;
        }
    }

    public void OnExitPlay()
    {
        spriteRenderer.enabled = true;
        grab = false;
        if (screenHistory.Count > steps)
        {
            spriteRenderer.sprite = Sprite.Create(screenHistory[steps], new Rect(0, 0, screenHistory[steps].width, screenHistory[steps].height), new Vector2(0.5f, 0.5f), ppu);
        }
    }

    public void OnStayRewinding()
    {
        if (rSpeed > rewindSpeed)
        {
            if (steps > 0)
            {
                steps--;
                spriteRenderer.sprite = Sprite.Create(screenHistory[steps], new Rect(0, 0, screenHistory[steps].width, screenHistory[steps].height), new Vector2(0.5f, 0.5f), ppu);
            }
            rSpeed = 0;
        }
        else
        {
            rSpeed += Time.deltaTime;
        }
    }

    public void OnStayRerewinding()
    {
        if (rSpeed > rewindSpeed)
        {
            if (steps < screenHistory.Count - 1)
            {
                steps++;
                Debug.Log("Rerewinding");
                spriteRenderer.sprite = Sprite.Create(screenHistory[steps], new Rect(0, 0, screenHistory[steps].width, screenHistory[steps].height), new Vector2(0.5f, 0.5f), ppu);
            }
            rSpeed = 0;
        }
        else
        {
            rSpeed += Time.deltaTime;
        }
    }
}
