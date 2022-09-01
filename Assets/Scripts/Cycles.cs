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
    float camPivY = 0;
    [SerializeField] Transform camPivot;
    [SerializeField] TMP_InputField HH, MM;

    public Light sun;
    [SerializeField] int factor;
    private void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        sunTransform = sun.transform;
        Cycle(0);
        Cycle(0);
    }
    void Cycle(float _inp)
    {
        if (_inp > 0)
            time += factor * Time.deltaTime;
        else if (_inp < 0)
            time -= factor * Time.deltaTime;

        t = TimeSpan.FromSeconds(time);

        if (t.TotalSeconds < 0)
            t = -t;

        string[] temp = t.ToString().Split(":"[0]);
        timeText.text = temp[0] + ":" + temp[1];       

        float tempTime = time;
        tempTime = Mathf.Abs(time);

        sunTransform.rotation = Quaternion.Euler(new Vector3((tempTime - 21600) / 86400 * 360, 0, 0));
        if (tempTime > 86400)
            time = 0;

        if (tempTime < 43200)
            intensity = 1 - (43200 - tempTime) / 43200;
        else
            intensity = 1 - (tempTime - 43200) / 43200;

        RenderSettings.fogColor = Color.Lerp(Color.black, Color.grey, intensity*intensity);
        sun.intensity = intensity;
    }
    void Update()
    {
        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        
        Cycle(vert);

        if (hor < 0)
            camPivY = -1;
        else if (hor > 0)
            camPivY = 1;
        else
            camPivY = 0;

        camPivot.Rotate(new Vector3(0, camPivY, 0));       
    }
    public void AssignTime()
    {
        print(HH.text);
        print(MM.text);
        time = float.Parse(HH.text) * 3600 + float.Parse(MM.text) * 60;
        Cycle(0);
    }
}
