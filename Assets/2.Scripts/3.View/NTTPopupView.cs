using System;
using UnityEngine;
using UnityEngine.UI;

public class NTTPopupView : MonoBehaviour
{
    private Button m_BtnConfirm;
    private Button m_BtnElse;
    private Button m_BtnExit;
    private Text m_TxtTitle;
    private Text m_TxtContent;
    private Text m_TxtBtnConfirm;
    private Text m_TxtBtnElse;
    private InputField m_IpfContent;

    private Action<string> m_OnConfirmWithInput;
    private Action m_OnConfirm;
    private Action m_OnElse;
    private Action m_OnExit;

    public void Init(string title, string content, string btnConfirmText, string btnElseText, Action onConfirm = null, Action onElse = null, Action onExit = null)
    {
        var container = transform.Find("Container");
        var buttonGroup = container.Find("ButtonGroup");
        var popupContent = container.Find("PopupContent");

        m_BtnConfirm = buttonGroup.Find("BtnConfirm").GetComponent<Button>();
        m_BtnElse = buttonGroup.Find("BtnElse").GetComponent<Button>();
        m_BtnExit = container.Find("m_BtnExit").GetComponent<Button>();
        m_TxtTitle = container.Find("TxtTitle").GetComponent<Text>();
        m_TxtContent = popupContent.Find("TxtContent").GetComponent<Text>();
        m_TxtBtnConfirm = buttonGroup.Find("BtnConfirm/TxtLabel").GetComponent<Text>();
        m_TxtBtnElse = buttonGroup.Find("BtnElse/TxtLabel").GetComponent<Text>();
        m_IpfContent = popupContent.Find("IpfContent").GetComponent<InputField>();

        m_BtnConfirm.onClick.AddListener(() => ConfirmTurnPopupOff());
        m_BtnElse.onClick.AddListener(OnElseClick);
        m_BtnExit.onClick.AddListener(ExitPopup);

        UpdatePopup(title, content, btnConfirmText, btnElseText, onConfirm, onElse, onExit);
    }

    public void UpdatePopup(string title, string content, string btnConfirmText, string btnElseText, Action onConfirm = null, Action onElse = null, Action onExit = null)
    {
        if (btnElseText == "NotShow") m_BtnElse.gameObject.SetActive(false);
        if (title == "Warning") m_TxtTitle.color = Color.red;
        if (btnConfirmText == "Delete" || btnConfirmText == "Decline") m_BtnConfirm.gameObject.GetComponent<Image>().color = Color.red;
        m_TxtTitle.text = title;
        m_TxtContent.text = content;
        m_TxtBtnConfirm.text = btnConfirmText;
        m_TxtBtnElse.text = btnElseText;
        m_OnConfirm = onConfirm;
        m_OnElse = onElse;
        m_OnExit = onExit;
    }

    public void Init(string title, string content, string btnConfirmText, string btnElseText, Action<string> onConfirm = null, Action onElse = null, Action onExit = null, bool isShowInputField = false)
    {
        var container = transform.Find("Container");
        var buttonGroup = container.Find("ButtonGroup");
        var popupContent = container.Find("PopupContent");

        m_BtnConfirm = buttonGroup.Find("BtnConfirm").GetComponent<Button>();
        m_BtnElse = buttonGroup.Find("BtnElse").GetComponent<Button>();
        m_BtnExit = container.Find("m_BtnExit").GetComponent<Button>();
        m_TxtTitle = container.Find("TxtTitle").GetComponent<Text>();
        m_TxtContent = popupContent.Find("TxtContent").GetComponent<Text>();
        m_TxtBtnConfirm = buttonGroup.Find("BtnConfirm/TxtLabel").GetComponent<Text>();
        m_TxtBtnElse = buttonGroup.Find("BtnElse/TxtLabel").GetComponent<Text>();
        m_IpfContent = popupContent.Find("IpfContent").GetComponent<InputField>();

        m_BtnConfirm.onClick.AddListener(() => ConfirmTurnPopupOff(m_IpfContent.text));
        m_BtnElse.onClick.AddListener(OnElseClick);
        m_BtnExit.onClick.AddListener(ExitPopup);

        UpdatePopup(title, content, btnConfirmText, btnElseText, onConfirm, onElse, onExit, isShowInputField);
    }

    public void UpdatePopup(string title, string content, string btnConfirmText, string btnElseText, Action<string> onConfirm = null, Action onElse = null, Action onExit = null, bool isShowInputField = false)
    {
        if (btnElseText == "NotShow") m_BtnElse.gameObject.SetActive(false);
        if (title == "Warning") m_TxtTitle.color = Color.red;
        if (btnConfirmText == "Delete" || btnConfirmText == "Decline") m_BtnConfirm.gameObject.GetComponent<Image>().color = Color.red;
        m_TxtTitle.text = title;
        m_TxtContent.text = content;
        m_TxtBtnConfirm.text = btnConfirmText;
        m_TxtBtnElse.text = btnElseText;
        m_OnConfirmWithInput = onConfirm;
        m_OnElse = onElse;
        m_OnExit = onExit;

        if (isShowInputField)
        {
            m_IpfContent.gameObject.SetActive(true);
            m_TxtContent.gameObject.SetActive(false);
        }
    }

    private void ConfirmTurnPopupOff()
    {
        m_OnConfirm?.Invoke();
        TurnPopupOff();
    }

    private void ConfirmTurnPopupOff(string input)
    {
        m_OnConfirmWithInput?.Invoke(input);
        TurnPopupOff();
    }

    private void OnElseClick()
    {
        m_OnElse?.Invoke();
        TurnPopupOff();
    }

    private void ExitPopup()
    {
        m_OnExit?.Invoke();
        TurnPopupOff();
    }

    private void TurnPopupOff()
    {
        gameObject.SetActive(false);
        m_BtnElse.gameObject.SetActive(true);
        m_IpfContent.gameObject.SetActive(false);
        m_TxtContent.gameObject.SetActive(true);
        m_BtnConfirm.gameObject.GetComponent<Image>().color = NTTConstant.MAIN_COLOR_GREEN;
    }
}