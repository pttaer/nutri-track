using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NTTMyHealthPopupInputCalRecordView : MonoBehaviour
{
    Button m_BtnExit;
    Button m_BtnAddRecord;

    TMP_InputField m_IpfNutrient;
    TMP_InputField m_IpfCaloAmount;
    Button m_BtnUnitSelect;
    TMP_InputField m_IpfDescriptionInput;

    TextMeshProUGUI m_TxtCaloUnit;
    TextMeshProUGUI m_TxtUnitPlaceholder;

    bool m_IsInit = false;
    Action m_OnExit;
    DateTime m_CurrentDate;
    private NTTDailyCalDTO m_CurrentDailyCal;

    public void Init(Action callbackExitAddRecord = null)
    {
        m_OnExit = callbackExitAddRecord;

        if (!m_IsInit)
        {
            m_BtnExit = transform.Find("BtnExit").GetComponent<Button>();
            m_BtnAddRecord = transform.Find("BtnAddRecord").GetComponent<Button>();

            m_IpfNutrient = transform.Find("Nutrient/IpfValue").GetComponent<TMP_InputField>();
            m_IpfCaloAmount = transform.Find("Amount/IpfValue").GetComponent<TMP_InputField>();
            m_BtnUnitSelect = transform.Find("Unit/BtnValue").GetComponent<Button>();
            m_IpfDescriptionInput = transform.Find("Description/IpfValue").GetComponent<TMP_InputField>();

            m_TxtCaloUnit = transform.Find("Unit/BtnValue/Value").GetComponent<TextMeshProUGUI>();
            m_TxtUnitPlaceholder = transform.Find("Unit/BtnValue/Placeholder").GetComponent<TextMeshProUGUI>();

            m_BtnExit.onClick.AddListener(SetPopupOff);
            m_BtnAddRecord.onClick.AddListener(AddRecordWeight);
            m_BtnUnitSelect.onClick.AddListener(OnClickSelectUnit);

            m_IsInit = true;
        }
    }

    public void SetCurrentDate(DateTime date, NTTDailyCalDTO currentDailyCal)
    {
        m_CurrentDailyCal = currentDailyCal;

        m_CurrentDate = DateTime.Parse(date.ToString(NTTConstant.DATE_FORMAT_ISO_STRING));

        //Debug.Log("Run here m_CurrentDate" + m_CurrentDate);

        SetPopupActive(true);
    }

    private void OnClickSelectUnit()
    {
        NTTControl.Api.ShowSelector(new List<string>() { NTTConstant.CALS, NTTConstant.KILO_CALS }, (selectedUnit) =>
        {
            m_TxtCaloUnit.text = selectedUnit;
            SetUnitPlaceholder(string.IsNullOrEmpty(m_TxtCaloUnit.text));
        });
    }

    private void SetUnitPlaceholder(bool enable)
    {
        m_TxtUnitPlaceholder.gameObject.SetActive(enable);
    }

    private void AddRecordWeight()
    {
        bool isAllValueInputted = 
            !string.IsNullOrEmpty(m_IpfNutrient.text) 
            && !string.IsNullOrEmpty(m_IpfCaloAmount.text) 
            && !string.IsNullOrEmpty(m_TxtCaloUnit.text);

        if (isAllValueInputted && int.TryParse(m_IpfCaloAmount.text, out int amount))
        {
            string description = string.IsNullOrEmpty(m_IpfDescriptionInput.text) ? NTTConstant.NONE : m_IpfDescriptionInput.text;

            PostCalData(m_IpfNutrient.text, amount, m_TxtCaloUnit.text, description);
        }
    }

    private void SetPopupOff()
    {
        SetPopupActive(false);
        SetUnitPlaceholder(true);

        m_IpfNutrient.text = string.Empty;
        m_IpfCaloAmount.text = string.Empty;
        m_TxtCaloUnit.text = string.Empty;
        m_IpfDescriptionInput.text = string.Empty;


        NTTMyHealthControl.Api.ClosePopupRecord();
    }

    private void PostCalData(string nutrient, int amount, string unit, string description)
    {
        if(m_CurrentDailyCal != null)
        {
            NTTCalRecordPostDTO calRecord = new(nutrient, amount, unit, m_CurrentDailyCal.Id, description);

            StartCoroutine(NTTApiControl.Api.PostData(NTTConstant.CAL_RECORDS_ROUTE, calRecord, (data) =>
            {
                Debug.Log("Run here CALRECORD");

                OnFinishAddRecord();
            }));
        }
        else
        {
            NTTDailyCalPostDTO dailyCal = new("daily", m_CurrentDate);

            StartCoroutine(NTTApiControl.Api.PostData(NTTConstant.DAILY_CAL_ROUTE, dailyCal, (data) =>
            {
                Debug.Log("Run here DAILYCAL");

                NTTDailyCalDTO dailyCal = NTTDailyCalDTO.FromJObject(data);

                m_CurrentDailyCal = dailyCal;

                NTTCalRecordPostDTO calRecord = new(nutrient, amount, unit, dailyCal.Id, description);

                StartCoroutine(NTTApiControl.Api.PostData(NTTConstant.CAL_RECORDS_ROUTE, calRecord, (data) =>
                {
                    OnFinishAddRecord();
                }));
            }));
        }
    }

    private void OnFinishAddRecord()
    {
        SetPopupOff();
        m_OnExit?.Invoke();
    }

    private void SetPopupActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
