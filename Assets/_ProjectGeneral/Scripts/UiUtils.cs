using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiUtils : Singleton<UiUtils>
{
    [SerializeField] private GameObject popupObj;
    [SerializeField] private Text popupText;
    [SerializeField] private Button yesBtn;
    [SerializeField] private Button noBtn;

    [SerializeField] public GameObject onlinePopup;
    [SerializeField] public Text onlinePopupText;

    [SerializeField] private GameObject loadingObj;
    [SerializeField] private Text loadingText;
    
    private UnityAction _quitGame;

    private void Start()
    {
        _quitGame += QuitGame;
    }

    public void QuitGameConfirm()
    {
        ShowPopup(true, "You want to quit the game?", _quitGame);
    }

    public void ShowPopup(bool state, string value, UnityAction btnFunction)
    {
        popupObj.SetActive(state);
        popupText.text = value;
        noBtn.gameObject.SetActive(true);
        noBtn.onClick.AddListener(() => SimplePopup(false));
        yesBtn.transform.Find("Text").GetComponent<Text>().text = "Yes";
        yesBtn.onClick.RemoveAllListeners();
        yesBtn.onClick.AddListener(btnFunction);
    }

    public void SimplePopup(bool state, string value = null, bool showNoBtn = false)
    {
        popupObj.SetActive(state);
        popupText.text = value;
        if (!showNoBtn) noBtn.gameObject.SetActive(false);
        yesBtn.transform.Find("Text").GetComponent<Text>().text = "Ok";
        yesBtn.onClick.RemoveAllListeners();
        yesBtn.onClick.AddListener(() => SimplePopup(false));
    }

    public void OnlinePopup(bool state, float showTime = 0, string value = null)
    {
        onlinePopup.SetActive(state);
        onlinePopupText.text = value;
        if (showTime > 0) StartCoroutine(OnlinePopupDeactivate(showTime));
    }

    private IEnumerator OnlinePopupDeactivate(float time)
    {
        yield return new WaitForSeconds(time);
        onlinePopup.SetActive(false);
    }

    public void ShowLoading(bool state, string value = null)
    {
        loadingObj.SetActive(state);
        loadingText.text = value;
    }
    
    public IEnumerator ShowLoading(bool state, float time, string value = null)
    {
        loadingObj.SetActive(state);
        loadingText.text = value;
        yield return new WaitForSeconds(time);
        loadingObj.SetActive(false);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        _quitGame -= QuitGame;
    }
}
