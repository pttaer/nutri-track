using System;
using UnityEngine;
using UnityEngine.UI;

public class NTTGeneralView : MonoBehaviour
{
    private GameObject m_popupMessagePrefab;//prefab popup message
    [SerializeField] private GameObject m_FAMPopupPrefab;//prefab popup message
    private GameObject m_Popup;//prefab popup message
    private GameObject m_Loading;//prefab popup message

    private Transform m_canvasObj;//object canvas
    private Transform m_SorryText;//object canvas

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        // unregister event
        NTTControl.Api.ShowNTTPopupEvent -= ShowFAMPopup;
        NTTControl.Api.ShowFAMPopupInputEvent -= ShowFAMPopup;
        NTTControl.Api.OnLoadShowLoading -= ShowLoading;
        NTTControl.Api.OnLoadFailShowSorry -= ShowSorryTxt;
        NTTControl.Api.CheckingSchoolIdEvent -= CheckSchoolId;
    }

    private void Init()
    {
        // find reference
        m_canvasObj = transform.Find("Canvas");
        m_SorryText = transform.Find("Canvas/TxtSorry");
        m_Loading = transform.Find("Canvas/Loading").gameObject;
        //
        m_popupMessagePrefab = ResourceObject.GetResource<GameObject>(NTTConstant.CONFIG_PREFAB_POPUP_MESSAGE);

        // register event
        NTTControl.Api.ShowNTTPopupEvent += ShowFAMPopup;
        NTTControl.Api.ShowFAMPopupInputEvent += ShowFAMPopup;
        NTTControl.Api.OnLoadShowLoading += ShowLoading;
        NTTControl.Api.OnLoadFailShowSorry += ShowSorryTxt;
        NTTControl.Api.CheckingSchoolIdEvent += CheckSchoolId;
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

    private void ShowFAMPopup(string title, string content, string btnConfirmText, string btnElseText, Action onConfirm = null, Action onElse = null, Action onExit = null)
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

    private void ShowFAMPopup(string title, string content, string btnConfirmText, string btnElseText, Action<string> onConfirm = null, Action onElse = null, Action onExit = null, bool isShowInput = false)
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
}