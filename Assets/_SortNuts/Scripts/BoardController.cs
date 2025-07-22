using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public BoltController[] bolts;
    private GameState state = GameState.Idle;

    private BoltController selectedBolt;      // Bolt đang được chọn lần đầu
    private List<NutController> pickedNuts;   // Các nut sẽ di chuyển

    void Start()
    {
        state = GameState.Idle;
        pickedNuts = new List<NutController>();
    }

    // Gọi hàm này từ sự kiện OnClick từng Bolt (ví dụ add trong Inspector hoặc script)
    public void OnBoltClicked(BoltController bolt)
    {
        if (state == GameState.Idle)
        {
            // Chọn nut trên cùng (và stack cùng màu bên dưới)
            pickedNuts = bolt.GetTopNutStack();
            if (pickedNuts.Count == 0) return;

            selectedBolt = bolt;
            StartCoroutine(MoveNutsUp(pickedNuts));  // Animation move lên trên và xoay

            state = GameState.NutPicked;
        }
        else if (state == GameState.NutPicked)
        {
            // Không cho click lại chính bolt đang cầm nut
            if (bolt == selectedBolt) return;

            NutController topNutTarget = bolt.GetTopNut();
            // Điều kiện hợp lệ: cột rỗng hoặc nut đầu cùng màu nut đang cầm
            bool canPlace = (topNutTarget == null)
                          || (topNutTarget.nutColor == pickedNuts[0].nutColor);

            if (canPlace)
            {
                // Di chuyển nut sang bolt mới, animation
                StartCoroutine(MoveNutsToBolt(pickedNuts, selectedBolt, bolt));
            }
            else
            {
                // Không hợp lệ, trả nut về lại vị trí cũ
                StartCoroutine(MoveNutsBack(pickedNuts, selectedBolt));
            }
            // Reset state
            state = GameState.Idle;
            selectedBolt = null;
            pickedNuts = new List<NutController>();
        }
    }

    // Animation move lên trên và xoay (có thể dùng DOTween hoặc Coroutine)
    IEnumerator MoveNutsUp(List<NutController> nuts)
    {
        foreach (var nut in nuts)
        {
            Vector3 upPos = nut.transform.position + new Vector3(0, 1f, 0);
            StartCoroutine(MoveAndRotateNut(nut.transform, upPos, 0.2f));
        }
        yield return new WaitForSeconds(0.25f);
    }

    // Animation move đến bolt target
    IEnumerator MoveNutsToBolt(List<NutController> nuts, BoltController from, BoltController to)
    {
        // Tính vị trí đích cho từng nut ở bolt mới
        int targetIndex = to.GetNuts().Count;
        for (int i = 0; i < nuts.Count; i++)
        {
            // Chuyển parent sang bolt mới
            nuts[i].transform.SetParent(to.nutsContainer);

            // Vị trí đích theo localPosition
            Vector3 targetPos = to.nutsContainer.position + new Vector3(0, targetIndex + i, 0);

            StartCoroutine(MoveAndRotateNut(nuts[i].transform, targetPos, 0.25f));
        }
        yield return new WaitForSeconds(0.3f);
    }

    // Trả nut về lại bolt cũ
    IEnumerator MoveNutsBack(List<NutController> nuts, BoltController from)
    {
        var oldNuts = from.GetNuts();
        int startIndex = oldNuts.Count;
        for (int i = 0; i < nuts.Count; i++)
        {
            nuts[i].transform.SetParent(from.nutsContainer);
            Vector3 backPos = from.nutsContainer.position + new Vector3(0, startIndex + i, 0);
            StartCoroutine(MoveAndRotateNut(nuts[i].transform, backPos, 0.25f));
        }
        yield return new WaitForSeconds(0.3f);
    }

    // Animation chuyển nut tới vị trí (và xoay)
    IEnumerator MoveAndRotateNut(Transform nut, Vector3 target, float time)
    {
        Vector3 startPos = nut.position;
        Quaternion startRot = nut.rotation;
        Quaternion targetRot = Quaternion.Euler(0, 360, 0) * startRot;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / time;
            nut.position = Vector3.Lerp(startPos, target, t);
            nut.rotation = Quaternion.Lerp(startRot, targetRot, t); // Xoay 1 vòng
            yield return null;
        }
        nut.position = target;
        nut.rotation = startRot; // Giữ hướng ban đầu
    }
}
