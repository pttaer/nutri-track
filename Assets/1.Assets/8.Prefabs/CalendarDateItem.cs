using UnityEngine;
using uPalette.Generated;
using UnityEngine.UI;
using uPalette.Runtime.Core;

public class CalendarDateItem : MonoBehaviour
{
    [SerializeField] Text m_Txt;
    [SerializeField] GameObject m_BG;
    public bool m_IsSunday;
    public bool m_IsChoosen;

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
