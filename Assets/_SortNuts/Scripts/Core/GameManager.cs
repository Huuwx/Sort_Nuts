using UnityEngine;

public enum GameState
{
    Idle,
    NutPicked,
    CantPick
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    
    public BoardManager boardManager; // Kéo vào Inspector
    
    [SerializeField] ParticleSystem winParticle; // Hiệu ứng khi thắng level

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (boardManager == null)
        {
            boardManager = FindFirstObjectByType<BoardManager>();
        }
        boardManager.LoadLevel(0);
    }

    // Đổi sang level mới (ví dụ nút next)
    public void LoadNextLevel()
    {
        boardManager.LoadLevel(boardManager.currentLevelIndex + 1);
    }
}
