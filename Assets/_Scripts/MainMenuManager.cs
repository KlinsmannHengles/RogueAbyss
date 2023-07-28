using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    public Camera mainCamera;
    public SpriteRenderer backgroundSprite;
    public CanvasGroup mainCanvasGroup;
    public AudioSource backgroundSong;

    public GameObject mainMenuScreenObject;
    public GameObject HowToPlayScreenObject;
    public GameObject CreditsScreenObject;

    public CanvasGroup mainMenuScreenCanvasGroup;
    public CanvasGroup HowToPlayScreenCanvasGroup;
    public CanvasGroup CreditsScreenCanvasGroup;

    private GameObject screenToChange; // support
    private bool enableToPressStartButton = true; // support

    public void StartGame()
    {
        if (enableToPressStartButton)
        {
            enableToPressStartButton = false;
            mainCamera.DOOrthoSize(1f, 2f).SetEase(Ease.InQuint);
            backgroundSprite.DOFade(0f, 2f).SetEase(Ease.InQuint);
            mainCanvasGroup.DOFade(0f, 2f).SetEase(Ease.InQuint);
            backgroundSong.DOFade(0f, 2f).SetEase(Ease.InQuint).onComplete = EnableToPressStartButton;
            StartCoroutine(StartGameSupport());
        }     
    }

    private void EnableToPressStartButton()
    {
        enableToPressStartButton = true;
    }

    public IEnumerator StartGameSupport()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowScreen(GameObject _screen, CanvasGroup _canvasGroup)
    {
        _screen.SetActive(true);
        _canvasGroup.DOFade(1f, 1f);
    }

    public void HideScreen(GameObject _screen, CanvasGroup _canvasGroup)
    {
        screenToChange = _screen;
        _canvasGroup.DOFade(0f, 1f).onComplete = HideScreenSupport;
    }

    public void HideScreenSupport()
    {
        screenToChange.SetActive(false);
    }

    public void MenuToHowToPlay()
    {
        HideScreen(mainMenuScreenObject, mainMenuScreenCanvasGroup);
        ShowScreen(HowToPlayScreenObject, HowToPlayScreenCanvasGroup);
    }

    public void HowToPlayToMenu()
    {
        HideScreen(HowToPlayScreenObject, HowToPlayScreenCanvasGroup);
        ShowScreen(mainMenuScreenObject, mainMenuScreenCanvasGroup);
    }

    public void MenuToCredits()
    {
        HideScreen(mainMenuScreenObject, mainMenuScreenCanvasGroup);
        ShowScreen(CreditsScreenObject, CreditsScreenCanvasGroup);
    }

    public void CreditsToMenu()
    {
        HideScreen(CreditsScreenObject, CreditsScreenCanvasGroup);
        ShowScreen(mainMenuScreenObject, mainMenuScreenCanvasGroup);
    }
}
