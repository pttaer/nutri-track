//using Firebase.Auth;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using uPalette.Generated;
using uPalette.Runtime.Core;

public class NTTControl
{
    // Start is called before the first frame update
    private static NTTControl m_api;

    public Action<bool> OnLoadShowLoading;
    public Action CheckingSchoolIdEvent;
    public Action<bool, string> OnLoadFailShowSorry;
    public Action<bool> OnCheckSchoolSetMain;
    public Action<bool> OnFailLogin;

    private Action<string, string, string, string, Action, Action, Action> m_showNTTPopupEvent; //notify show popup message

    public Action<string, string, string, string, Action, Action, Action> ShowNTTPopupEvent
    {
        get { return m_showNTTPopupEvent; }
        set { m_showNTTPopupEvent = value; }
    }

    private Action<string, string, string, string, Action<string>, Action, Action, bool> m_showFAMPopupInputEvent; //notify show popup message

    public Action<string, string, string, string, Action<string>, Action, Action, bool> ShowFAMPopupInputEvent
    {
        get { return m_showFAMPopupInputEvent; }
        set { m_showFAMPopupInputEvent = value; }
    }

    private Action<TextMeshProUGUI, bool> m_ShowPopupCalendar; //notify show popup message

    public Action<TextMeshProUGUI, bool> OnCallShowPopupCalendarEvent
    {
        get { return m_ShowPopupCalendar; }
        set { m_ShowPopupCalendar = value; }
    }
    
    private Action<List<string>, Action<string>> m_ShowSelector; //notify show popup message

    public Action<List<string>, Action<string>> OnCallShowSelector
    {
        get { return m_ShowSelector; }
        set { m_ShowSelector = value; }
    }

    public static NTTControl Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new NTTControl();
            }
            return m_api;
        }
    }

    public void Init()
    {
        //init other controls here
        PrefsUtils.Api = new ();
        NTTApiControl.Api = new ();
        NTTDeeplinkControl.Api = new();
        PrefsUtils.Api = new();
        NTTApiControl.Api = new();
        NTTMyHealthControl.Api = new();
        NTTMenuControl.Api = new();
        NTTSceneLoaderControl.Api = new();
        NTTNotificationControl.Api = new();
        NTTLoginControl.Api = new();
        NTTRegisterControl.Api = new();

        Application.targetFrameRate = 60;

        NTTDeeplinkControl.Api.Init();
        NTTSceneLoaderControl.Api.Init();

        // Default value
        NTTSceneLoaderControl.Api.LoadScene(NTTConstant.SCENE_WELCOME);
        Debug.Log("INIT");
    }

    public Color GetPaletteColorValue(ColorEntry entry)
    {
        return PaletteStore.Instance.ColorPalette.GetActiveValue(entry.ToEntryId()).Value;
    }

    #region UTILS

    //show hide loading
    //isShow: true = show, false = hide

    public void ShowLoading()
    {
        LoadShowLoading(true);
    }

    public void HideLoading()
    {
        LoadShowLoading(false);
    }

    private void LoadShowLoading(bool isShow)
    {
        OnLoadShowLoading?.Invoke(isShow);
    }

    public void ShowSorry(string txtShow = null)
    {
        LoadFailShowSorry(true, txtShow);
    }

    public void HideSorry(string txtShow = null)
    {
        LoadFailShowSorry(false, txtShow);
    }

    private void LoadFailShowSorry(bool isShow, string txtShow)
    {
        OnLoadFailShowSorry?.Invoke(isShow, txtShow);
    }

    public void ShowMain(bool isShow)
    {
        OnCheckSchoolSetMain?.Invoke(isShow);
    }

    public void FailLogin(bool isWrongPasswordEmail)
    {
        OnFailLogin?.Invoke(isWrongPasswordEmail);
    }

    public void ShowFAMPopup(string title, string content, string btnConfirmText, string btnElseText, Action onConfirm = null, Action onElse = null, Action onExit = null)
    {
        ShowNTTPopupEvent?.Invoke(title, content, btnConfirmText, btnElseText, onConfirm, onElse, onExit);
    }

    public void ShowFAMPopup(string title, string content, string btnConfirmText, string btnElseText, Action<string> onConfirm, Action onElse = null, Action onExit = null, bool isShowInputField = false)
    {
        ShowFAMPopupInputEvent?.Invoke(title, content, btnConfirmText, btnElseText, onConfirm, onElse, onExit, isShowInputField);
    }
    
    public void ShowCalendarPopup(TextMeshProUGUI targetTxt, bool isClosePopup)
    {
        OnCallShowPopupCalendarEvent?.Invoke(targetTxt, isClosePopup);
    }
    
    public void ShowSelector(List<string> listTxtShow, Action<string> callbackSelect = null)
    {
        OnCallShowSelector?.Invoke(listTxtShow, callbackSelect);
    }
    #endregion UTILS
}