using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NTTMenu : MonoBehaviour
{
    private Transform m_TfSelected;
    private Transform m_Content;

    [SerializeField] List<Button> m_BtnList = new List<Button>();
    private const float TWEEN_MOVE_TIME = 0.3f;
    private const int DEFAULT_SELECT_TRANSFORM = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_TfSelected = transform.Find("ChosenImg");
        m_Content = transform.Find("Content");

        InitBtnList();
    }

    private void InitBtnList()
    {
        foreach (Transform tf in m_Content)
        {
            Button btn = tf.Find("Btn").GetComponent<Button>();
            btn.onClick.AddListener(() => MoveSelectedTo(btn.transform));
            m_BtnList.Add(btn);
        }

        // Default
        MoveSelectedTo(m_BtnList[DEFAULT_SELECT_TRANSFORM].transform, true);
    }

    private void ActiveAllButtonsImage()
    {
        foreach (var btn in m_BtnList)
        {
            btn.gameObject.SetActive(true);
        }
    }

    private void MoveSelectedTo(Transform tf, bool isDefault = false)
    {
        m_TfSelected.DOMoveX(tf.position.x, isDefault ? 0f : TWEEN_MOVE_TIME, true).OnComplete(() =>
        {
            ActiveAllButtonsImage();

            // Set current button off
            tf.gameObject.SetActive(false);
        });
    }
}
