using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NTTWelcomeView : MonoBehaviour
{
    private Button m_BtnGetStarted;
    private Button m_BtnLogin;

    // Start is called before the first frame update
    void Start()
    {
        Init(); 
    }

    public void Init()
    {
        m_BtnGetStarted = transform.Find("Body/BtnGr/BtnGetStarted").GetComponent<Button>();
        m_BtnLogin = transform.Find("Body/BtnGr/LoginGr/BtnLogin").GetComponent<Button>();

        m_BtnGetStarted.onClick.AddListener(LoadRegisterScene);
        m_BtnLogin.onClick.AddListener(LoadLoginScene);
    }

    private void LoadRegisterScene()
    {
        NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_REGISTER);
    }

    private void LoadLoginScene()
    {
        NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_LOGIN);
    }
}
