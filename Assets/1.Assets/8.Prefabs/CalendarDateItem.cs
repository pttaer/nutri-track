using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalendarDateItem : MonoBehaviour 
{
    [SerializeField] Text m_Txt;
    [SerializeField] GameObject m_BG;

    public void OnDateItemClick()
    {
        CalendarController.Api.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
        m_Txt.color = m_Txt.color == Color.black ? Color.white : Color.black;
        m_BG.SetActive(!m_BG.activeSelf);
    }

    public void SetItemOff()
    {
        if (m_BG.activeSelf)
        {
            m_Txt.color = Color.black;
            m_BG.SetActive(false);
        }
    }
}
