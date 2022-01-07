using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI nextLevel;
    [SerializeField] private TextMeshProUGUI percenThrowed;
    [SerializeField] private Image fillThrowed;
    public GameObject nextLevelPanel;
    private int remainingCount;
    private int percent;
    private float fillRatio;
    private void Start()
    {
        GameManager.Instance.nextLevelAction += ShowLevel;

        GameManager.Instance.showFillImage += ShowPercent;

        GameManager.Instance.levelCompleting += ShowNextLevelPanel;

        nextLevelPanel.SetActive(false);

        ShowLevel();
    }

    private void ShowLevel()
    {
        currentLevel.text = GameManager.Instance.level.ToString();
        nextLevel.text = (GameManager.Instance.level + 1).ToString();
    }
    private void FillImage()
    {
        fillRatio = percent / 100f;
        fillThrowed.fillAmount = fillRatio;
    }
    private void ShowPercent()
    {
        remainingCount = GameManager.Instance.totalThrowedCount - GameManager.Instance.cylinderInScene.Count;

        percent = (remainingCount * 100) / GameManager.Instance.totalThrowedCount;
        percenThrowed.text = "%" + percent;

        FillImage();
    }

    public void NextLevelButton()
    {
        nextLevelPanel.SetActive(false);

        GameManager.Instance.NextLevelGameManager();
    }

    private void ShowNextLevelPanel()
    {
        nextLevelPanel.SetActive(true);
    }
}
