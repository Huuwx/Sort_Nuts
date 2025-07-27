using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }
    
    [SerializeField] private GameObject levelTextObject; // GameObject chứa TextMeshProUGUI để hiển thị level
    [SerializeField] private TextMeshProUGUI levelText; // Hiển thị level hiện tại
    [SerializeField] private GameObject panelLevelComplete; // Panel hiển thị khi hoàn thành level

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
    
    public void UpdateLevelText(int level)
    {
        if (levelText != null)
        {
            levelText.text = "Level " + (level + 1).ToString();
        }
        else
        {
            Debug.LogWarning("Level Text is not assigned in UIManager");
        }
    }
    
    public void ShowLevelCompletePanel()
    {
        if (panelLevelComplete != null)
        {
            panelLevelComplete.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Panel Level Complete is not assigned in UIManager");
        }
    }
    
    public void HideLevelCompletePanel()
    {
        if (panelLevelComplete != null)
        {
            panelLevelComplete.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Panel Level Complete is not assigned in UIManager");
        }
    }
}
