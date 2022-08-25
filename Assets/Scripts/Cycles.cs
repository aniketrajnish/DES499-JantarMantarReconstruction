using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Cycles : MonoBehaviour
{
    float time;
    TimeSpan t;
    Transform sunTransform;    
    TextMeshProUGUI timeText;
    float intensity;

    public Light sun;
    [SerializeField] int factor;
    private void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        sunTransform = sun.transform;
    }
    void Cycle()
    {
        time += factor * Time.deltaTime;    
        t = TimeSpan.FromSeconds(time);
        string[] temp = t.ToString().Split(":"[0]);
        timeText.text = temp[0] + ":" + temp[1];
        sunTransform.rotation = Quaternion.Euler(new Vector3((time-21600)/86400*360, 0, 0));

        if (time > 86400)
            time = 0;

        if (time < 43200)
            intensity = 1 - (43200 - time) / 43200;
        else
            intensity = 1 - (time - 43200) / 43200;

        RenderSettings.fogColor = Color.Lerp(Color.black, Color.grey, intensity*intensity);
        sun.intensity = intensity;
    }
    void Update()
    {
        Cycle();
    }
}
