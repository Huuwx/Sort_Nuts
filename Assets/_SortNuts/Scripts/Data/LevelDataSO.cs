using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "SortNuts/Level Data", order = 1)]
public class LevelDataSO : ScriptableObject
{
    public List<BoltData> bolts = new List<BoltData>();
}