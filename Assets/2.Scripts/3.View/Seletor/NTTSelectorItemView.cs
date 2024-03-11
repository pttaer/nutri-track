using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NTTSelectorItemView : MonoBehaviour
{
    private TextMeshProUGUI m_TxtTitle;
    private TextMeshProUGUI m_TxtDescription;

    private bool m_IsInited = false;

    public void Init(NTTSelectorItemDTO data)
    {
        if (!m_IsInited)
        {
            m_TxtTitle = transform.Find("TxtTitle").GetComponent<TextMeshProUGUI>();
            m_TxtDescription = transform.Find("TxtDescription").GetComponent<TextMeshProUGUI>();

            m_IsInited = true;
        }

        m_TxtTitle.text = data.name;
        m_TxtDescription.text = data.shortDescription;
    }
}
