using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Globalization;

public class NTTMyHealthPopupInputBMIRecordView : MonoBehaviour
{
    Button m_BtnExit;
    Button m_BtnAddRecord;

    TMP_InputField m_IpfWeightSelect;

    TextMeshProUGUI m_TxtBMICalculated;

    bool m_IsInit = false;
    NTTBMIRecordDTO m_CurrentBMIRecord;
    Action m_OnExit;
    DateTime m_CurrentDate;

    public void Init(Action callbackExitAddRecord = null)
    {
        m_OnExit = callbackExitAddRecord;

        if (!m_IsInit)
        {
            m_BtnExit = transform.Find("BtnExit").GetComponent<Button>();
            m_BtnAddRecord = transform.Find("BtnAddRecord").GetComponent<Button>();

            m_IpfWeightSelect = transform.Find("Weight/IpfValue").GetComponent<TMP_InputField>();

            m_TxtBMICalculated = transform.Find("CalculatedBMI/Title").GetComponent<TextMeshProUGUI>();

            m_BtnExit.onClick.AddListener(SetPopupOff);
            m_BtnAddRecord.onClick.AddListener(AddRecordWeight);
            m_IpfWeightSelect.onValueChanged.AddListener(ValidateWeight);

            m_IsInit = true;
        }
    }


    public void SetCurrentDate(DateTime date, NTTBMIRecordDTO currentBMIRecord)
    {
        m_CurrentBMIRecord = currentBMIRecord;

        m_CurrentDate = DateTime.Parse(date.ToString(NTTConstant.DATE_FORMAT_ISO_STRING));

        //Debug.Log("Run here m_CurrentDate" + m_CurrentDate);

        SetPopupActive(true);
    }

    private void ValidateWeight(string weightTxt)
    {
        if (!string.IsNullOrEmpty(weightTxt))
        {
            var bmi = NTTMyHealthControl.Api.CurrentUserBMICalculate(float.Parse(weightTxt));

            m_TxtBMICalculated.text = $"Calculated BMI: {bmi.ToString("F1")} {NTTConstant.BMI_UNIT}";
        }
    }

    private void AddRecordWeight()
    {
        if (!string.IsNullOrEmpty(m_IpfWeightSelect.text) && float.TryParse(m_IpfWeightSelect.text, out float weight))
        {
            PostWeightBMIData(weight);
        }
    }

    private void SetPopupOff()
    {
        SetPopupActive(false);

        m_IpfWeightSelect.text = string.Empty;

        NTTMyHealthControl.Api.ClosePopupRecord();
    }

    private void PostWeightBMIData(float weight)
    {
        NTTBMIRecordPostDTO bmiRecord = new(weight, m_CurrentDate);

        if (m_CurrentBMIRecord != null)
        {
            StartCoroutine(NTTApiControl.Api.EditData(string.Format(NTTConstant.BMI_RECORDS_ROUTE_FORMAT, m_CurrentBMIRecord.Id), bmiRecord, (data) =>
            {
                Debug.Log("PUT");

                OnFinishAddRecord();

                m_CurrentBMIRecord = null;
            }));

            m_CurrentBMIRecord = null;
        }
        else
        {
            StartCoroutine(NTTApiControl.Api.PostData(NTTConstant.BMI_RECORDS_ROUTE, bmiRecord, (data) =>
            {
                Debug.Log("POST");

                OnFinishAddRecord();
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
