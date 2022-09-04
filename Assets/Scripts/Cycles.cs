using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] Slider pan_View;

    bool is_Clock_Rotating = false;
    int external_Clock_Input = 0;
    float initialY_rot;

    Vector2 input;

    public Light sun;
    [SerializeField] int factor;
    private void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        sunTransform = sun.transform;
        Cycle(0);
        Cycle(0);
        initialY_rot = camPivot.transform.rotation.y;
    }
    void Cycle(float _inp)
    {
        if (_inp > 0)
            time += factor * Time.deltaTime * _inp;
        else if (_inp < 0)
            time -= factor * Time.deltaTime * _inp;

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

        Take_Input();
        Cycle(input.y);

        /*        if (input.x < 0)
                    camPivY = -1;
                else if (input.x > 0)
                    camPivY = 1;
                else
                    camPivY = 0;*/

        camPivot.transform.rotation = Quaternion.Euler(0, initialY_rot + input.x, 0);  
    }
    public void AssignTime()
    {
        print(HH.text);
        print(MM.text);
        time = float.Parse(HH.text) * 3600 + float.Parse(MM.text) * 60;
        Cycle(0);
    }

    public void Start_Clock()
    {
        is_Clock_Rotating = true;
    }

    public void Stop_Clock()
    {
        is_Clock_Rotating = false;
    }

    void Take_Input()
    {
        if(is_Clock_Rotating == true)
        {
            input.y = 1;
        }
        else
        {
            input.y = external_Clock_Input;
            if(external_Clock_Input != 0)
            {
                external_Clock_Input = 0;
            }
        }
        input.x = pan_View.value * 360;
    }

    public void Forward_External_Input()
    {
        external_Clock_Input = 300;
    }

    public void Backward_External_Input()
    {
        external_Clock_Input = -300;
    }
}
