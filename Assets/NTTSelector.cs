using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NTTSelector : MonoBehaviour
{
    private Button m_BtnSearch;
    private Button m_BtnSortByName;
    private Button m_BtnFilter;

    private NTTSelectorItemView m_PrefItemSelectorView;

    private List<NTTSelectorItemView> m_LsSelectorItem = new List<NTTSelectorItemView>();

    public void Init(List<NTTSelectorItemDTO> listItemData)
    {
        Transform topBar = transform.Find("SVContainer/TopBar");
        Transform scrollViewContent = transform.Find("SVContainer/Viewport/Content");

        m_BtnSearch = topBar.Find("BtnSearch").GetComponent<Button>();
        m_BtnSortByName = topBar.Find("BtnSortByName").GetComponent<Button>();
        m_BtnFilter = topBar.Find("BtnFilter").GetComponent<Button>();

        m_PrefItemSelectorView = scrollViewContent.Find("Item").GetComponent<NTTSelectorItemView>();

        m_BtnSearch.onClick.AddListener(OnSearchClicked);
        m_BtnSortByName.onClick.AddListener(OnSortByNameClicked);
        m_BtnFilter.onClick.AddListener(OnFilterClicked);

        GenerateSelectorItems(listItemData);
    }

    private void GenerateSelectorItems(List<NTTSelectorItemDTO> data)
    {
        m_PrefItemSelectorView.gameObject.SetActive(false);
        Transform prefItemTransform = m_PrefItemSelectorView.transform.parent;

        foreach (NTTSelectorItemDTO itemData in data)
        {
            GameObject selectorGO = Instantiate(m_PrefItemSelectorView.gameObject, prefItemTransform);
            m_LsSelectorItem.Add(selectorGO.GetComponent<NTTSelectorItemView>());
            m_LsSelectorItem.Last().Init(itemData);
        }
    }

    private void OnFilterClicked()
    {
        throw new NotImplementedException();
    }

    private void OnSortByNameClicked()
    {
        throw new NotImplementedException();
    }

    private void OnSearchClicked()
    {
        throw new NotImplementedException();
    }
}
