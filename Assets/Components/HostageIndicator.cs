using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HostageIndicator : MonoBehaviour
{
    TextMeshProUGUI textUI;
    HostageManager hostageManager;

    void Start()
    {
        textUI = GetComponentInChildren<TextMeshProUGUI>();
        hostageManager = FindObjectOfType<HostageManager>();
    }

    void Update()
    {
        textUI.text = hostageManager.GetUIText();
    }
}
