using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTDietView : MonoBehaviour
{
    private NTTDietItemView m_DietItemView;

    private List<NTTDietItemView> m_DietList = new List<NTTDietItemView>();

    void Start()
    {
        Transform scrollViewContent = transform.Find("Content/ScrollView/Viewport/Content");

        m_DietItemView = scrollViewContent.Find("ItemDiet").GetComponent<NTTDietItemView>();

        StartCoroutine(NTTApiControl.Api.GetListData<NTTDietDTO>(NTTConstant.DIET_ROUTE, (dataList) =>
        {
            Debug.Log("Run here dataList " + JsonConvert.SerializeObject(dataList));
            foreach (var data in dataList.Results)
            {
                NTTDietItemView itemView = Instantiate(m_DietItemView, scrollViewContent).GetComponent<NTTDietItemView>();
                itemView.Init(data);
                m_DietList.Add(itemView);
            }
        }));
    }
}
