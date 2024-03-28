using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NTTExpertItemView : MonoBehaviour
{
    public Button m_BtnDetail;

    public Button m_BtnVerified;
    public Button m_BtnUnVerified;

    public TextMeshProUGUI m_TxtName;
    public TextMeshProUGUI m_TxtEmail;
    public TextMeshProUGUI m_TxtGender;

    public TextMeshProUGUI m_TxtOverallRating;
    public TextMeshProUGUI m_TxtRatingValue;

    public bool m_IsActive;

    public void Init(NTTExpertDTO data)
    {
        gameObject.SetActive(true);

        m_BtnDetail = GetComponent<Button>();

        Transform info = transform.Find("Info");
        Transform rating = transform.Find("Rating/OverallRating");
        Transform topBar = transform.Find("TopBar");

        m_BtnVerified = topBar.Find("BtnVerified").GetComponent<Button>();
        m_BtnUnVerified = topBar.Find("BtnUnVerified").GetComponent<Button>();

        m_TxtName = info.Find("Name").GetComponent<TextMeshProUGUI>();
        m_TxtEmail = info.Find("Email").GetComponent<TextMeshProUGUI>();
        m_TxtGender = info.Find("Gender").GetComponent<TextMeshProUGUI>();

        m_TxtOverallRating = rating.Find("Title").GetComponent<TextMeshProUGUI>();
        m_TxtRatingValue = rating.Find("Value").GetComponent<TextMeshProUGUI>();

        m_BtnDetail.onClick.AddListener(OnClickViewDetail);

        m_BtnVerified.gameObject.SetActive(data.ExpertProfile != null);
        m_BtnUnVerified.gameObject.SetActive(data.ExpertProfile == null);

        m_TxtName.text = $"Name: {data.Name}";
        m_TxtEmail.text = $"Email: {data.Email}";
        m_TxtGender.text = $"Gender: {data.Gender}";

        m_TxtOverallRating.text = $"Overall rating: --";
        m_TxtRatingValue.text = $"Not yet rated";

        if(data.ExpertRatings.Count > 0)
        {
            float overallRating = (float)data.ExpertRatings.Average(item => item?.Score);
            m_TxtOverallRating.text = $"Overall rating: {Math.Round(overallRating, 1)}";
            m_TxtRatingValue.text = $"{StarFromFloat(RoundToNearestHalf(overallRating))}";
        }
    }

    private string StarFromFloat(float number)
    {
        StringBuilder sb = new StringBuilder();

        float times = Mathf.Ceil(number);

        for (int i = 0; i < times; i++)
        {
            sb.Append("\ue838");
        }

        if (number - times == 0.5f)
        {
            sb.Append("\ue839");
        }

        return sb.ToString();
    }

    private float RoundToNearestHalf(float number)
    {
        return (float)(Math.Round(number * 2) / 2);
    }

    private void OnClickViewDetail()
    {
        throw new NotImplementedException();
    }
}
