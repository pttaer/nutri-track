using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTExpertView : MonoBehaviour
{
    private NTTExpertItemView m_ExpertItemView;

    private List<NTTExpertItemView> m_ExpertList = new List<NTTExpertItemView>();

    void Start()
    {
        Transform scrollViewContent = transform.Find("Content/ScrollView/Viewport/Content");

        m_ExpertItemView = scrollViewContent.Find("ItemExpert").GetComponent<NTTExpertItemView>();

        StartCoroutine(NTTApiControl.Api.GetListData<NTTExpertDTO>(NTTConstant.EXPERT_ROUTE, (dataList) =>
        {
            foreach (var data in dataList.Results)
            {
                NTTExpertItemView itemView = Instantiate(m_ExpertItemView, scrollViewContent).GetComponent<NTTExpertItemView>();
                itemView.Init(data);
                m_ExpertList.Add(itemView);
            }
        }));
    }
}
