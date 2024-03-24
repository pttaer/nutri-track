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

    public void Init(Action callbackExit = null)
    {
        m_OnExit = callbackExit;

        if (!m_IsInit)
        {
            m_BtnExit = transform.Find("BtnExit").GetComponent<Button>();
            m_BtnAddRecord = transform.Find("BtnAddRecord").GetComponent<Button>();

            m_IpfWeightSelect = transform.Find("Weight/IpfValue").GetComponent<TMP_InputField>();

            m_TxtBMICalculated = transform.Find("CalculatedBMI/Title").GetComponent<TextMeshProUGUI>();

            m_BtnExit.onClick.AddListener(SetPopupOff);
            m_IpfWeightSelect.onValueChanged.AddListener(ValidateWeight);
            m_BtnAddRecord.onClick.AddListener(AddRecordWeight);

            m_IsInit = true;
        }
    }


    public void SetCurrentDate(DateTime date, NTTBMIRecordDTO currentBMIRecord)
    {
        m_CurrentBMIRecord = currentBMIRecord;
        m_CurrentDate = DateTime.Parse(date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
        Debug.Log("Run here m_CurrentDate" + m_CurrentDate);
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
        Debug.Log("Run here m_IpfWeightSelect.text" + m_IpfWeightSelect.text);
        if (!string.IsNullOrEmpty(m_IpfWeightSelect.text) && float.TryParse(m_IpfWeightSelect.text, out float weight))
        {
            Debug.Log("Run here weight" + weight);
            PostWeightBMIData(weight);
        }
    }

    private void SetPopupOff()
    {
        gameObject.SetActive(false);
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
                SetPopupOff();
                Debug.Log("PUT");
                m_CurrentBMIRecord = null;
                m_OnExit?.Invoke();
            }));
            m_CurrentBMIRecord = null;
        }
        else
        {
            StartCoroutine(NTTApiControl.Api.PostData(NTTConstant.BMI_RECORDS_ROUTE, bmiRecord, (data) =>
            {
                SetPopupOff();
                Debug.Log("POST");
                m_OnExit?.Invoke();
            }));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Run here");
            PostWeightBMIData(40);
        }
    }
}
