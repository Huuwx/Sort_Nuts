using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "SortNuts/Level Data", order = 1)]
public class LevelDataSO : ScriptableObject
{
    public int maxNutsPerBolt = 3; // Số lượng nut tối đa trên mỗi bolt
    public int boltsNumberToComplete = 3; // Số lượng bolt cần hoàn thành để hoàn thành level
    public List<BoltData> bolts = new List<BoltData>();
}