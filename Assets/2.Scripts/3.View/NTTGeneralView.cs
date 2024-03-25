using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NTTGeneralView : MonoBehaviour
{
    private GameObject m_popupMessagePrefab;//prefab popup message
    private GameObject m_FAMPopupPrefab;//prefab popup message
    private GameObject m_Popup;//prefab popup message
    private GameObject m_Loading;//prefab popup message
    private GameObject m_CalendarPopup;

    private ScrollMechanic m_SelectorView;// PopupSelector
    private CalendarController m_CalendarPopupView;

    [SerializeField] Camera m_Camera;

    private RectTransform m_RtCanvas;

    private Transform m_CanvasObj;
    private Transform m_SorryText;

    private TextMeshProUGUI m_DummyTxt;

    private Button m_BtnSelect;

    private Action<string> m_CallbackSelect;


    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        // unregister event
        NTTControl.Api.ShowNTTPopupEvent -= ShowNTTPopup;
        NTTControl.Api.ShowFAMPopupInputEvent -= ShowNTTPopup;
        NTTControl.Api.OnLoadShowLoading -= ShowLoading;
        NTTControl.Api.OnLoadFailShowSorry -= ShowSorryTxt;
        NTTControl.Api.CheckingSchoolIdEvent -= CheckSchoolId;
        NTTControl.Api.OnCallShowPopupCalendarEvent -= ShowCalendarPopup;
        NTTControl.Api.OnCallShowSelector -= ShowSelector;
    }

    private void Init()
    {
        // find reference
        m_RtCanvas = transform.Find("Canvas").GetComponent<RectTransform>();
        m_CanvasObj = transform.Find("Canvas");
        m_SorryText = transform.Find("Canvas/TxtSorry");
        m_Loading = transform.Find("Canvas/Loading").gameObject;
        m_CalendarPopup = transform.Find("Canvas/CalendarPopup").gameObject;
        m_DummyTxt = transform.Find("Canvas/TxtDummy").GetComponent<TextMeshProUGUI>();
        m_CalendarPopupView = transform.Find("Canvas/CalendarPopup/BG/CalendarControllerPreb").GetComponent<CalendarController>();
        m_SelectorView = transform.Find("Canvas/PopupSelector/ScrollSystem").GetComponent<ScrollMechanic>();
        m_BtnSelect = transform.Find("Canvas/PopupSelector/BtnSelect").GetComponent<Button>();
        //
        m_popupMessagePrefab = ResourceObject.GetResource<GameObject>(NTTConstant.CONFIG_PREFAB_POPUP_MESSAGE);

        m_BtnSelect.onClick.AddListener(ClosePopupSelector);

        // register event
        NTTControl.Api.ShowNTTPopupEvent += ShowNTTPopup;
        NTTControl.Api.ShowFAMPopupInputEvent += ShowNTTPopup;
        NTTControl.Api.OnLoadShowLoading += ShowLoading;
        NTTControl.Api.OnLoadFailShowSorry += ShowSorryTxt;
        NTTControl.Api.CheckingSchoolIdEvent += CheckSchoolId;
        NTTControl.Api.OnCallShowPopupCalendarEvent += ShowCalendarPopup;
        NTTControl.Api.OnCallShowSelector += ShowSelector;
    }

    private void ClosePopupSelector()
    {
        m_SelectorView.transform.parent.gameObject.SetActive(false);
        m_CallbackSelect?.Invoke(m_SelectorView.GetCurrentSelectedTxt());
    }

    private void ShowCalendarPopup(TextMeshProUGUI targetTxt, bool isClosePopup = false)
    {
        if (isClosePopup)
        {
            m_CalendarPopup.gameObject.SetActive(false);
        }
        else
        {
            m_CalendarPopup.gameObject.SetActive(true);
            m_CalendarPopupView.Init(targetTxt);
            m_CalendarPopupView.ShowExitFinishPopup(true);
            m_CalendarPopupView.gameObject.SetActive(true);
        }
    }

    private void ShowSelector(List<string> itemTxtList, Action<string> callbackSelect = null)
    {
        m_CallbackSelect = callbackSelect;
        m_SelectorView.gameObject.SetActive(true);
        m_SelectorView.transform.parent.gameObject.SetActive(true);
        m_SelectorView.Init(itemTxtList, m_Camera, canvas: m_RtCanvas, false);
    }

    public void ShowSorryTxt(bool isShow, string txtShow = null)
    {
        m_SorryText.gameObject.SetActive(isShow);
        if (isShow)
        {
            TweenUtils.TweenPopIn(m_SorryText, 0.5f, 1f);
        }
        m_SorryText.GetComponent<Text>().text = txtShow ?? m_SorryText.GetComponent<Text>().text;
    }

    public void ShowLoading(bool isShow)
    {
        m_Loading.gameObject.SetActive(isShow);
    }

    private void ShowNTTPopup(string title, string content, string btnConfirmText, string btnElseText, Action onConfirm = null, Action onElse = null, Action onExit = null)
    {
        if (m_Popup == null)
        {
            m_Popup = Instantiate(m_FAMPopupPrefab, m_CanvasObj);
            m_Popup.GetComponent<NTTPopupView>().Init(title, content, btnConfirmText, btnElseText, onConfirm, onElse, onExit);
        }
        else
        {
            m_Popup.gameObject.SetActive(true);
            m_Popup.GetComponent<NTTPopupView>().UpdatePopup(title, content, btnConfirmText, btnElseText, onConfirm, onElse, onExit);
        }
    }

    private void ShowNTTPopup(string title, string content, string btnConfirmText, string btnElseText, Action<string> onConfirm = null, Action onElse = null, Action onExit = null, bool isShowInput = false)
    {
        if (m_Popup == null)
        {
            m_Popup = Instantiate(m_FAMPopupPrefab, m_CanvasObj);
            m_Popup.GetComponent<NTTPopupView>().Init(title, content, btnConfirmText, btnElseText, onConfirm, onElse, onExit, isShowInput);
        }
        else
        {
            m_Popup.gameObject.SetActive(true);
            m_Popup.GetComponent<NTTPopupView>().UpdatePopup(title, content, btnConfirmText, btnElseText, onConfirm, onElse, onExit, isShowInput);
        }
    }

    private void CheckSchoolId()
    {
        //StartCoroutine(FAMApiControl.Api.CheckSchoolId());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ShowCalendarPopup(m_DummyTxt);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            ShowSelector(new List<string>() { "cals", "kcals" });
        }
    }
}