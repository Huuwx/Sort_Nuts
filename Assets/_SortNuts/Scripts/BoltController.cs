using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    public Transform nutsContainer; // Drag object Nuts vào đây trên Inspector

    // Lấy danh sách Nut trên trụ từ dưới lên trên
    public List<NutController> GetNuts()
    {
        List<NutController> nuts = new List<NutController>();
        foreach (Transform child in nutsContainer)
        {
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

    // Kiểm tra trụ đã đầy chưa (tối đa số nut, vd 4)
    public bool IsFull(int maxNuts)
    {
        return GetNuts().Count >= maxNuts;
    }

    // Kiểm tra trụ rỗng
    public bool IsEmpty()
    {
        return GetNuts().Count == 0;
    }

    // Thêm Nut vào trụ (chuyển nut về nutsContainer)
    public void AddNut(NutController nut)
    {
        nut.transform.SetParent(nutsContainer);
        // Đặt vị trí localPosition.y kế tiếp (cao hơn nut trên cùng 1 đơn vị)
        var nuts = GetNuts();
        float nextY = nuts.Count > 1 ? nuts[nuts.Count - 2].transform.localPosition.y + 1f : 0f;
        nut.transform.localPosition = new Vector3(0, nextY, 0);
    }

    // Xóa Nut trên cùng (pop)
    public NutController RemoveTopNut()
    {
        NutController topNut = GetTopNut();
        if (topNut != null)
        {
            topNut.transform.SetParent(null);
        }
        return topNut;
    }
    
    public List<NutController> GetTopNutStack()
    {
        List<NutController> nuts = GetNutsSortedByY();
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

// Đảm bảo trả nuts từ thấp đến cao (theo localPosition.y)
    public List<NutController> GetNutsSortedByY()
    {
        List<NutController> nuts = new List<NutController>();
        foreach (Transform child in nutsContainer)
        {
            NutController nut = child.GetComponent<NutController>();
            if (nut != null)
                nuts.Add(nut);
        }
        nuts.Sort((a, b) => a.transform.localPosition.y.CompareTo(b.transform.localPosition.y));
        return nuts;
    }

}
