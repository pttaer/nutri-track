using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NTTPopupInputCalRecordView : MonoBehaviour
{
    Button m_BtnExit;
    Button m_BtnAddRecord;

    Button m_BtnNutrientSelect;
    Button m_BtnAmountSelect;
    Button m_BtnUnitSelect;
    Button m_BtnDescriptionInput;

    TextMeshProUGUI m_TxtNutrientPlaceholder;
    TextMeshProUGUI m_TxtAmountPlaceholder;
    TextMeshProUGUI m_TxtUnitPlaceholder;
    TextMeshProUGUI m_TxtDescriptionPlaceholder;

    TextMeshProUGUI m_TxtNutrient;
    TextMeshProUGUI m_TxtAmount;
    TextMeshProUGUI m_TxtUnit;
    TextMeshProUGUI m_TxtDescription;

    List<TextMeshProUGUI> m_TxtValueList = new List<TextMeshProUGUI>();

    bool m_IsInit = false;

    public void Init()
    {
        if (!m_IsInit)
        {
            m_BtnExit = transform.Find("BtnExit").GetComponent<Button>();
            m_BtnAddRecord = transform.Find("BtnAddRecord").GetComponent<Button>();

            m_BtnNutrientSelect = transform.Find("Nutrient/BtnValue").GetComponent<Button>();
            m_BtnAmountSelect = transform.Find("Amount/BtnValue").GetComponent<Button>();
            m_BtnUnitSelect = transform.Find("Unit/BtnValue").GetComponent<Button>();
            m_BtnDescriptionInput = transform.Find("Description/BtnValue").GetComponent<Button>();

            m_TxtNutrientPlaceholder = transform.Find("Nutrient/BtnValue/Placeholder").GetComponent<TextMeshProUGUI>();
            m_TxtAmountPlaceholder = transform.Find("Amount/BtnValue/Placeholder").GetComponent<TextMeshProUGUI>();
            m_TxtUnitPlaceholder = transform.Find("Unit/BtnValue/Placeholder").GetComponent<TextMeshProUGUI>();
            m_TxtDescriptionPlaceholder = transform.Find("Description/BtnValue/Placeholder").GetComponent<TextMeshProUGUI>();

            m_TxtNutrient = transform.Find("Nutrient/BtnValue/Value").GetComponent<TextMeshProUGUI>();
            m_TxtAmount = transform.Find("Amount/BtnValue/Value").GetComponent<TextMeshProUGUI>();
            m_TxtUnit = transform.Find("Unit/BtnValue/Value").GetComponent<TextMeshProUGUI>();
            m_TxtDescription = transform.Find("Description/BtnValue/Value").GetComponent<TextMeshProUGUI>();

            m_TxtValueList.Add(m_TxtNutrient);
            m_TxtValueList.Add(m_TxtAmount);
            m_TxtValueList.Add(m_TxtUnit);
            m_TxtValueList.Add(m_TxtDescription);

            m_BtnExit.onClick.AddListener(SetPopupOff);
            m_BtnAddRecord.onClick.AddListener(AddRecordWeight);

            m_BtnNutrientSelect.onClick.AddListener(ShowSelectorWeight);
            m_BtnAmountSelect.onClick.AddListener(ShowSelectorWeight);
            m_BtnUnitSelect.onClick.AddListener(ShowSelectorWeight);
            m_BtnDescriptionInput.onClick.AddListener(ShowSelectorWeight);

            m_IsInit = true;
        }
    }

    private void AddRecordWeight()
    {
        if (string.IsNullOrEmpty(m_TxtNutrient.text) && string.IsNullOrEmpty(m_TxtAmount.text) && string.IsNullOrEmpty(m_TxtUnit.text) && int.TryParse(m_TxtAmount.text, out int amount))
        {
            PostCalData(m_TxtNutrient.text, amount, m_TxtUnit.text, m_TxtDescription.text);
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

    private void PostCalData(string nutrient, int amount, string unit, string description)
    {
        NTTCalRecordPostDTO calRecord = new(nutrient, amount, unit, description);

        Debug.Log("Run here calRecord: " + JsonConvert.SerializeObject(calRecord));

        StartCoroutine(NTTApiControl.Api.PostData(NTTConstant.CAL_ROUTE, calRecord, (data) =>
        {
            Debug.Log("Run here" + JsonConvert.SerializeObject(data));

            NTTCalRecordDTO newRecord = NTTCalRecordDTO.FromJObject(data);

            Debug.Log("Run here" + JsonConvert.SerializeObject(newRecord));
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
