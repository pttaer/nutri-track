using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NTTDashboardMenuItem : MonoBehaviour
{
    private LayoutElement m_Layout;
    private TextMeshProUGUI m_TxtLabel;

    public void Init()
    {
        m_Layout = GetComponent<LayoutElement>();
        m_TxtLabel = transform.Find("TxtLabel").GetComponent<TextMeshProUGUI>();

        NTTDashboardControl.Api.OnMenuExpandEvent += OnMenuExpand;
    }

    private void OnDestroy()
    {
        NTTDashboardControl.Api.OnMenuExpandEvent -= OnMenuExpand;
    }

    private void OnMenuExpand(bool isExpanded, Tween firstSequence)
    {
        if (!isExpanded && m_TxtLabel != null)
        {
            m_TxtLabel.gameObject.SetActive(isExpanded);
        }

        Tween tween2 = TweenUtils.TweenLerpFloat(() => m_Layout.minWidth, x => m_Layout.minWidth = x, isExpanded ? 124f : 32f, 0.5f);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(firstSequence);
        sequence.Join(tween2);

        if (isExpanded && m_TxtLabel != null)
        {
            sequence.OnComplete(() =>
         {
             m_TxtLabel.gameObject.SetActive(isExpanded);
         });
        }
    }
}
