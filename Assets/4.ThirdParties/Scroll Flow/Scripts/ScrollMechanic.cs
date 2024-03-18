using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class ScrollMechanic : MonoBehaviour, IDropHandler, IDragHandler, IBeginDragHandler, IPointerExitHandler, IPointerEnterHandler
{
    [Header("Test variables")]
    public bool initTest; //Test initialization
    public bool m_IsInfinite; //Is infinite scrolling (Required initialization)
    public string[] testData; //Test data

    [Header("Text prefab")]
    public GameObject templateValues;

    [Header("Required objects")]
    public Camera camera; //Main camera
    public RectTransform m_TargetCanvas; //Target canvas

    public RectTransform m_RtContent; //Target content
    public Transform m_TfContent; //Target content's transform
    public AutoSizeLayoutScrollFlow contentSize; //My own layout group script. You could use it instead of default layout group

    [Header("Settings")]
    [Space(20)]
    public float m_HeightTemplate = 27; //Height of template rect texts

    public AnimationCurve curve; //Curve for controlling "Shape" of scroll
    public AnimationCurve curveShift; //Curve for controlling text offset

    public float speedLerp = 5; //Speed of concentrating
    public float minVelocity = 0.2f; //Minimun inertion value to start concentrating

    public float shiftUp = 32; //Offset of upper texts
    public float shiftDown = 32; //Offset of lower texts
    public float padding = 0; //Spacing from upper and lower borders
    [Range(0, 1)]
    public float colorPad = 0.115f; //Padding of text color
    public float maxFontSize = 48.2f; //Maximun font size

    public bool m_IsElastic = true; //Is elastic movement
    public float maxElastic = 50; //Maximun elasity distance

    public float inertiaSense = 4; //Inertia sensibility

    [Header("Mouse Wheel and Touchpad scroll methods")]
    public bool isCanUseMouseWheel;
    public bool isInvertMouseWheel;
    public float mouseWheelSensibility = 0.5f;
    public float touchpadSensibility = 0.5f;

    bool m_IsDragging;
    float m_Inertia;

    float m_StartPosContent;
    float m_StartPosMouse;
    float m_Middle;
    float m_HeightText = 27;

    int m_CountCheck = 4;

    int m_CurrentCenter;

    bool m_IsInitialized;

    int m_CountTotal;

    int m_PadCount;

    float m_PadScroll;

    public float MouseScroll
    {
        get
        {
            float mouseScroll = Input.mouseScrollDelta.y;

            if (mouseScroll != 0)
            {
                return mouseScroll;
            }
            else
            {
                return m_PadScroll;
            }
        }
    }

    //Get TrackPad Scroll
    void OnGUI()
    {
        if (Event.current.type == EventType.ScrollWheel)
        {
            m_PadScroll = (-Event.current.delta.y / 10) * touchpadSensibility;
        }
        else
        {
            m_PadScroll = 0;
        }
    }

    private void Start()
    {
        m_HeightText = m_HeightTemplate / 2;
        m_Middle = GetComponent<RectTransform>().sizeDelta.y / 2;
        contentSize.topPad = m_Middle - m_HeightText;
        contentSize.bottomPad = m_Middle - m_HeightText;
        m_CountCheck = Mathf.CeilToInt((m_Middle * 2) / m_HeightTemplate);
        m_TfContent = m_RtContent.transform;
    }

    /// <summary>
    /// Initialization method
    /// </summary>
    /// <param name="dataToInit"> List of texts to show </param>
    /// <param name="isInfinite"> Is scroll will be infinite </param>
    /// <param name="firstTarget"> Which text in list will be first </param>
    public void Initialize(List<string> dataToInit, bool isInfinite = false, int firstTarget = 0)
    {
        m_CountTotal = dataToInit.Count;
        for (int i = 0; i < GetContentChildCount(); i++)
        {
            Destroy(m_RtContent.GetChild(i).gameObject);
        }

        this.m_IsInfinite = isInfinite;

        int half = (int)(m_CountCheck / 2) + 1;
        int numberOfItems = Mathf.CeilToInt((float)half / (float)m_CountTotal);

        if (isInfinite)
        {
            if (m_CountTotal > half)
            {
                m_PadCount = half;
                GenerateItemFromData(dataToInit, m_CountTotal, m_CountTotal - half);
            }
            else
            {
                m_PadCount = m_CountTotal;
                CallbackLoop(numberOfItems, () =>
                {
                    GenerateItemFromData(dataToInit, m_CountTotal);
                });
            }
            m_IsElastic = false;
            SetContentAnchor(new Vector2(0, (firstTarget + m_PadCount) * m_HeightTemplate));
        }
        else
        {
            m_PadCount = half;
            SetContentAnchor(new Vector2(0, firstTarget * m_HeightTemplate));
        }

        GenerateItemFromData(dataToInit, m_CountTotal);

        if (isInfinite)
        {
            if (m_CountTotal > half)
            {
                GenerateItemFromData(dataToInit, half);
            }
            else
            {
                CallbackLoop(numberOfItems, () =>
                {
                    GenerateItemFromData(dataToInit, m_CountTotal);
                });
            }
        }

        contentSize.UpdateLayout();
        m_IsInitialized = true;
    }

    private void GenerateItemFromData(List<string> dataToInit, int countLoop, int startIndex = 0)
    {
        for (int i = startIndex; i < countLoop; i++)
        {
            var textComponent = GetTxtFromFirstChild(Instantiate(templateValues, m_TfContent).transform);
            var tfTxtParent = textComponent.transform.parent;

            textComponent.text = dataToInit[i];
            tfTxtParent.name = i + "";
            tfTxtParent.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, m_HeightTemplate);
        }
    }
    
    private void CallbackLoop(int maxLoop, Action callback)
    {
        for (int j = 0; j < maxLoop; j++)
        {
            callback?.Invoke();
        }
    }

    /// <summary>
    /// Return list ID of current concentration
    /// </summary>
    /// <returns></returns>
    public int GetCurrentValue()
    {
        return int.Parse(m_RtContent.GetChild(m_CurrentCenter).name);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            m_IsDragging = false;
        }

        if (isCanUseMouseWheel && isInArea && Input.mouseScrollDelta.y != 0)
        {
            m_IsDragging = true;
        }
        else if (!Input.GetMouseButton(0))
        {
            m_IsDragging = false;
        }

        if (initTest)
        {
            initTest = false;
            var newList = new List<string>();
            for (int i = 0; i < testData.Length; i++)
            {
                newList.Add(testData[i]);
            }
            Initialize(newList, m_IsInfinite);
        }

        if (m_IsInitialized)
        {
            float adjustedRTSizeDeltaY = GetRTSizeDeltaY(m_RtContent) - m_Middle * 2;

            float contentYPosOnElastic = Mathf.Abs(GetContentYPos()) / maxElastic;
            float contentYPosOnElasticAdjusted = Mathf.Abs(adjustedRTSizeDeltaY - GetContentYPos()) / maxElastic;

            if (!m_IsDragging)
            {
                float contentPosWithInertia = GetContentYPos() + m_Inertia;

                if (contentPosWithInertia < 0)
                {
                    if (m_IsElastic)
                    {
                        SetContentAnchor(new Vector2(0, contentPosWithInertia));
                        SetInertia(m_Inertia * ClampReverseValueFromZeroToOne(contentYPosOnElastic));
                    }
                    else
                    {
                        SetContentAnchor(new Vector2(0, 0));
                        SetDefaultInertia();
                    }
                }
                else if (contentPosWithInertia > adjustedRTSizeDeltaY)
                {
                    if (m_IsElastic)
                    {
                        SetContentAnchor(new Vector2(0, contentPosWithInertia));
                        SetInertia(m_Inertia * ClampReverseValueFromZeroToOne(contentYPosOnElasticAdjusted));
                    }
                    else
                    {
                        SetContentAnchor(new Vector2(0, adjustedRTSizeDeltaY));
                        SetDefaultInertia();
                    }
                }
                else
                {
                    SetContentAnchor(new Vector2(0, contentPosWithInertia));
                    SetInertia(Mathf.Lerp(m_Inertia, 0, inertiaSense * Time.deltaTime));
                }
            }
            else
            {
                if (isCanUseMouseWheel && isInArea && MouseScroll != 0)
                {
                    float invertMouseAmount = isInvertMouseWheel ? -1 : 1;
                    float mouseScrollSense = MouseScroll * mouseWheelSensibility;
                    float currentYPosContent = GetContentYPos() + (invertMouseAmount * mouseScrollSense);

                    if (m_IsElastic)
                    {
                        if (GetContentYPos() < 0)
                        {
                            SetDefaultInertia();
                            SetContentAnchor(new Vector2(0, currentYPosContent * ClampReverseValueFromZeroToOne(contentYPosOnElastic)));
                        }
                        else if (GetContentYPos() > adjustedRTSizeDeltaY)
                        {
                            SetDefaultInertia();
                            SetContentAnchor(new Vector2(0, currentYPosContent * ClampReverseValueFromZeroToOne(contentYPosOnElasticAdjusted)));
                        }
                        else
                        {
                            SetInertia(m_Inertia + invertMouseAmount * mouseScrollSense);
                            SetContentAnchor(new Vector2(0, currentYPosContent));
                        }
                    }
                    else
                    {
                        SetInertia(m_Inertia + invertMouseAmount * mouseScrollSense);
                        SetContentAnchor(new Vector2(0, Mathf.Clamp(currentYPosContent, 0, adjustedRTSizeDeltaY)));
                    }
                }
                else
                {
                    float currentYPosContent = m_StartPosContent + (-m_StartPosMouse + GetNormalizeMousePosY());
                    if (m_IsElastic)
                    {
                        if (GetContentYPos() < 0)
                        {
                            SetDefaultInertia();
                            SetContentAnchor(new Vector2(0, currentYPosContent * ClampReverseValueFromZeroToOne(contentYPosOnElastic)));
                        }
                        else if (GetContentYPos() > adjustedRTSizeDeltaY)
                        {
                            SetDefaultInertia();
                            SetContentAnchor(new Vector2(0, currentYPosContent * ClampReverseValueFromZeroToOne(contentYPosOnElasticAdjusted)));
                        }
                        else
                        {
                            SetInertia(currentYPosContent - GetContentYPos());
                            SetContentAnchor(new Vector2(0, currentYPosContent));
                        }

                        m_StartPosMouse = GetNormalizeMousePosY();
                        m_StartPosContent = GetContentYPos();
                    }
                    else
                    {
                        SetInertia(currentYPosContent - GetContentYPos());
                        SetContentAnchor(new Vector2(0, Mathf.Clamp(currentYPosContent, 0, adjustedRTSizeDeltaY)));
                    }
                }
            }

            if (m_IsInfinite)
            {
                float contentCount = m_PadCount + (m_CountTotal - m_PadCount);
                float fullsizeContent = contentCount * m_HeightTemplate;

                bool cotentSmallerThanMiddle = GetContentYPos() < m_Middle;
                bool contentGreaterThanMiddle = GetContentYPos() > GetRTSizeDeltaY(m_RtContent) - m_Middle * 3;

                if (cotentSmallerThanMiddle)
                {
                    SetContentAnchor(new Vector2(0, GetContentYPos() + fullsizeContent));
                    for (int i = 0; i < contentCount; i++)
                    {
                        GetTxtFromChildIndex(i).fontSize = 0;
                    }
                    m_StartPosMouse = GetNormalizeMousePosY();
                    m_StartPosContent = GetContentYPos();
                }
                else if (contentGreaterThanMiddle)
                {
                    SetContentAnchor(new Vector2(0, GetContentYPos() - fullsizeContent));
                    for (int i = (int)(m_RtContent.childCount - contentCount); i < m_RtContent.childCount; i++)
                    {
                        GetTxtFromChildIndex(i).fontSize = 0;
                    }
                    m_StartPosMouse = GetNormalizeMousePosY();
                    m_StartPosContent = GetContentYPos();
                }
            }

            float contentPos = GetContentYPos();

            int startPoint = Mathf.CeilToInt((contentPos - (m_Middle + m_HeightText)) / m_HeightTemplate);
            int minID = Mathf.Max(0, startPoint);
            int maxID = Mathf.Min(m_TfContent.childCount, startPoint + m_CountCheck + 1);
            minID = Mathf.Clamp(minID, 0, int.MaxValue);
            maxID = Mathf.Clamp(maxID, 0, int.MaxValue);

            m_CurrentCenter = Mathf.Clamp(Mathf.RoundToInt(contentPos / m_HeightTemplate), 0, GetContentChildCount() - 1);

            if (maxID > minID)
            {
                for (int i = minID; i < maxID; i++)
                {
                    var currentRect = m_TfContent.GetChild(i).GetComponent<RectTransform>();
                    var currentText = GetTxtFromChildIndex(i);
                    var rtCurrentText = currentText.GetComponent<RectTransform>();

                    float contentSizeWithMiddle = contentPos + currentRect.anchoredPosition.y + m_Middle;
                    float ratio = Mathf.Clamp(1 - Mathf.Abs(contentSizeWithMiddle) / (m_Middle - padding), 0, 1);

                    float curveEvaluated = curveShift.Evaluate(1 - ratio);

                    rtCurrentText.anchoredPosition = new Vector2(0, contentSizeWithMiddle > 0 
                        ? -curveEvaluated * shiftUp 
                        : curveEvaluated * shiftDown);

                    currentText.fontSize = maxFontSize * curve.Evaluate(ratio);
                    currentText.color = new Vector4(currentText.color.r, currentText.color.g, currentText.color.b, Mathf.Clamp((ratio - colorPad) / (1 - colorPad), 0, 1));
                }
            }

            if (Mathf.Abs(m_Inertia) < minVelocity && !Input.GetMouseButton(0))
            {
                SetDefaultInertia();

                float currentItemCenterYPos = m_TfContent.GetChild(m_CurrentCenter).GetComponent<RectTransform>().anchoredPosition.y;

                SetContentAnchor(new Vector2(0, Mathf.Lerp(GetContentYPos(), - currentItemCenterYPos - m_Middle, speedLerp * Time.deltaTime)));
            }
        }
    }

    private float ClampReverseValueFromZeroToOne(float clampValue)
    {
        return Mathf.Clamp(1 - clampValue, 0, 1);
    }

    private void SetContentAnchor(Vector2 pos)
    {
        m_RtContent.anchoredPosition = pos;
    }
    
    private TextMeshProUGUI GetTxtFromChildIndex(int index)
    {
        return GetTxtFromFirstChild(m_TfContent.GetChild(index));
    }
    
    private TextMeshProUGUI GetTxtFromFirstChild(Transform t)
    {
        return t.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private float GetRTSizeDeltaY(RectTransform rt)
    {
        return rt.sizeDelta.y;
    }
    
    private float GetContentYPos()
    {
        return m_RtContent.anchoredPosition.y;
    }
    
    private int GetContentChildCount()
    {
        return m_RtContent.childCount;
    }

    private void SetDefaultInertia()
    {
        SetInertia(0);
    }
    
    private void SetInertia(float value)
    {
        m_Inertia = value;
    }

    public void OnDrop(PointerEventData eventData)
    {
        m_IsDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_IsDragging = true;
        m_StartPosMouse = GetNormalizeMousePosY();
        m_StartPosContent = GetContentYPos();
    }

    private float GetNormalizeMousePosY()
    {
        return Input.mousePosition.y / camera.pixelHeight * GetRTSizeDeltaY(m_TargetCanvas);
    }

    bool isInArea;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInArea = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInArea = false;
    }
}