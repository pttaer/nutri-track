using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOverflowCheck : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> texts;
    [SerializeField] float scrollSpeed = 6f;

    private void Start()
    {
        foreach (var item in texts)
        {
            CheckScrollText(item);
        }
    }

    private void CheckScrollText(TextMeshProUGUI item)
    {
        if (item.preferredWidth > item.transform.parent.GetComponent<RectTransform>().sizeDelta.x)
        {
            SetText(item);
            StartCoroutine(ScrollText(item));
        }
    }

    public void AddScrollText(TextMeshProUGUI text)
    {
        texts.Add(text);
        CheckScrollText(text);
    }

    private void SetText(TextMeshProUGUI text)
    {
        RectTransform textRectTransform = text.GetComponent<RectTransform>();

        text.enableWordWrapping = false;
        text.overflowMode = TextOverflowModes.Overflow;

        TextMeshProUGUI cloneText = Instantiate(text);
        RectTransform cloneRectTransform = cloneText.GetComponent<RectTransform>();
        cloneRectTransform.SetParent(textRectTransform);
        cloneRectTransform.anchorMin = new Vector2(1, 0.5f);
        cloneRectTransform.localPosition = new Vector3(text.preferredWidth, 0, cloneRectTransform.position.z);
        cloneRectTransform.localScale = new Vector3(1, 1, 1);
        cloneText.text = text.text;
    }

    private IEnumerator ScrollText(TextMeshProUGUI text)
    {
        float width = text.preferredWidth;

        RectTransform textRectTransform = text.GetComponent<RectTransform>();

        Vector3 startPosition = textRectTransform.localPosition;

        float scrollPosition = 0;

        while (true)
        {
            textRectTransform.localPosition = new Vector3(-scrollPosition % width, startPosition.y, startPosition.z);
            scrollPosition += scrollSpeed * 20 * Time.deltaTime;
            yield return null;
        }
    }
}