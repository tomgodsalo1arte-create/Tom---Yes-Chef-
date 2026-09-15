using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreparationProgressUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Image progressFill;
    [SerializeField] private TMP_Text remainingTimeText;

    public void Show(float remainingTime, float totalTime)
    {
        if (root != null)
            root.SetActive(true);

        float progress = totalTime <= 0f
            ? 0f
            : remainingTime / totalTime;

        progressFill.fillAmount = Mathf.Clamp01(progress);

        int seconds = Mathf.CeilToInt(remainingTime);
        remainingTimeText.text = $"{seconds}s";
    }

    public void Hide()
    {
        if (root != null)
            root.SetActive(false);
    }
}
