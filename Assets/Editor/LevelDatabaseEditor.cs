using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelDatabaseSO))]
public class LevelDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelDatabaseSO db = (LevelDatabaseSO)target;

        GUILayout.Space(15);
        GUILayout.Label("Level Tools", EditorStyles.boldLabel);

        // Thêm nút tạo level mới
        if (GUILayout.Button("Add New Level"))
        {
            LevelDataSO newLevel = CreateLevelAsset(db.levels.Count + 1);
            db.levels.Add(newLevel);
            EditorUtility.SetDirty(db);
        }

        // Clone/copy level hiện tại
        if (db.levels.Count > 0)
        {
            GUILayout.Space(5);
            GUILayout.Label("Clone/Copy Last Level");

            if (GUILayout.Button("Clone Last Level"))
            {
                LevelDataSO lastLevel = db.levels[db.levels.Count - 1];
                LevelDataSO cloned = CloneLevel(lastLevel, db.levels.Count + 1);
                db.levels.Add(cloned);
                EditorUtility.SetDirty(db);
            }
        }
    }

    // Tạo asset LevelDataSO mới
    private LevelDataSO CreateLevelAsset(int index)
    {
        var newLevel = ScriptableObject.CreateInstance<LevelDataSO>();
        string path = $"Assets/_SortNuts/ScriptableObject/Levels/Level_{index:D2}.asset";
        AssetDatabase.CreateAsset(newLevel, path);
        AssetDatabase.SaveAssets();
        return newLevel;
    }

    // Clone một LevelDataSO sang asset mới
    private LevelDataSO CloneLevel(LevelDataSO original, int index)
    {
        var cloned = ScriptableObject.CreateInstance<LevelDataSO>();
        // Clone deep tất cả bolt/nut data
        foreach (var bolt in original.bolts)
        {
            var newBolt = new BoltData();
            foreach (var nut in bolt.nuts)
            {
                newBolt.nuts.Add(new NutData { material = nut.material, isMysteryNut = nut.isMysteryNut });
            }
            cloned.bolts.Add(newBolt);
        }
        string path = $"Assets/_SortNuts/ScriptableObject/Levels/Level_{index:D2}.asset";
        AssetDatabase.CreateAsset(cloned, path);
        AssetDatabase.SaveAssets();
        return cloned;
    }
}
