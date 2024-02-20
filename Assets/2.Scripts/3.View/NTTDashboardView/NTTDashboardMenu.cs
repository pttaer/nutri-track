using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NTTDashboardMenu : MonoBehaviour
{
    private Button m_BtnExpand;
    private List<NTTDashboardMenuItem> m_MenuItems;
    private NTTMaterialIconView m_MIconBtnExpand;

    private LayoutElement m_Layout;

    private bool m_IsExpanded = true;

    public void Init()
    {
        m_BtnExpand = transform.Find("Body/BtnExpand").GetComponent<Button>();
        m_MIconBtnExpand = m_BtnExpand.transform.Find("Icon/Img").GetComponent<NTTMaterialIconView>();
        m_Layout = transform.GetComponent<LayoutElement>();

        m_MenuItems = transform.Find("Body").GetComponentsInChildren<NTTDashboardMenuItem>(false).ToList();

        m_MenuItems.ForEach(item =>
        {
            item.Init();
        });
        m_MIconBtnExpand.Init();

        m_BtnExpand.onClick.AddListener(OnClickExpand);
    }

    private void OnClickExpand()
    {
        Tween tween1 = TweenUtils.TweenLerpFloat(() => m_Layout.preferredWidth, x => m_Layout.preferredWidth = x, m_IsExpanded ? 100f : 200f, 0.5f, () =>
        {
            m_IsExpanded = !m_IsExpanded;
            NTTDashboardControl.Api.OnUpdateIcon(m_IsExpanded ? NTTConstant.ICON_CLOSE : NTTConstant.ICON_MENU);
        });
        NTTDashboardControl.Api.OnMenuExpand(!m_IsExpanded, tween1);
    }
}
