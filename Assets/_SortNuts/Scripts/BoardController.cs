using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public BoltController[] bolts;
    private GameState state = GameState.Idle;

    private BoltController selectedBolt;      // Bolt đang được chọn lần đầu
    private List<NutController> pickedNuts;   // Các nut sẽ di chuyển
    private NutController pickedNut;

    private Vector3 pickedNutPosition;
    
    [SerializeField] int maxNutsPerBolt = 3;
    [SerializeField] int boltsNumberToComple = 3;
    

    void Start()
    {
        state = GameState.Idle;
        pickedNuts = new List<NutController>();
    }

    // Gọi hàm này từ sự kiện OnClick từng Bolt (ví dụ add trong Inspector hoặc script)
    public void OnBoltClicked(BoltController bolt)
    {
        if (bolt.IsCompleted(maxNutsPerBolt))
            return;
        
        if (state == GameState.Idle)
        {
            // Chọn nut trên cùng (và stack cùng màu bên dưới)
            pickedNut = bolt.GetTopNut();
            pickedNuts = bolt.GetTopNutStack();
            //if (pickedNuts.Count == 0) return;
            if (pickedNut == null) return;
            
            pickedNutPosition = pickedNut.transform.position;

            selectedBolt = bolt;
            StartCoroutine(MoveNutUp(pickedNut, bolt));  // Animation move lên trên và xoay

            state = GameState.NutPicked;
        }
        else if (state == GameState.NutPicked)
        {
            if (bolt == selectedBolt)
            {
                StartCoroutine(MoveNutBack(pickedNut, selectedBolt));
                state = GameState.CantPick;
                selectedBolt = null;
                pickedNut = null;
                pickedNuts = new List<NutController>();
                return;
            }

            NutController topNutTarget = bolt.GetTopNut();
            // Điều kiện hợp lệ: cột rỗng hoặc nut đầu cùng màu nut đang cầm va có đủ chỗ
            List<NutController> nutsInTargetBolt = bolt.GetNuts();
            bool canPlace = ((topNutTarget == null)
                          || (topNutTarget.nutColor == pickedNut.nutColor))
                          && (pickedNuts.Count + nutsInTargetBolt.Count <= maxNutsPerBolt);

            if (canPlace)
            {
                // Di chuyển nut sang bolt mới, animation
                StartCoroutine(MoveNutsToBolt(pickedNuts, selectedBolt,bolt));
                Debug.Log("DI chuyen");
            }
            else
            {
                // Không hợp lệ, trả nut về lại vị trí cũ
                StartCoroutine(MoveNutBack(pickedNut, selectedBolt));
            }
            // Reset state
            state = GameState.CantPick;
            selectedBolt = null;
            pickedNut = null;
            pickedNuts = new List<NutController>();
        }
    }

    // Animation move lên trên và xoay (có thể dùng DOTween hoặc Coroutine)
    IEnumerator MoveNutUp(NutController nut, BoltController bolt)
    {
        Vector3 upPos = bolt.transform.position + bolt.transform.up * 0.7f;
        StartCoroutine(MoveAndRotateNut(nut.transform, upPos, 0.2f));
        
        yield return new WaitForSeconds(0.25f);
    }

    // Animation move đến bolt target
    IEnumerator MoveNutsToBolt(List<NutController> nuts, BoltController from ,BoltController to)
    {
        // Tính vị trí đích cho từng nut ở bolt mới
        int targetIndex = to.GetNuts().Count;
        for (int i = nuts.Count - 1; i >= 0; i--)
        {
            // Chuyển parent sang bolt mới
            nuts[i].transform.SetParent(to.nutsContainer);
            
            float targetY = (targetIndex + 1) * 0.12f;
            targetIndex++;
            // Vị trí đích theo localPosition
            Vector3 targetPos = to.nutsContainer.position + new Vector3(0, targetY, 0);

            if (i < nuts.Count - 1)
            {
                yield return StartCoroutine(MoveNutUp(nuts[i], from));
            }
            yield return StartCoroutine(MoveAndRotateNut(nuts[i].transform, to.transform.position + to.transform.up * 0.7f, 0.25f));
            StartCoroutine(MoveAndRotateNut(nuts[i].transform, targetPos, 0.25f));
        }
        yield return new WaitForSeconds(0.3f);
        
        // Hiệu ứng bolt hoàn thành nếu có
        if (to.IsCompleted(maxNutsPerBolt))
        {
            Debug.Log("Bolt đã hoàn thành!");
            to.OnCompleted();
        }

        // Kiểm tra tất cả các bolt
        if (AllBoltsCompleted())
        {
            Debug.Log("TẤT CẢ BOLT ĐÃ HOÀN THÀNH! WIN GAME!");
            // TODO: Show win panel, khóa thao tác, v.v...
        }
        
        // Kiểm tra nut trên cùng của bolt cũ
        var remainNuts = from.GetNuts();
        if (remainNuts.Count > 0)
        {
            // Lấy nut trên cùng mới
            var newTopNut = remainNuts[remainNuts.Count - 1];
            // Nếu nut này là nut bí ẩn, thì Reveal
            if (newTopNut.isMysteryNut)
            {
                newTopNut.RevealColor();
                // Có thể thêm hiệu ứng (lật, sáng...)
            }
        }
        
        state = GameState.Idle;
    }

    // Trả nut về lại bolt cũ
    IEnumerator MoveNutBack(NutController nut, BoltController from)
    {
        nut.transform.SetParent(from.nutsContainer);
        StartCoroutine(MoveAndRotateNut(nut.transform, pickedNutPosition, 0.25f));
        yield return new WaitForSeconds(0.3f);
        state = GameState.Idle;
    }

    // Animation chuyển nut tới vị trí (và xoay)
    IEnumerator MoveAndRotateNut(Transform nut, Vector3 target, float time)
    {
        Vector3 startPos = nut.position;
        Quaternion startRot = nut.rotation;
        Quaternion targetRot = Quaternion.Euler(0, 180, 0) * startRot;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / time;
            nut.position = Vector3.Lerp(startPos, target, t);
            nut.rotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }
        nut.position = target;
        nut.rotation = startRot; // Giữ hướng ban đầu
    }

    // Kiểm tra tất cả các Bolt đã hoàn thành chưa
    public bool AllBoltsCompleted()
    {
        int count = 0;
        foreach (var bolt in bolts)
        {
            if (bolt.IsCompleted(maxNutsPerBolt))
                count++;
        }
        return count == boltsNumberToComple;
    }


    private void OnDrawGizmosSelected()
    {
        bolts = transform.GetComponentsInChildren<BoltController>();
    }
}
