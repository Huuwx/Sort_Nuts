using UnityEngine;

public enum GameState
{
    Idle,
    NutPicked,
    CantPick
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern
    
    public BoardManager boardManager; // Kéo vào Inspector

    private void Awake()
    {
        Instance = this; // Gán instance cho singleton
    }
    
    void Start()
    {
        if (boardManager == null)
        {
            boardManager = FindObjectOfType<BoardManager>();
        }
        boardManager.LoadLevel(0);
    }

    // Đổi sang level mới (ví dụ nút next)
    public void LoadNextLevel()
    {
        boardManager.LoadLevel(boardManager.currentLevelIndex + 1);
    }
}
