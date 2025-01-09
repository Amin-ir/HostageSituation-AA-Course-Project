using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonComponent : MonoBehaviour
{
    bool IsMenuOpen = false;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();   
    }

    public void ToggleMenu()
    {
        if (IsMenuOpen) CloseMenu();
        else OpenMenu();   
    }

    private void OpenMenu()
    {
        animator.Play("PauseOpen");
        IsMenuOpen = true;
    }

    private void CloseMenu()
    {
        animator.Play("PauseClose");
        IsMenuOpen = false;
    }

    public void FreezeGame()
    {
        Time.timeScale = 0f;
    }

    public void PlayGame()
    {
        Time.timeScale = 1.0f;
    }
}
