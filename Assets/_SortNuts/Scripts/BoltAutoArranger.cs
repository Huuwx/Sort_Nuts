using UnityEngine;
using System.Collections.Generic;

public class BoltAutoArranger : MonoBehaviour
{
    public Camera mainCamera;          // Kéo MainCamera vào đây
    public float boltWidth = 1.0f;     // Chiều rộng thực tế của bolt (cỡ prefab, thường là 1.0)
    public float boltSpacingRatio = 0.4f; // Tỉ lệ spacing (40% chiều rộng bolt)
    public float topRowZ = 2.5f;       // Độ sâu của hàng trên (Z)
    public float botRowZ = 1.2f;       // Độ sâu của hàng dưới (Z)
    public float yLevel = 0f;          // Bolt đặt tại mặt phẳng Y=0

    /// <summary>
    /// Gọi hàm này sau khi Instantiate toàn bộ bolt cho level (trong BoardManager)
    /// </summary>
    public void ArrangeBoltsToFitCamera(List<GameObject> bolts)
    {
        if (mainCamera == null) mainCamera = Camera.main;
        int boltCount = bolts.Count;
        if (boltCount == 0) return;

        // Chia bolt thành 2 hàng
        int numTopRow = Mathf.CeilToInt(boltCount / 2f);
        int numBotRow = boltCount - numTopRow;

        // Tìm chiều rộng khung hình thực tế tại vị trí Z mong muốn
        float maxBoltsInRow = Mathf.Max(numTopRow, numBotRow);
        float targetZ = (numTopRow >= numBotRow) ? topRowZ : botRowZ; // Lấy Z hàng dài nhất

        float worldWidth = GetWorldWidthAtZ(targetZ, mainCamera);
        // Tính tổng width các bolt + spacing tại hàng đông nhất
        float neededWidth = maxBoltsInRow * boltWidth + (maxBoltsInRow - 1) * boltWidth * boltSpacingRatio;
        float scale = Mathf.Min(1f, worldWidth * 0.9f / neededWidth); // Giảm chút để không sát lề

        // --- Arrange Top Row ---
        float totalTopWidth = numTopRow * boltWidth * scale + (numTopRow - 1) * boltWidth * scale * boltSpacingRatio;
        float startXTop = -totalTopWidth / 2f + boltWidth * scale / 2f;

        for (int i = 0; i < numTopRow; i++)
        {
            float x = startXTop + i * boltWidth * scale * (1 + boltSpacingRatio);
            bolts[i].transform.position = new Vector3(x, yLevel, topRowZ);
            bolts[i].transform.localScale = Vector3.one * scale;
        }

        // --- Arrange Bottom Row ---
        if (numBotRow > 0)
        {
            float totalBotWidth = numBotRow * boltWidth * scale + (numBotRow - 1) * boltWidth * scale * boltSpacingRatio;
            float startXBot = -totalBotWidth / 2f + boltWidth * scale / 2f;

            for (int i = 0; i < numBotRow; i++)
            {
                float x = startXBot + i * boltWidth * scale * (1 + boltSpacingRatio);
                bolts[numTopRow + i].transform.position = new Vector3(x, yLevel, botRowZ);
                bolts[numTopRow + i].transform.localScale = Vector3.one * scale;
            }
        }
    }

    /// <summary>
    /// Tính chiều rộng khung hình tại vị trí Z (dành cho camera Perspective)
    /// </summary>
    public float GetWorldWidthAtZ(float z, Camera cam)
    {
        float y = 0; // hoặc Y nào đó, vì camera đang nhìn chéo (nhưng bolt nằm trên Y=0)
        Vector3 left = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, z - cam.transform.position.z));
        Vector3 right = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, z - cam.transform.position.z));
        return Mathf.Abs(right.x - left.x);
    }
}
