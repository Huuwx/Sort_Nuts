using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoltClickHandler : MonoBehaviour, IPointerClickHandler
{
    public BoltController boltController;
    public BoardManager boardManager;

    private void Start()
    {
        boltController = GetComponentInParent<BoltController>();
        boardManager = FindAnyObjectByType<BoardManager>();
    }

    // Hàm này sẽ tự động được gọi khi click hoặc tap vào GameObject này (có Collider hoặc UI Raycaster)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (boardManager != null && boltController != null)
        {
            Debug.Log("Bolt clicked: " + gameObject.name);
            boardManager.OnBoltClicked(boltController);
        }
        else
        {
            Debug.Log("Bolt not clicked");
        }
    }
}