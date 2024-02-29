//using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        NTTDeeplinkControl.Api = new ();
        PrefsUtils.Api = new();
        NTTApiControl.Api = new();
        NTTMyHealthControl.Api = new();
        NTTMenuControl.Api = new();
        NTTSceneLoaderControl.Api = new();

        Application.targetFrameRate = 60;

        NTTDeeplinkControl.Api.Init();
        NTTSceneLoaderControl.Api.Init();

        // Default value
        //UnloadThenLoadScene(NTTConstant.SCENE_LOGIN);
        //LoadScene(NTTConstant.SCENE_MENU);
        NTTSceneLoaderControl.Api.LoadScene(NTTConstant.SCENE_MENU);

        Debug.Log("INIT");
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

    public void UnloadThenLoadScene(string sceneToLoad)
    {
        // Unload the current scene & remove scenes loaded beside loadfirst
        List<string> sceneNames = SceneManager.GetAllScenes()
                                              .Select(scene => scene.name)
                                              .ToList();

        foreach (var sceneName in sceneNames)
        {
            //if (sceneName != NTTConstant.SCENE_LOADFIRST && sceneName != NTTConstant.SCENE_MENU)
            //{
            //    if (NTTModel.Api.ScenesLoaded.Contains(sceneName) && NTTModel.Api.ScenesLoaded.Contains(sceneToLoad))
            //    {
            //        NTTModel.Api.ScenesLoaded.Remove(sceneName);
            //    }
            //    UnLoadScene(sceneName);
            //}
        }

        LoadScene(sceneToLoad);
    }

    // Load and add scene to scenes loaded list
    private void LoadScene(string sceneToLoad)
    {
        if (!SceneManager.GetSceneByName(sceneToLoad).isLoaded)
        {
            SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

//            if (!NTTModel.Api.ScenesLoaded.Contains(sceneToLoad))
//            {
//                NTTModel.Api.ScenesLoaded.Add(sceneToLoad);
//            }

//#if UNITY_EDITOR
//            foreach (var scene in NTTModel.Api.ScenesLoaded)
//            {
//                Debug.Log("SCENE LOADED: " + scene);
//            }
//#endif
        }
    }

    private void UnLoadScene(string sceneToUnLoad)
    {
        if (SceneManager.GetSceneByName(sceneToUnLoad).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneToUnLoad);
        }
    }

    // For opening one or many panels in the set of panels of one scene
    public void OpenPanel(GameObject pnlToShow, List<GameObject> pnlList, bool isCheckToTurnPanelOff = false)
    {
        if (pnlToShow.activeSelf && isCheckToTurnPanelOff)
        {
            pnlToShow.SetActive(false);
            return;
        }

        foreach (var pnl in pnlList)
        {
            if (pnl.gameObject == pnlToShow)
            {
                pnl.gameObject.SetActive(true);
                continue;
            }
            pnl.SetActive(false);
        }
    }

    /*public void OpeManyPanel(List<GameObject> pnlList, bool isCheckToTurnPanelOff = false, params GameObject[] pnlsToShow)
    {
        pnlList.ForEach(panel => panel.SetActive(!pnlsToShow.Contains(panel)));
        pnlsToShow.ToList().ForEach(panelToShow => panelToShow.SetActive(true));

        if (isCheckToTurnPanelOff)
        {
            pnlList.Where(panel => !pnlsToShow.Contains(panel)).ToList().ForEach(panel => panel.SetActive(false));
        }
    }*/

    public void OpenManyPanel(List<GameObject> pnlList, List<GameObject> pnlsToShow)
    {
        pnlList.ForEach(panel => panel.SetActive(false));
        pnlsToShow.ForEach(panel => panel.SetActive(true));
    }

    #endregion UTILS
}