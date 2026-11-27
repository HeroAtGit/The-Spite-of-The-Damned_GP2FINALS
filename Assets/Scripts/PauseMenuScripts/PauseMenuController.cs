using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class PauseMenuController : MonoBehaviour
{
    [Header("Responsive Positions")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform openTarget;
    [SerializeField] private RectTransform closedTarget;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup overlay;
    [SerializeField] private CanvasGroup menuGroup;
    [SerializeField] private CanvasGroup[] buttonGroups;

    [Header("Animated Controls")]
    [SerializeField] private Transform[] buttonTransforms;

    [Header("Timing")]
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float stagger = 0.08f;

    private Sequence currentTween;
    private bool isOpen;

    private void Awake()
    {
        Time.timeScale = 1f;
        panel.position = closedTarget.position;
        overlay.alpha = 0f;
        overlay.blocksRaycasts = false;
        menuGroup.alpha = 0f;
        menuGroup.interactable = false;
        menuGroup.blocksRaycasts = false;
        PrepareButtons();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isOpen)
                ClosePauseMenu();
            else
                OpenPauseMenu();
        }
    }

    public void OpenPauseMenu()
    {
        isOpen = true;

        currentTween?.Kill();

        PrepareButtons();

        panel.position = closedTarget.position;
        overlay.alpha = 0f;
        menuGroup.alpha = 0f;
        menuGroup.interactable = false;
        menuGroup.blocksRaycasts = true;
        overlay.blocksRaycasts = true;
        Time.timeScale = 0f;

        currentTween = DOTween.Sequence().SetUpdate(true);

        currentTween.Append(overlay.DOFade(0.72f, 0.25f));

        currentTween.Join(panel.DOMove(openTarget.position, duration).SetEase(Ease.OutCubic));

        currentTween.Join(menuGroup.DOFade(1f, duration));

        currentTween.Insert(0.18f, CreateButtonIntro());

        currentTween.OnComplete(() => menuGroup.interactable = true);

        currentTween.OnKill(() => currentTween = null);
    }
    public void ClosePauseMenu()
    {
        isOpen = false;

        currentTween?.Kill();

        menuGroup.interactable = false;

        currentTween = DOTween.Sequence().SetUpdate(true);

        currentTween.Append(panel.DOMove(closedTarget.position, duration).SetEase(Ease.InCubic));

        currentTween.Join(menuGroup.DOFade(0f, duration));

        currentTween.Join(overlay.DOFade(0f, duration));

        currentTween.OnComplete(() =>
        {
            menuGroup.blocksRaycasts = false;
            overlay.blocksRaycasts = false;
            Time.timeScale = 1f;
        });

        currentTween.OnKill(() => currentTween = null);
    }

    public void GoToOptions()
    {
    }
        

    public void GoToMainMenu()
    {
    }

    private Sequence CreateButtonIntro()
    {
        Sequence buttons = DOTween.Sequence();

        for (int i = 0; i < buttonTransforms.Length; i++)
        {
            float time = i * stagger;

            buttons.Insert(time,
                buttonTransforms[i]
                .DOScale(0.2f, 0.2f)
                .SetEase(Ease.OutBack));

            buttons.Insert(time, buttonGroups[i].DOFade(1f, 0.2f));
        }

        return buttons;
    }

    private void PrepareButtons()
    {
        for (int i = 0; i < buttonTransforms.Length; i++)
        {
            buttonTransforms[i].localScale = Vector3.one * 0.85f;
            buttonGroups[i].alpha = 0f;
        }
    }
}
