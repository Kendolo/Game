using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Recorder recorder;
    public float averageFPS;
    float sumFPS;
    int frames;
    public int ppu;
    public float zoom;

    void Start()
    {
        recorder = GameObject.Find("Recorder").GetComponent<Recorder>();
        Camera.main.orthographicSize = Screen.height / (zoom * 2 * ppu);
    }

    void Update()
    {
        frames++;
        sumFPS += 1 / Time.deltaTime;
        averageFPS = sumFPS / frames;
        Debug.Log(averageFPS);
    }

    public void OnPostRender()
    {
        if (recorder.grab)
        {
            recorder.screenHistory.Add(new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false));
            recorder.screenHistory[recorder.screenHistory.Count - 1].ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            recorder.screenHistory[recorder.screenHistory.Count - 1].Apply();
            recorder.steps = recorder.screenHistory.Count - 1;
            recorder.grab = false;
        }
    }

}
