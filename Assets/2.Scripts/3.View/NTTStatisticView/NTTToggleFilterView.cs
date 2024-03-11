using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NTTToggleFilterView : MonoBehaviour
{
    public Toggle Toggle;

    private TextMeshProUGUI m_TxtLabel;

    public void Init()
    {
        Toggle = transform.GetComponent<Toggle>();
        m_TxtLabel = transform.Find("TxtLabel").GetComponent<TextMeshProUGUI>();

        Toggle.onValueChanged.AddListener(OnFilterSelectedHandler);
    }

    private void OnFilterSelectedHandler(bool isOn)
    {
        m_TxtLabel.color = isOn ? Color.white : Color.black;
    }
}
