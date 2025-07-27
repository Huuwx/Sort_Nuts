using System;
using UnityEngine;

public class TextController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void NextLevel()
    {
        GameManager.Instance.LoadNextLevel();
        UIManager.Instance.HideLevelCompletePanel();
    }
}
