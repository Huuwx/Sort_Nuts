using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoltClickHandler : MonoBehaviour, IPointerClickHandler
{
    public BoltController boltController;
    public BoardManager boardManager;

    // Hàm này sẽ tự động được gọi khi click hoặc tap vào GameObject này (có Collider hoặc UI Raycaster)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (boardManager != null && boltController != null)
        {
            boardManager.OnBoltClicked(boltController);
        }
    }

    private void OnDrawGizmosSelected()
    {
        boltController = GetComponentInParent<BoltController>();
        boardManager = FindAnyObjectByType<BoardManager>();
    }
}