using System;
using UnityEngine;

public class NutController : MonoBehaviour
{
    public Color nutColor;

    private void Start()
    {
        nutColor = transform.GetChild(0).GetComponent<Renderer>().material.color;
    }
}
