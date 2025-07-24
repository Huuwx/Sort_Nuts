using System;
using UnityEngine;

public class NutController : MonoBehaviour
{
    public Color nutColor;
    public bool isMysteryNut = false; // có phải nut "?"
    public GameObject mysteryMark; // GameObject chứa dấu "?" (kéo vào Inspector)
    public Renderer bodyRenderer;  // MeshRenderer phần thân nut (kéo vào Inspector)

    private void Start()
    {
        nutColor = transform.GetChild(0).GetComponent<Renderer>().material.color;
        if(isMysteryNut)
            SetMystery(true);
        else
            RevealColor();
    }
    
    // Hiện/ẩn màu thật hoặc dấu ?
    public void SetMystery(bool isMystery)
    {
        isMysteryNut = isMystery;
        if (mysteryMark != null)
            mysteryMark.SetActive(isMysteryNut);
        if (bodyRenderer != null)
            bodyRenderer.enabled = !isMysteryNut;
    }

    // Hiện màu thật, ẩn dấu ?
    public void RevealColor()
    {
        isMysteryNut = false;
        if (mysteryMark != null)
            mysteryMark.SetActive(false);
        if (bodyRenderer != null)
            bodyRenderer.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        // nutColor = transform.GetChild(0).GetComponent<Renderer>().material.color;
        // mysteryMark = transform.GetChild(1).gameObject;
        // bodyRenderer = transform.GetChild(0).GetComponent<Renderer>();
    }
}
