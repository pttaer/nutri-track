using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NTTMaterialIconView : MonoBehaviour
{
    private TextMeshProUGUI m_TxtMaterialIconCode;

    public void Init()
    {
        m_TxtMaterialIconCode = GetComponent<TextMeshProUGUI>();

        if (NTTDashboardControl.Api != null)
        {
            NTTDashboardControl.Api.OnUpdateIconEvent += UpdateIconCode;
        }
        else
        {
            Debug.LogError("NTTDashboardControl.Api is null. Make sure it's properly initialized.");
        }
    }

    private void OnDestroy()
    {
        NTTDashboardControl.Api.OnUpdateIconEvent -= UpdateIconCode;
    }

    private void UpdateIconCode(string iconCode)
    {
        m_TxtMaterialIconCode.text = iconCode;
    }
}
