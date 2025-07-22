using UnityEngine;

public class BoltClickHandler : MonoBehaviour
{
    public BoltController boltController;
    public BoardManager boardManager;

    void OnMouseDown() // hoặc OnPointerClick nếu dùng EventSystem
    {
        boardManager.OnBoltClicked(boltController);
    }
}

