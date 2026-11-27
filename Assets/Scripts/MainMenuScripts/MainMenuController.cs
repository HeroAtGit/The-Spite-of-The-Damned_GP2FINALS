using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;

public class MainMenuController : MonoBehaviour
{
    [Header("Responsive Positions")]
    [SerializeField] RectTransform SettingsPanel;
    [SerializeField] RectTransform CreditsPanel;
    [SerializeField] RectTransform openTarget;
    [SerializeField] RectTransform[] closedTarget;

    [Header("Canvas Groups")]
    [SerializeField] CanvasGroup Overlay;
    [SerializeField] CanvasGroup SettingsGroup;
    [SerializeField] CanvasGroup CreditsGroup;

    [Header("Timing")]
    [SerializeField] float duration = 0.4f;

    private Sequence currentTween;
    private bool isSettingsOpen;
    private bool isCreditsOpen;

    private void Awake()
    {
        isSettingsOpen = false;
        isCreditsOpen = false;

        SettingsGroup.alpha = 0;
        SettingsGroup.interactable = false;
        SettingsGroup.blocksRaycasts = false;

        CreditsGroup.alpha = 0;
        CreditsGroup.interactable = false;
        CreditsGroup.blocksRaycasts = false;

        Overlay.alpha = 0;
        Overlay.blocksRaycasts = false;
        Overlay.interactable = false;

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isSettingsOpen)
            {
                CloseSettingsPanel();
            }
            else if (isCreditsOpen)
            {
                CloseCreditsPanel();
            }
        }
    }

    public void OpenSettingsPanel()
    {
        isSettingsOpen = true;
        currentTween?.Kill();

        SettingsPanel.position = closedTarget[0].position;
        SettingsGroup.alpha = 0f;
        Overlay.alpha = 0f;

        SettingsGroup.blocksRaycasts = true;
        SettingsGroup.interactable = false;
        Overlay.blocksRaycasts = true;

        currentTween = DOTween.Sequence().SetUpdate(true);
        currentTween.Append(Overlay.DOFade(0.9f, duration));
        currentTween.Join(SettingsPanel.DOMove(openTarget.position, duration).SetEase(Ease.OutCubic));
        currentTween.Join(SettingsGroup.DOFade(1f, duration));

        currentTween.OnComplete(() => SettingsGroup.interactable = true);
        currentTween.OnKill(() => currentTween = null);
        Time.timeScale = 0f;
    }

    public void OpenCreditsPanel()
    {
        isCreditsOpen = true;
        currentTween?.Kill();

        ScrollRect scrollRect = CreditsPanel.GetComponentInChildren<ScrollRect>(true);
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.StopMovement();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        CreditsPanel.position = closedTarget[1].position;
        CreditsGroup.alpha = 0f;
        Overlay.alpha = 0f;

        CreditsGroup.blocksRaycasts = true;
        CreditsGroup.interactable = false;
        Overlay.blocksRaycasts = true;

        currentTween = DOTween.Sequence().SetUpdate(true);
        currentTween.Append(Overlay.DOFade(0.9f, duration));
        currentTween.Join(CreditsPanel.DOMove(openTarget.position, duration).SetEase(Ease.OutCubic));
        currentTween.Join(CreditsGroup.DOFade(1f, duration));

        currentTween.OnComplete(() => CreditsGroup.interactable = true);
        currentTween.OnKill(() => currentTween = null);
        Time.timeScale = 0f;
    }

    public void CloseSettingsPanel()
    {
        isSettingsOpen = false;
        currentTween?.Kill();

        SettingsGroup.interactable = false;

        currentTween = DOTween.Sequence().SetUpdate(true);
        currentTween.Append(SettingsPanel.DOMove(closedTarget[0].position, duration).SetEase(Ease.InCubic));
        currentTween.Join(SettingsGroup.DOFade(0f, duration));
        currentTween.Join(Overlay.DOFade(0f, duration));

        currentTween.OnComplete(() =>
        {
            SettingsGroup.blocksRaycasts = false;
            Overlay.blocksRaycasts = false;
            Time.timeScale = 1f;
        });
        currentTween.OnKill(() => currentTween = null);
    }

    public void CloseCreditsPanel()
    {
        isCreditsOpen = false;
        currentTween?.Kill();

        CreditsGroup.interactable = false;

        currentTween = DOTween.Sequence().SetUpdate(true);
        currentTween.Append(CreditsPanel.DOMove(closedTarget[1].position, duration).SetEase(Ease.InCubic));
        currentTween.Join(CreditsGroup.DOFade(0f, duration));
        currentTween.Join(Overlay.DOFade(0f, duration));

        currentTween.OnComplete(() =>
        {
            CreditsGroup.blocksRaycasts = false;
            Overlay.blocksRaycasts = false;
            Time.timeScale = 1f;
        });
        currentTween.OnKill(() => currentTween = null);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
