using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NTTTagListItemView : MonoBehaviour
{
    private TextMeshProUGUI m_TxtLabel;
    private Button m_BtnClose;

    private Action<string> m_Callback;

    public void Init(string labelText, Action<string> callback = null)
    {
        m_TxtLabel = transform.Find("TxtLabel").GetComponent<TextMeshProUGUI>();
        m_BtnClose = transform.Find("BtnClose").GetComponent<Button>();

        m_TxtLabel.text = labelText;

        m_Callback = callback;

        m_BtnClose.onClick.AddListener(OnClickClose);

        transform.gameObject.SetActive(true);
    }

    private void OnClickClose()
    {
        m_Callback?.Invoke(m_TxtLabel.text);
    }
}
