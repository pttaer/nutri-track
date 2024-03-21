using Assets.SimpleGoogleSignIn.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTRegisterControl
{
    public static NTTRegisterControl Api;

    public Action OnUpdateBmiValueEvent;
    public Action<UserInfo> OnRegisterWithGoogleEvent;

    public void OnUpdateBmiValue()
    {
        OnUpdateBmiValueEvent?.Invoke();
    }

    public void OnRegisterWithGoogle(UserInfo userInfo)
    {
        OnRegisterWithGoogleEvent?.Invoke(userInfo);
    }
}
