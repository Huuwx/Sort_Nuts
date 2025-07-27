using System;
using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    public Transform nutsContainer; // Drag object Nuts vào đây trên Inspector
    public int maxNutsPerBolt = 3;

    [SerializeField] private GameObject screw;
    
    [SerializeField] Transform boltObj;
    
    [SerializeField] ParticleSystem boltParticles;
    
    private void Awake()
    {
        boltObj = transform.GetChild(0);
        nutsContainer = transform.GetChild(1);
        boltParticles = transform.GetChild(3).GetComponent<ParticleSystem>();
        boltParticles.Stop();
        //screw = transform.GetChild(2).gameObject;
    }

    private void OnEnable()
    {
        //screw.SetActive(false);
    }
    
    public void SetMaxNuts(int maxNuts)
    {
        maxNutsPerBolt = maxNuts;
        boltObj.localScale = new Vector3(boltObj.localScale.x, 0.14f, boltObj.localScale.z);
        if (maxNutsPerBolt > 3)
        {
            float height = 0.14f + (maxNutsPerBolt - 3) * 0.04f; // Mỗi nut thêm cao thêm 0.04
            boltObj.localScale = new Vector3(boltObj.localScale.x, height, boltObj.localScale.z);
        }
    }

    // Lấy danh sách Nut trên trụ từ dưới lên trên
    public List<NutController> GetNuts()
    {
        List<NutController> nuts = new List<NutController>();
        foreach (Transform child in nutsContainer)
        {
            if (child == null) continue; // Không duyệt nếu child đã bị Destroy
            
            NutController nut = child.GetComponent<NutController>();
            if (nut != null)
                nuts.Add(nut);
        }
        // Sắp xếp theo LocalPosition.y (thấp đến cao)
        nuts.Sort((a, b) => a.transform.localPosition.y.CompareTo(b.transform.localPosition.y));
        return nuts;
    }

    // Lấy Nut trên cùng (trả về null nếu trụ rỗng)
    public NutController GetTopNut()
    {
        var nuts = GetNuts();
        if (nuts.Count == 0)
            return null;

        // Nut trên cùng là nut có localPosition.y cao nhất
        NutController topNut = nuts[0];
        foreach (var nut in nuts)
        {
            if (nut.transform.localPosition.y > topNut.transform.localPosition.y)
                topNut = nut;
        }
        return topNut;
    }
    
    public List<NutController> GetTopNutStack()
    {
        List<NutController> nuts = GetNuts();
        List<NutController> result = new List<NutController>();
        if (nuts.Count == 0) return result;

        Color topColor = nuts[nuts.Count - 1].nutColor; // nut trên cùng
        // Lấy tất cả nut trên cùng màu
        for (int i = nuts.Count - 1; i >= 0; i--)
        {
            if (nuts[i].nutColor == topColor)
                result.Insert(0, nuts[i]);
            else
                break;
        }
        return result;
    }
    
    public List<NutController> GetTopNutMysteryStack()
    {
        List<NutController> nuts = GetNuts();
        List<NutController> result = new List<NutController>();
        if (nuts.Count == 0) return result;

        Color topColor = nuts[nuts.Count - 1].realColor; // nut trên cùng
        // Lấy tất cả nut trên cùng màu
        for (int i = nuts.Count - 1; i >= 0; i--)
        {
            if (nuts[i].realColor == topColor)
                result.Insert(0, nuts[i]);
            else
                break;
        }
        return result;
    }
    
    public bool IsCompleted(int maxNuts)
    {
        List<NutController> nuts = GetNuts();
        if (nuts.Count != maxNuts) return false;
        if (nuts.Count == 0) return false;

        Color color = nuts[0].nutColor;
        for (int i = 1; i < nuts.Count; i++)
        {
            if (nuts[i].nutColor != color)
                return false;
        }
        return true;
    }

    public void OnCompleted()
    {
        //screw.SetActive(true);
        boltParticles.Play();
    }
}
