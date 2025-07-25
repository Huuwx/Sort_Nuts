using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoltData
{
    public List<NutData> nuts = new List<NutData>(); // Danh sách nut trên bolt này (từ dưới lên trên)
}