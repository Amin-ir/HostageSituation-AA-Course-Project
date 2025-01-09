using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinComponent : MonoBehaviour
{
    AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

	private void OnCollisionEnter(Collision collision)
	{
        audio.Play();
	}
}
