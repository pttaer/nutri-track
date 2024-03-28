using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NTTHomeView : MonoBehaviour
{
    private Button m_BtnDiet;
    private Button m_BtnExpert;
    private Button m_BtnDailyProgressUpdate;
    private Button m_BtnFoodList;

    private LayoutElement m_LETips;

    void Start()
    {
        Transform content = transform.Find("Content");
        Transform scrollContent = content.Find("ScrollView/Viewport/Content");
        Transform pnlTips = content.Find("PnlTips");

        m_BtnDiet = scrollContent.Find("BtnDiet/Btn").GetComponent<Button>();
        m_BtnExpert = scrollContent.Find("BtnExpert/Btn").GetComponent<Button>();
        m_BtnDailyProgressUpdate = scrollContent.Find("BtnDailyProgressUpdate/Btn").GetComponent<Button>();
        m_BtnFoodList = scrollContent.Find("BtnFoodList/Btn").GetComponent<Button>();

        m_LETips = pnlTips.GetComponent<LayoutElement>();

        m_BtnDiet.onClick.AddListener(OnClickDiet);
        m_BtnExpert.onClick.AddListener(OnClickExpert);
        m_BtnDailyProgressUpdate.onClick.AddListener(OnClickProgressUpdate);
        m_BtnFoodList.onClick.AddListener(OnClickFoodList);

        m_LETips.gameObject.SetActive(false);
    }

    private void OnClickFoodList()
    {
        throw new NotImplementedException();
    }

    private void OnClickProgressUpdate()
    {
        throw new NotImplementedException();
    }

    private void OnClickExpert()
    {
        throw new NotImplementedException();
    }

    private void OnClickDiet()
    {
        NTTSceneLoaderControl.Api.LoadScene(NTTConstant.SCENE_DIET);
    }
}
