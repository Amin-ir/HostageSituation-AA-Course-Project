using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivatingComponent : MonoBehaviour
{
    private void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }
}
