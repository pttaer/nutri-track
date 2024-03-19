using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class NTTPopupInputBMIRecordView : MonoBehaviour
{
    Button m_BtnExit;
    Button m_BtnWeightSelect;
    Button m_BtnAddRecord;

    TextMeshProUGUI m_TxtBMICalculated;
    TextMeshProUGUI m_TxtWeightPlaceholder;
    TextMeshProUGUI m_TxtWeight;

    bool m_IsInit = false;

    public void Init()
    {
        if (!m_IsInit)
        {
            m_BtnExit = transform.Find("BtnExit").GetComponent<Button>();
            m_BtnWeightSelect = transform.Find("Weight/BtnValue").GetComponent<Button>();
            m_BtnAddRecord = transform.Find("BtnAddRecord").GetComponent<Button>();

            m_TxtBMICalculated = transform.Find("CalculatedBMI/Title").GetComponent<TextMeshProUGUI>();
            m_TxtWeightPlaceholder = transform.Find("Weight/BtnValue/Placeholder").GetComponent<TextMeshProUGUI>();
            m_TxtWeight = transform.Find("Weight/BtnValue/Value").GetComponent<TextMeshProUGUI>();

            m_BtnExit.onClick.AddListener(SetPopupOff);
            m_BtnWeightSelect.onClick.AddListener(ShowSelectorWeight);
            m_BtnAddRecord.onClick.AddListener(AddRecordWeight);

            m_IsInit = true;
        }
    }

    private void AddRecordWeight()
    {
        if(string.IsNullOrEmpty(m_TxtWeight.text) && float.TryParse(m_TxtWeight.text, out float weight))
        {
            PostWeightBMIData(weight);
        }
    }

    private void ShowSelectorWeight()
    {
        throw new NotImplementedException();

        // TODO: Calculate and show on ui BMI
        // m_TxtBMICalculated.text = $"{float.Parse(m_TxtWeight.text) / } {NTTConstant.BMI_UNIT}";
    }

    private void SetPopupOff()
    {
        gameObject.SetActive(false);
        NTTMyHealthControl.Api.ClosePopupRecord();
    }

    private void PostWeightBMIData(float weight)
    {
        //NTTBMIRecordPostDTO bmiRecord = new(weight);

        //Debug.Log("Run here bmiRecord: " + JsonConvert.SerializeObject(bmiRecord));

        //StartCoroutine(NTTApiControl.Api.PostData(NTTConstant.BMI_ROUTE, bmiRecord, (data, result) =>
        //{
        //    if (result == UnityWebRequest.Result.Success)
        //    {
        //        Debug.Log("Run here" + JsonConvert.SerializeObject(data));

        //        NTTBMIRecordDTO newRecord = NTTBMIRecordDTO.FromJObject(data);

        //        Debug.Log("Run here" + JsonConvert.SerializeObject(newRecord));
        //    }
        //}));
    }
    
    private void GetWeightBMIData()
    {
        //StartCoroutine(NTTApiControl.Api.GetListData<NTTBMIRecordDTO>(NTTConstant.BMI_ROUTE, callback: (data) =>
        //{
        //    Debug.Log("Run here" + JsonConvert.SerializeObject(data));
        //}));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Run here");
            PostWeightBMIData(40);
        }
    }
}
