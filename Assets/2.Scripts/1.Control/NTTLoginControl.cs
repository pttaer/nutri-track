using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTLoginControl : MonoBehaviour
{
    public static NTTLoginControl Api;

    public Action OnLoginSuccessfullyEvent;

    public void OnLoginSuccessfully()
    {
        OnLoginSuccessfullyEvent?.Invoke();
    }
}
