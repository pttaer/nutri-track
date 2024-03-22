using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NTTMyHealthRecordDetailItemView : MonoBehaviour
{
    private TextMeshProUGUI m_TxtTitle;
    private TextMeshProUGUI m_TxtDetail;

    private bool m_IsInited = false;

    public void Init(string title, string detail)
    {
        if (!m_IsInited)
        {
            m_TxtTitle = transform.Find("TxtTitle").GetComponent<TextMeshProUGUI>();
            m_TxtDetail = transform.Find("TxtDetail").GetComponent<TextMeshProUGUI>();
            m_IsInited = true;
        }

        m_TxtTitle.text = title;
        m_TxtDetail.text = detail;

        gameObject.SetActive(true);
    }

    public void ClearItem()
    {
        Destroy(gameObject);
    }
}
