using UnityEngine;
using System.Collections.Generic;

public class BoltAutoArranger : MonoBehaviour
{
    public Camera mainCamera;
    public float boltWidth = 0.2f;
    public float boltSpacingRatio = 1.5f;
    public float yLevel = 0f;

    // Z-positions cho từng hàng (0 = hàng trên cùng)
    private float[] rowZOffsets = new float[] { 1.5f, 0.5f, -0.5f };

    public void ArrangeBoltsToFitCamera(List<GameObject> bolts)
    {
        if (mainCamera == null) mainCamera = Camera.main;
        int boltCount = bolts.Count;
        if (boltCount == 0) return;

        int rowCount = boltCount >= 9 ? 3 : 2;

        if (rowCount == 2)
        {
            rowZOffsets = new float[] { 0.5f, -0.5f};
        }
        else if (rowCount == 3)
        {
            rowZOffsets = new float[] { 1.5f, 0.5f, -0.5f };
        }

        // Chia đều bolt vào các hàng
        int[] boltsPerRow = new int[rowCount];
        for (int i = 0; i < boltCount; i++)
        {
            boltsPerRow[i % rowCount]++;
        }

        // Tìm hàng có nhiều bolt nhất để làm cơ sở scale
        int maxBoltsInRow = 0;
        foreach (int count in boltsPerRow) maxBoltsInRow = Mathf.Max(maxBoltsInRow, count);

        float targetZ = rowZOffsets[0]; // hàng trên cùng
        float worldWidth = GetWorldWidthAtZ(targetZ, mainCamera);
        float neededWidth = maxBoltsInRow * boltWidth + (maxBoltsInRow - 1) * boltWidth * boltSpacingRatio;
        float scale = Mathf.Min(1f, worldWidth * 0.9f / neededWidth);

        int boltIndex = 0;
        for (int row = 0; row < rowCount; row++)
        {
            int boltsInThisRow = boltsPerRow[row];
            float totalWidth = boltsInThisRow * boltWidth * scale + (boltsInThisRow - 1) * boltWidth * scale * boltSpacingRatio;
            float startX = -totalWidth / 2f + boltWidth * scale / 2f;
            float z = (row < rowZOffsets.Length) ? rowZOffsets[row] : 0f;

            for (int i = 0; i < boltsInThisRow; i++)
            {
                if (boltIndex >= bolts.Count) break;

                float x = startX + i * boltWidth * scale * (1 + boltSpacingRatio);
                bolts[boltIndex].transform.position = new Vector3(x, yLevel, z);
                bolts[boltIndex].transform.localScale = Vector3.one * scale;
                boltIndex++;
            }
        }
    }

    public float GetWorldWidthAtZ(float z, Camera cam)
    {
        Vector3 left = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, z - cam.transform.position.z));
        Vector3 right = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, z - cam.transform.position.z));
        return Mathf.Abs(right.x - left.x);
    }
}
