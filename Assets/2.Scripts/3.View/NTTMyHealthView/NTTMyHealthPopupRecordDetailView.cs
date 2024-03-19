using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class NTTMyHealthPopupRecordDetailView : MonoBehaviour
{
    private Button m_BtnExit;

    private Transform m_TfContent;
    private NTTMyHealthRecordDetailItemView m_ItemDetailPref;

    private NTTBMIRecordDTO m_BmiRecordData;
    private float m_CurrentDayBMI;
    private NTTDailyCalDTO m_DailyCalData;
    private List<NTTCalRecordDTO> m_CalRecordList = new List<NTTCalRecordDTO>();

    private List<NTTMyHealthRecordDetailItemView> m_DetailItemViewList = new List<NTTMyHealthRecordDetailItemView>();

    private bool m_IsInited = false;

    public void Init(float currentBMI = 0, NTTBMIRecordDTO bmiRecordData = null, NTTDailyCalDTO dailyCalData = null, List<NTTCalRecordDTO> calRecordList = null)
    {
        m_CurrentDayBMI = currentBMI;
        m_BmiRecordData = bmiRecordData;
        m_DailyCalData = dailyCalData;
        m_CalRecordList = calRecordList;

        if (!m_IsInited)
        {
            m_TfContent = transform.Find("Content/NTTNoteScrollView/Viewport/Content");

            m_BtnExit = transform.Find("Content/NTTNoteScrollView/BtnExit").GetComponent<Button>();
            m_ItemDetailPref = transform.Find("Content/NTTNoteScrollView/Viewport/Content/ItemDetail").GetComponent<NTTMyHealthRecordDetailItemView>();

            m_BtnExit.onClick.AddListener(SetPopupOff);

            m_ItemDetailPref.gameObject.SetActive(false);

            m_IsInited = true;
        }

        SetBMIRecordDetail();
        SetCalRecordDetail();

        gameObject.SetActive(true);
    }

    private void SetPopupOff()
    {
        gameObject.SetActive(false);
        foreach (var item in m_DetailItemViewList)
        {
            item.ClearItem();
        }
        m_DetailItemViewList.Clear();
    }

    private void SetBMIRecordDetail()
    {
        if (m_BmiRecordData == null && m_CurrentDayBMI == 0)
        {
            return;
        }

        var view = Instantiate(m_ItemDetailPref, m_TfContent).GetComponent<NTTMyHealthRecordDetailItemView>();
        m_DetailItemViewList.Add(view);

        view.Init($"{m_BmiRecordData.Date.ToString(NTTConstant.DATE_FORMAT_FULL) ?? "ERROR NULL"}\n\n  BMI Record", $"     Weight: {m_BmiRecordData.Weight}\n     BMI: {m_CurrentDayBMI}");
    }

    private void SetCalRecordDetail()
    {
        if(m_CalRecordList.Count < 1 && m_DailyCalData == null)
        {
            return;
        }

        var view = Instantiate(m_ItemDetailPref, m_TfContent).GetComponent<NTTMyHealthRecordDetailItemView>();
        m_DetailItemViewList.Add(view);

        StringBuilder sb = new StringBuilder();

        int count = m_CalRecordList.Count;

        for (int i = 0; i < count; i++)
        {
            var item = m_CalRecordList[i];
            sb.Append($"    Record {i + 1}:\n      Nutrient: {item.Nutrient}\n      Amount: {item.Amount} {item.Unit}\n      Description: {item.Description}\n\n");
        }
 
        view.Init("  Calories Record", sb.ToString());
    }
}
