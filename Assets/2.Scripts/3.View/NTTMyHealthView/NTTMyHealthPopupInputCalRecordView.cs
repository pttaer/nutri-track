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

    TMP_InputField m_IpfNutrientSelect;
    TMP_InputField m_IpfAmountSelect;
    Button m_BtnUnitSelect;
    TMP_InputField m_IpfDescriptionInput;

    TextMeshProUGUI m_TxtBtnUnit;

    bool m_IsInit = false;
    Action m_OnExit;
    DateTime m_CurrentDate;

    public void Init(Action callbackExit = null)
    {
        m_OnExit = callbackExit;
        if (!m_IsInit)
        {
            m_BtnExit = transform.Find("BtnExit").GetComponent<Button>();
            m_BtnAddRecord = transform.Find("BtnAddRecord").GetComponent<Button>();

            m_IpfNutrientSelect = transform.Find("Nutrient/IpfValue").GetComponent<TMP_InputField>();
            m_IpfAmountSelect = transform.Find("Amount/IpfValue").GetComponent<TMP_InputField>();
            m_BtnUnitSelect = transform.Find("Unit/BtnValue").GetComponent<Button>();
            m_IpfDescriptionInput = transform.Find("Description/IpfValue").GetComponent<TMP_InputField>();

            m_TxtBtnUnit = transform.Find("Unit/BtnValue/Value").GetComponent<TextMeshProUGUI>();

            m_BtnExit.onClick.AddListener(SetPopupOff);
            m_BtnAddRecord.onClick.AddListener(AddRecordWeight);

            m_IpfNutrientSelect.onValueChanged.AddListener(Validate);
            m_IpfAmountSelect.onValueChanged.AddListener(Validate);
            m_BtnUnitSelect.onClick.AddListener(OnClickSelectUnit);
            m_IpfDescriptionInput.onValueChanged.AddListener(Validate);

            m_IsInit = true;
        }
    }

    public void SetCurrentDate(DateTime date)
    {
        m_CurrentDate = DateTime.Parse(date.ToString(), null, System.Globalization.DateTimeStyles.RoundtripKind);

        Debug.Log("Run here m_CurrentDate" + m_CurrentDate);
    }

    private void Validate(string arg0)
    {

    }

    private void OnClickSelectUnit()
    {

    }

    private void AddRecordWeight()
    {
        if (!string.IsNullOrEmpty(m_IpfNutrientSelect.text) && !string.IsNullOrEmpty(m_IpfAmountSelect.text) && !string.IsNullOrEmpty(m_TxtBtnUnit.text) && int.TryParse(m_IpfAmountSelect.text, out int amount))
        {
            PostCalData(m_IpfNutrientSelect.text, amount, m_TxtBtnUnit.text, m_IpfDescriptionInput.text);
        }
    }

    private void SetPopupOff()
    {
        gameObject.SetActive(false);
        m_IpfNutrientSelect.text = string.Empty;
        m_IpfAmountSelect.text = string.Empty;
        m_TxtBtnUnit.text = string.Empty;
        m_IpfDescriptionInput.text = string.Empty;
        NTTMyHealthControl.Api.ClosePopupRecord();
    }

    private void PostCalData(string nutrient, int amount, string unit, string description)
    {
        NTTCalRecordPostDTO calRecord = new(nutrient, amount, unit, description);

        Debug.Log("Run here calRecord: " + JsonConvert.SerializeObject(calRecord));

        StartCoroutine(NTTApiControl.Api.PostData(NTTConstant.CAL_RECORDS_ROUTE, calRecord, (data) =>
        {
            NTTCalRecordDTO newRecord = NTTCalRecordDTO.FromJObject(data);
            Debug.Log("Run here" + JsonConvert.SerializeObject(newRecord));
            SetPopupOff();
            m_OnExit?.Invoke();
        }));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Run here");
            PostCalData("testNutrient", 200, "cals", "");
        }
    }
}
