using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "SortNuts/Level Database", order = 2)]
public class LevelDatabaseSO : ScriptableObject
{
    public List<LevelDataSO> levels = new List<LevelDataSO>();
}