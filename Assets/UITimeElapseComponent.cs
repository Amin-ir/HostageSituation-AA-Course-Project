using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITimeElapseComponent : MonoBehaviour
{
    public string elapsedTimeTitle;

    TextMeshProUGUI text;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTime;
        string minutes = ((int)(elapsedTime / 60)).ToString("00");
        string seconds = ((int)(elapsedTime % 60)).ToString("00");
        elapsedTimeTitle = $"{minutes}:{seconds}";
        text.text = elapsedTimeTitle;
    }
}
