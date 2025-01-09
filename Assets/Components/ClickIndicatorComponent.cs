using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickIndicatorComponent : MonoBehaviour
{
	public void SelfDeactivate()
	{
		gameObject.SetActive(false);
	}
}
