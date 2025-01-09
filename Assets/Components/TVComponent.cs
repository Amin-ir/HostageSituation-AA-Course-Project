using Assets.Resources.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TVComponent : MonoBehaviour, IInteractable
{
    [SerializeField] private Material TvOnScreenMaterial;

    bool IsOn = false;
    AudioSource audio;
    MeshRenderer tvScreen;
    Light light;
	BillboardComponent interactionBillboard;
	InteractionComponent interactor;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        tvScreen = GetComponentsInChildren<MeshRenderer>()
                  .First(mesh => mesh.materials.Length == 0);
        light = GetComponent<Light>();
		interactionBillboard = GetComponentInChildren<BillboardComponent>();
		interactor = FindObjectOfType<InteractionComponent>();
		HideInteractionButton();
    }

	void Update()
	{
		if (Vector3.Distance(transform.position, interactor.transform.position) > interactor.DistanceToInteractable)
			HideInteractionButton();
	}

	public void Interact()
	{
		if (IsOn)
			TurnOff();
		else
			TurnOn();
	}

	void TurnOn()
	{
		audio.Play();
		tvScreen.materials = new Material[] { TvOnScreenMaterial };
		light.enabled = true;
		IsOn = true;
	}

	void TurnOff()
	{
		tvScreen.materials = new Material[] { };
		audio.Stop();
		light.enabled = false;
		IsOn = false;
	}

	public void DisplayInteractionButton()
	{
		interactionBillboard.gameObject.SetActive(true);
	}

	public void HideInteractionButton()
	{
		interactionBillboard.gameObject.SetActive(false);
	}
}
