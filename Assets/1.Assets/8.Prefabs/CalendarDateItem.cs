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
    [SerializeField] Image m_ImgBMI;
    [SerializeField] Image m_ImgCalories;
    private Button m_Btn;
    public bool m_IsSunday;
    public bool m_IsChoosen;

    public void EnableButton(bool enable)
    {
        if(m_Btn == null)
        {
            m_Btn = GetComponent<Button>();
        }
        m_Btn.interactable = enable;
        m_Txt.color = enable ? Color.black : NTTConstant.DISABLED_TEXT_COLOR;
    }

    public void EnableBMI(DateTime itemDate, List<NTTBMIRecordDTO> bmiRecordList)
    {
        NTTMyHealthControl.Api.CheckExistItemByDateInListBMI(
            itemDate,
            bmiRecordList,
            callbackExist: (itemData) =>
            {
                m_BMI.SetActive(true);
                var color = NTTControl.Api.GetPaletteColorValue(ColorEntry.BMI);
                m_ImgBMI.color = color;
                //Debug.Log("Run here YEET " + itemDate);
            },
            callbackNone: () =>
            {
                m_BMI.SetActive(false);
            }
        );
    }

    public void EnableDailyCal(DateTime itemDate, List<NTTDailyCalDTO> dailyCalList, List<NTTCalRecordDTO> calRecordList)
    {
        NTTMyHealthControl.Api.CheckExistItemByDateInListDailyCal(
            itemDate,
            dailyCalList,
            calRecordList,
            callbackExist: (dailyCal, listCalRecord) =>
            {
                m_DailyCal.SetActive(true);
                var color = NTTControl.Api.GetPaletteColorValue(ColorEntry.Calories);
                m_ImgCalories.color = color;
                //Debug.Log("Run here YEET " + itemDate);
            },
            callbackNone: () =>
            {
                m_DailyCal.SetActive(false);
            }
        );
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
            CalendarController.Api.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text, m_BMI.activeSelf, m_DailyCal.activeSelf);
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
            var color = NTTControl.Api.GetPaletteColorValue(ColorEntry.Primary);
            m_Txt.color = m_IsSunday ? color : Color.black;
            m_Txt.fontStyle = FontStyle.Normal;
            m_BG.SetActive(false);
        }
    }
}
