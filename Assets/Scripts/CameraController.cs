using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Recorder recorder;

    void Start()
    {
        recorder = GameObject.Find("Recorder").GetComponent<Recorder>();
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
