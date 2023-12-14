using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;

    [SerializeField]
    private Button replayButton;

    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Slider time;
    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Transform achivementCount;

    private void Start()
    {
        levelText.text = "LEVEL " + (LevelManager.instance.currentLevelIndex + 1);
    }


    public void ShowWinPanel()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        replayButton.interactable = false;
        homeButton.interactable = false;

        Transform achivementContainer = winPanel.GetChild(0);
        StartCoroutine(SetAchive(achivementContainer));
    }

    private IEnumerator SetAchive(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            if (i < GameManager.instance.achivement)
            {
                container.GetChild(i).GetChild(0).gameObject.SetActive(false);

                container.GetChild(i).localScale = Vector3.zero;
                container.GetChild(i).DOScale(1, .3f);
                yield return new WaitForSecondsRealtime(.3f);
            }
        }
    }

    public void ShowLosePanel()
    {
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        replayButton.interactable = false;
        homeButton.interactable = false;
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }


    public void UpdateTime(float timeLeft, float timeLimit)
    {
        time.value = timeLeft/ timeLimit;
        int achivement = (int)((timeLeft / timeLimit) * (3 + 1));

        if(achivementCount.childCount > achivement)
        {
            Transform star = achivementCount.GetChild(achivement).GetChild(0);
            star.gameObject.SetActive(true);
            star.DOLocalJump(star.localPosition + new Vector3(20, 0, 0), 20, 1, .3f);
            star.DORotate(new Vector3(0, 0, 360), .5f, RotateMode.FastBeyond360);
        }

    }
}
