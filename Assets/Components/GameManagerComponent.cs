using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerComponent : MonoBehaviour
{
    public GameObject winningPanel;
    HostageManager hostageManager;
    public TextMeshProUGUI finalElapsedTimeIndicator;
    // Start is called before the first frame update
    void Start()
    {
        hostageManager = FindObjectOfType<HostageManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hostageManager.HasWon())
        {
            winningPanel.SetActive(true);
            finalElapsedTimeIndicator.text = $"Time Elapsed: {hostageManager.finalElapsedTime}";
        }
    }
}
