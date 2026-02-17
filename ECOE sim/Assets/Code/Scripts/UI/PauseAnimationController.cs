using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseAnimationController : MonoBehaviour
{
    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup backgroundGroup;
    [SerializeField] private CanvasGroup pauseStickGroup;
    [SerializeField] private CanvasGroup settingsStickGroup;

    [Header("Elementos Visuales")]
    [SerializeField] private RectTransform grassImage;
    [SerializeField] private RectTransform settingsPanelRect;

    [Header("Configuración Animaciones")]
    [SerializeField] private float grassOffset = 300f;
    [SerializeField] private float settingsSlideOffset = 800f;
    [SerializeField] private float fadeDuration = 0.4f;
    [SerializeField] private float moveDuration = 0.5f;

    private Vector2 grassShowPos;
    private Vector2 grassHidePos;

    private Vector2 settingsShowPos;
    private Vector2 settingsHidePos;

    private void Awake()
    {
        if (grassImage)
        {
            grassShowPos = grassImage.anchoredPosition;
            grassHidePos = grassShowPos - new Vector2(0, grassOffset);
        }

        if (settingsPanelRect == null && settingsStickGroup != null)
        {
            settingsPanelRect = settingsStickGroup.GetComponent<RectTransform>();
        }

        if (settingsPanelRect)
        {
            settingsShowPos = settingsPanelRect.anchoredPosition;
            settingsHidePos = settingsShowPos + new Vector2(0, settingsSlideOffset);
        }
    }

    private void OnEnable()
    {
        ResetToHiddenState();
        PlayOpenSequence();
    }

    private void ResetToHiddenState()
    {
        if (backgroundGroup)
        {
            backgroundGroup.alpha = 0;
            backgroundGroup.interactable = false;
            backgroundGroup.blocksRaycasts = false;
        }

        if (pauseStickGroup)
        {
            pauseStickGroup.alpha = 0;
            pauseStickGroup.interactable = false;
            pauseStickGroup.blocksRaycasts = false;
        }

        if (settingsStickGroup)
        {
            settingsStickGroup.alpha = 0;
            settingsStickGroup.interactable = false;
            settingsStickGroup.blocksRaycasts = false;

            if (settingsPanelRect)
                settingsPanelRect.anchoredPosition = settingsHidePos;
        }

        if (grassImage) grassImage.anchoredPosition = grassHidePos;
    }

    public void PlayOpenSequence()
    {
        DOTween.Kill(backgroundGroup);
        if (pauseStickGroup) DOTween.Kill(pauseStickGroup);
        if (grassImage) DOTween.Kill(grassImage);

        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true);

        if (backgroundGroup) seq.Append(backgroundGroup.DOFade(1, fadeDuration));

        if (grassImage)
            seq.Join(grassImage.DOAnchorPos(grassShowPos, moveDuration).SetEase(Ease.OutBack));

        if (pauseStickGroup)
            seq.Join(pauseStickGroup.DOFade(1, fadeDuration));

        seq.OnComplete(() => {
            EnableInteraction(backgroundGroup, true);
            EnableInteraction(pauseStickGroup, true);
        });
    }

    public void SwitchToSettings()
    {
        if (!pauseStickGroup || !settingsStickGroup || !settingsPanelRect) return;

        EnableInteraction(backgroundGroup, false);
        EnableInteraction(pauseStickGroup, false);

        settingsStickGroup.gameObject.SetActive(true);
        settingsPanelRect.anchoredPosition = settingsHidePos;

        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true);

        seq.Append(pauseStickGroup.DOFade(0, fadeDuration));

        seq.Join(settingsStickGroup.DOFade(1, fadeDuration));

        seq.Join(settingsPanelRect.DOAnchorPos(settingsShowPos, moveDuration).SetEase(Ease.OutBack));

        seq.OnComplete(() => {
            EnableInteraction(backgroundGroup, true);
            EnableInteraction(settingsStickGroup, true);
        });
    }

    public void SwitchToMain()
    {
        if (!pauseStickGroup || !settingsStickGroup || !settingsPanelRect) return;

        EnableInteraction(backgroundGroup, false);
        EnableInteraction(settingsStickGroup, false);

        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true);

        seq.Append(settingsStickGroup.DOFade(0, fadeDuration));

        seq.Join(settingsPanelRect.DOAnchorPos(settingsHidePos, moveDuration).SetEase(Ease.InBack));

        seq.Join(pauseStickGroup.DOFade(1, fadeDuration));

        seq.OnComplete(() => {
            EnableInteraction(backgroundGroup, true);
            EnableInteraction(pauseStickGroup, true);
            settingsStickGroup.gameObject.SetActive(false);
        });
    }

    public void PlayCloseSequence(System.Action onCompleteAction)
    {
        EnableInteraction(backgroundGroup, false);
        EnableInteraction(pauseStickGroup, false);
        EnableInteraction(settingsStickGroup, false);

        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true);

        if (pauseStickGroup.alpha > 0) seq.Join(pauseStickGroup.DOFade(0, fadeDuration));

        if (settingsStickGroup.alpha > 0)
        {
            seq.Join(settingsStickGroup.DOFade(0, fadeDuration));
            if (settingsPanelRect)
                seq.Join(settingsPanelRect.DOAnchorPos(settingsHidePos, moveDuration).SetEase(Ease.InBack));
        }

        if (backgroundGroup) seq.Join(backgroundGroup.DOFade(0, fadeDuration));

        if (grassImage)
            seq.Join(grassImage.DOAnchorPos(grassHidePos, moveDuration).SetEase(Ease.InBack));

        seq.OnComplete(() => {
            onCompleteAction?.Invoke();
        });
    }

    private void EnableInteraction(CanvasGroup group, bool enable)
    {
        if (group)
        {
            group.interactable = enable;
            group.blocksRaycasts = enable;
        }
    }
}