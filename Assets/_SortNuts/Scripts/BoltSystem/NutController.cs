using System;
using UnityEngine;

public class NutController : MonoBehaviour
{
    public Color nutColor;
    public Color realColor; // Màu thật của nut, dùng để so sánh với màu của bolt
    public Renderer nutRen;
    public bool isMysteryNut = false; // có phải nut "?"
    [SerializeField] GameObject mysteryMark; // GameObject chứa dấu "?" (kéo vào Inspector)
    [SerializeField] Renderer bodyRenderer;  // MeshRenderer phần thân nut (kéo vào Inspector)

    // private void Start()
    // {
    //     nutRen = transform.GetChild(0).GetComponent<Renderer>();
    //     mysteryMark = transform.GetChild(1).gameObject;
    //     bodyRenderer = transform.GetChild(0).GetComponent<Renderer>();
    //     if (isMysteryNut)
    //     {
    //         SetMystery(true);
    //     }
    //     else
    //     {
    //         SetMystery(false);  
    //     }
    // }

    public void SetUp()
    {
        SetNutRen();
        mysteryMark = transform.GetChild(1).gameObject;
        bodyRenderer = transform.GetChild(0).GetComponent<Renderer>();
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    private void SetNutRen()
    {
        nutRen = transform.GetChild(0).GetComponent<Renderer>();
    }

    // Hiện/ẩn màu thật hoặc dấu ?
    public void SetMystery(bool isMystery)
    {
        isMysteryNut = isMystery;
        if (mysteryMark != null)
            mysteryMark.SetActive(isMysteryNut);
        if (bodyRenderer != null)
            bodyRenderer.enabled = !isMysteryNut;
        
        if (!isMysteryNut)
            nutColor = transform.GetChild(0).GetComponent<Renderer>().material.color;
        else
            nutColor = transform.GetChild(1).GetComponent<Renderer>().material.color;
        
        realColor = transform.GetChild(0).GetComponent<Renderer>().material.color;
        
    }
}
