using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HostageManager : MonoBehaviour
{
    public string finalElapsedTime;

    int numberOfHostagesFreed, totalHostageNumber;

    UITimeElapseComponent timeElapseComponent;

    void Start()
    {
        numberOfHostagesFreed = 0;
        totalHostageNumber = FindObjectsOfType<HostageComponent>().Count();
        timeElapseComponent = FindObjectOfType<UITimeElapseComponent>();
    }

    public void IncrementHostageFreed()
    {
        numberOfHostagesFreed++;
        if (HasWon())
            finalElapsedTime = timeElapseComponent.elapsedTimeTitle;
    }

    public bool HasWon()
    {
        return totalHostageNumber == numberOfHostagesFreed;
    }

    public string GetUIText()
    {
        return $"{numberOfHostagesFreed}/{totalHostageNumber}";
    }
}
