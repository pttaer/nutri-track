using UnityEngine;
using uPalette.Generated;
using UnityEngine.UI;
using uPalette.Runtime.Core;
using System.Collections.Generic;
using System;

public class CalendarDateItem : MonoBehaviour
{
    [SerializeField] Text m_Txt;
    [SerializeField] GameObject m_BG;
    [SerializeField] GameObject m_BMI;
    [SerializeField] GameObject m_DailyCal;
    public bool m_IsSunday;
    public bool m_IsChoosen;

    public void EnableButton(bool enable)
    {
        GetComponent<Button>().interactable = enable;
    }
    
    public void EnableBMI(DateTime itemDate, List<NTTBMIRecordDTO> bmiRecordList)
    {
        foreach (NTTBMIRecordDTO item in bmiRecordList)
        {
            if (item.Date.Date == itemDate.Date)
            {
                m_BMI.SetActive(true);

                Debug.Log("Run here YEET " + itemDate);
                return;
            }
        }
        m_BMI.SetActive(false);
    }
    
    public void EnableDailyCal(DateTime itemDate, List<NTTDailyCalDTO> dailyCalList)
    {
        foreach (NTTDailyCalDTO item in dailyCalList)
        {
            if (item.Date.Date == itemDate.Date)
            {
                m_DailyCal.SetActive(true);

                Debug.Log("Run here YEET2 " + itemDate);
                return;
            }
        }
        m_DailyCal.SetActive(false);
    }

    public void OnDateItemClick()
    {
        if (m_BG.activeSelf)
        {
            SetItemOff();
            CalendarController.Api.ClearTargetText();
        }
        else
        {
            CalendarController.Api.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
            m_Txt.color = Color.white;
            m_Txt.fontStyle = FontStyle.Bold;
            m_BG.SetActive(true);
        }
        m_IsChoosen = m_BG.activeSelf;
    }

    public void SetItemOff()
    {
        if (m_BG.activeSelf)
        {
            var palette = PaletteStore.Instance.ColorPalette;
            var color = palette.GetActiveValue(ColorEntry.DateSelected.ToEntryId()).Value;
            m_Txt.color = m_IsSunday ? color : Color.black;
            m_Txt.fontStyle = FontStyle.Normal;
            m_BG.SetActive(false);
        }
    }
}
