using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTTagListView : MonoBehaviour
{
    private NTTTagListItemView m_PrefItem;

    public List<string> Data;

    public void Init()
    {
        Data = new List<string>();
        m_PrefItem = transform.Find("Item").GetComponent<NTTTagListItemView>();
    }

    public void RemoveItem(string item)
    {
        if (Data.Contains(item))
        {
            Data.Remove(item);
            UpdateView();
        }
    }

    public void AddItem(string item)
    {
        if (!Data.Contains(item))
        {
            Data.Add(item);
            UpdateView();
        }
    }

    public void UpdateView()
    {
        if (Data.Count > 0)
        {
            foreach (Transform item in transform)
            {
                if (item.name == "Item(Clone)")
                {
                    Destroy(item.gameObject);
                }
            }

            foreach (var item in Data)
            {
                NTTTagListItemView spawnItem = Instantiate(m_PrefItem, transform);
                spawnItem.Init(item, RemoveItem);
            }

            transform.gameObject.SetActive(true);
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
    }
}
