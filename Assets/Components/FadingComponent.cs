using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadingComponent : MonoBehaviour
{
    public float imageAlpha = 1;

    [SerializeField] bool ShouldFade = true;
    [SerializeField] float fadeDuration = 5f;

    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (ShouldFade)
            imageAlpha = Mathf.Lerp(imageAlpha, 0, Time.deltaTime * fadeDuration);
        else
            imageAlpha = Mathf.Lerp(imageAlpha, 1, Time.deltaTime * fadeDuration);

        image.color = new Color(image.color.r, image.color.g, image.color.b, imageAlpha);
    }

    public void Fade()
    {
        ShouldFade = true;
    }

    public void Unfade()
    {
        ShouldFade = false;
    }
}
