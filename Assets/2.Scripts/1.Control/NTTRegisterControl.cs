using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTRegisterControl
{
    public static NTTRegisterControl Api;

    public Action OnUpdateBmiValueEvent;

    public void OnUpdateBmiValue()
    {
        OnUpdateBmiValueEvent?.Invoke();
    }
}
