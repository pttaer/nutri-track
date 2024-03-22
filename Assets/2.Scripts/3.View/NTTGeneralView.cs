using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NTTGeneralView : MonoBehaviour
{
    private GameObject m_popupMessagePrefab;//prefab popup message
    [SerializeField] private GameObject m_FAMPopupPrefab;//prefab popup message
    private ScrollMechanic m_SelectorView;// PopupSelector
    private GameObject m_Popup;//prefab popup message
    private GameObject m_Loading;//prefab popup message
    private GameObject m_CalendarPopup;
    private CalendarController m_CalendarPopupView;
    private Transform m_canvasObj;//object canvas
    private Transform m_SorryText;//object canvas
    private TextMeshProUGUI m_DummyTxt;//object canvas

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
    }

    private void Init()
    {
        // find reference
        m_canvasObj = transform.Find("Canvas");
        m_SorryText = transform.Find("Canvas/TxtSorry");
        m_Loading = transform.Find("Canvas/Loading").gameObject;
        m_CalendarPopup = transform.Find("Canvas/CalendarPopup").gameObject;
        m_DummyTxt = transform.Find("Canvas/TxtDummy").GetComponent<TextMeshProUGUI>();
        m_CalendarPopupView = transform.Find("Canvas/CalendarPopup/BG/CalendarControllerPreb").GetComponent<CalendarController>();
        m_SelectorView = transform.Find("Canvas/PopupSelector/ScrollSystem").GetComponent<ScrollMechanic>();
        //
        m_popupMessagePrefab = ResourceObject.GetResource<GameObject>(NTTConstant.CONFIG_PREFAB_POPUP_MESSAGE);

        // register event
        NTTControl.Api.ShowNTTPopupEvent += ShowNTTPopup;
        NTTControl.Api.ShowFAMPopupInputEvent += ShowNTTPopup;
        NTTControl.Api.OnLoadShowLoading += ShowLoading;
        NTTControl.Api.OnLoadFailShowSorry += ShowSorryTxt;
        NTTControl.Api.CheckingSchoolIdEvent += CheckSchoolId;
        NTTControl.Api.OnCallShowPopupCalendarEvent += ShowCalendarPopup;
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
            m_Popup = Instantiate(m_FAMPopupPrefab, m_canvasObj);
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
            m_Popup = Instantiate(m_FAMPopupPrefab, m_canvasObj);
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
    }
}