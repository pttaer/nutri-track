using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TweenUtils
{
    #region COMPLEX

    public static void TweenComplexAnimation(Transform transform, Vector3 finalPos, Vector3 finalScale, Quaternion finalRot, Color finalColor, float duration, float overshoot = 1.5f, int numLoops = 2)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(finalPos, duration))
            .SetEase(Ease.OutElastic)
            .SetLoops(numLoops, LoopType.Yoyo);
        sequence.Join(transform.DOScale(finalScale, duration))
            .SetEase(Ease.OutElastic)
            .SetLoops(numLoops, LoopType.Yoyo);
        sequence.Join(transform.DORotateQuaternion(finalRot, duration))
            .SetEase(Ease.OutElastic)
            .SetLoops(numLoops, LoopType.Yoyo);
        sequence.Join(transform.GetComponent<SpriteRenderer>().DOColor(finalColor, duration))
            .SetEase(Ease.OutElastic)
            .SetLoops(numLoops, LoopType.Yoyo);
        sequence.OnComplete(() =>
        {
            TweenUtils.TweenComplexAnimation(transform, finalPos, finalScale, finalRot, finalColor, duration, overshoot, numLoops);
        });
        sequence.SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Restart)
            .SetDelay(duration);
        sequence.Append(transform.DOShakePosition(duration, overshoot, numLoops, 90, false))
            .SetEase(Ease.InOutQuad);
    }

    public static void TweenRipple(Transform transform, float duration, float strength, int vibrato, float randomness, bool fadeOut = true)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), duration / 4f))
            .SetEase(Ease.OutQuad);
        sequence.Join(transform.DOScale(new Vector3(1f + strength, 1f + strength, 1f), duration / 2f))
            .SetEase(Ease.OutQuad)
            .SetLoops(vibrato, LoopType.Restart)
            .SetRelative();
        sequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), duration / 4f))
            .SetEase(Ease.OutQuad);
        if (fadeOut)
        {
            sequence.Append(transform.DOScale(Vector3.zero, duration / 2f))
                .SetEase(Ease.InQuad);
        }
        sequence.OnUpdate(() =>
        {
            float randX = UnityEngine.Random.Range(-randomness, randomness) * strength;
            float randY = UnityEngine.Random.Range(-randomness, randomness) * strength;
            transform.localPosition = new Vector3(transform.localPosition.x + randX, transform.localPosition.y + randY, transform.localPosition.z);
        });
    }

    public static void TweenSquish(Transform transform, float duration, float strength, int vibrato, float randomness, bool fadeOut = true)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(1f, 1f - strength, 1f), duration / 4f))
            .SetEase(Ease.InOutQuad);
        sequence.Append(transform.DOScale(new Vector3(1f + strength, 1f - strength, 1f), duration / 2f))
            .SetEase(Ease.InOutQuad)
            .SetLoops(vibrato, LoopType.Yoyo)
            .SetRelative();
        sequence.Append(transform.DOScale(new Vector3(1f, 1f - strength, 1f), duration / 4f))
            .SetEase(Ease.InOutQuad);
        if (fadeOut)
        {
            sequence.Append(transform.DOScale(Vector3.zero, duration / 2f))
                .SetEase(Ease.InQuad);
        }
        sequence.OnUpdate(() =>
        {
            float randX = UnityEngine.Random.Range(-randomness, randomness) * strength;
            float randY = UnityEngine.Random.Range(-randomness, randomness) * strength;
            transform.localPosition = new Vector3(transform.localPosition.x + randX, transform.localPosition.y + randY, transform.localPosition.z);
        });
    }

    public static void TweenWave(Transform transform, float duration, float amplitude, float frequency)
    {
        float startY = transform.localPosition.y;
        float startZ = transform.localPosition.z;
        transform.DOLocalMoveY(amplitude, duration / 4f)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative();
        transform.DOLocalRotate(new Vector3(0f, 0f, -30f), duration / 8f)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveX(frequency, duration / 4f))
            .SetEase(Ease.OutQuad)
            .SetRelative();
        sequence.Append(transform.DOLocalMoveX(-2f * frequency, duration / 2f))
            .SetEase(Ease.InOutQuad)
            .SetRelative();
        sequence.Append(transform.DOLocalMoveX(frequency, duration / 4f))
            .SetEase(Ease.InQuad)
            .SetRelative();
        sequence.SetLoops(-1, LoopType.Restart);
        sequence.OnUpdate(() =>
        {
            float y = Mathf.Sin(Time.time * frequency) * amplitude + startY;
            float z = Mathf.Cos(Time.time * frequency) * amplitude + startZ;
            transform.localPosition = new Vector3(transform.localPosition.x, y, z);
        });
    }

    #endregion COMPLEX

    #region MOBILE

    public static void TweenPopIn(Transform transform, float duration, float scale = 0.5f)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, duration)
            .SetEase(Ease.OutExpo);
    }

    public static void TweenPopOut(Transform transform, float duration, float scale = 1.5f)
    {
        transform.DOScale(scale, duration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                transform.localScale = Vector3.zero;
                transform.gameObject.SetActive(false);
            });
    }

    public static void TweenFlipIn(Transform transform, float duration, bool fromLeft = true)
    {
        float fromAngle = fromLeft ? -90f : 90f;
        transform.rotation = Quaternion.Euler(0f, fromAngle, 0f);
        transform.DORotate(new Vector3(0f, 0f, 0f), duration)
            .SetEase(Ease.OutBack);
    }

    public static void TweenFlipOut(Transform transform, float duration, bool toLeft = true)
    {
        float toAngle = toLeft ? -90f : 90f;
        transform.DORotate(new Vector3(0f, toAngle, 0f), duration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                transform.gameObject.SetActive(false);
            });
    }

    public static void TweenPulse(Transform transform, float duration, float scale = 1.2f, float delay = 0f)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(scale, duration / 2f))
            .SetDelay(delay)
            .SetEase(Ease.OutQuad);
        sequence.Append(transform.DOScale(1f, duration / 2f))
            .SetEase(Ease.InQuad);
        sequence.SetLoops(-1);
    }

    public static void TweenCurve(Vector3 startPosition, Vector3 endPosition, Transform transform, float duration)
    {
        Vector3 middlePosition = (startPosition + endPosition) / 2f + new Vector3(0f, Screen.height / 3f, 0f);
        transform.position = startPosition;
        transform.DOMove(endPosition, duration)
            .SetEase(Ease.InOutQuad)
            .SetDelay(0.1f)
            .OnComplete(() =>
            {
                transform.gameObject.SetActive(false);
            });
        transform.DOScale(0f, duration / 2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOScale(1f, duration / 2f)
                    .SetEase(Ease.InQuad);
            });
        transform.DORotate(new Vector3(0f, 0f, -720f), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad);
        transform.DOJump(middlePosition, Screen.height / 6f, 1, duration / 3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOMove(endPosition, duration / 3f)
                    .SetEase(Ease.InQuad);
            });
    }

    public static void TweenSwipeIn(Transform transform, Vector3 direction, float duration)
    {
        transform.position += direction.normalized * Screen.width;
        transform.DOMove(transform.position - direction.normalized * Screen.width, duration)
            .SetEase(Ease.OutBack);
    }

    public static void TweenSwipeOut(Transform transform, Vector3 direction, float duration, Action callback = null)
    {
        transform.DOMove(transform.position + direction.normalized * Screen.width, duration)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
    }

    public static void TweenBounce(Transform transform, Vector3 direction, float duration, float strength)
    {
        transform.DOMove(transform.position + direction * strength, duration / 2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOMove(transform.position - direction * strength, duration / 2f)
                    .SetEase(Ease.InQuad);
            });
    }

    public static void TweenShake(Transform transform, Vector3 direction, float duration, float strength)
    {
        transform.DOShakePosition(duration, direction * strength);
    }

    public static void TweenFadeIn(CanvasGroup canvasGroup, float duration)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, duration)
            .SetEase(Ease.OutExpo);
    }

    public static void TweenFadeOut(CanvasGroup canvasGroup, float duration)
    {
        canvasGroup.DOFade(0f, duration)
            .SetEase(Ease.OutExpo);
    }

    public static void TweenShakePosition(Transform transform, float duration, float strength, int vibrato, float randomness, bool snapping)
    {
        Vector3 initialPosition = transform.position;
        transform.DOShakePosition(duration, strength, vibrato, randomness, snapping)
            .OnComplete(() => transform.position = initialPosition);
    }

    public static void TweenShakeRotation(Transform transform, float duration, float strength, int vibrato, float randomness, bool snapping)
    {
        Quaternion initialRotation = transform.rotation;
        transform.DOShakeRotation(duration, new Vector3(strength, strength, strength), vibrato, randomness, snapping)
            .OnComplete(() => transform.rotation = initialRotation);
    }

    public static void TweenShakeScale(Transform transform, float duration, float strength, int vibrato, float randomness, bool snapping)
    {
        Vector3 initialScale = transform.localScale;
        transform.DOShakeScale(duration, new Vector3(strength, strength, strength), vibrato, randomness, snapping)
            .OnComplete(() => transform.localScale = initialScale);
    }

    public static void Slide(GameObject obj, Vector3 targetPosition, float duration)
    {
        obj.transform.DOMove(targetPosition, duration);
    }

    public static void Scroll(ScrollRect scrollRect, float targetScrollValue, float duration)
    {
        scrollRect.verticalNormalizedPosition = targetScrollValue;
        scrollRect.DOVerticalNormalizedPos(targetScrollValue, duration);
    }

    #endregion MOBILE

    #region TEXT

    public static void TweenTextScale(TMP_Text text, float targetScale, float duration)
    {
        DOTween.To(() => text.fontSize, x => text.fontSize = x, targetScale, duration);
    }

    public static void TweenTextAlpha(TMP_Text text, float targetAlpha, float duration)
    {
        DOTween.To(() => text.color.a, x =>
        {
            Color color = text.color;
            color.a = x;
            text.color = color;
        }, targetAlpha, duration);
    }

    public static void TweenTextFade(TMP_Text text, float targetAlpha, float duration)
    {
        Color initialColor = text.color;
        text.DOFade(targetAlpha, duration)
            .OnComplete(() => text.color = initialColor);
    }

    public static void TweenTextOutlineWidth(TMP_Text text, float targetWidth, float duration)
    {
        DOTween.To(() => text.outlineWidth, x => text.outlineWidth = x, targetWidth, duration);
    }

    public static void TweenTextGradientColor(TMP_Text text, Gradient targetGradient, float duration)
    {
        DOTween.To(() => text.color, x => text.color = x, targetGradient.Evaluate(0), duration)
            .OnUpdate(() =>
            {
                for (int i = 0; i < text.textInfo.characterCount; i++)
                {
                    TMP_CharacterInfo charInfo = text.textInfo.characterInfo[i];
                    if (!charInfo.isVisible)
                    {
                        continue;
                    }

                    int materialIndex = charInfo.materialReferenceIndex;
                    int vertexIndex = charInfo.vertexIndex;

                    Color32 newColor = targetGradient.Evaluate((float)i / (text.textInfo.characterCount - 1)) * text.color;
                    text.textInfo.meshInfo[materialIndex].colors32[vertexIndex] = newColor;
                    text.textInfo.meshInfo[materialIndex].colors32[vertexIndex + 1] = newColor;
                    text.textInfo.meshInfo[materialIndex].colors32[vertexIndex + 2] = newColor;
                    text.textInfo.meshInfo[materialIndex].colors32[vertexIndex + 3] = newColor;
                }
                text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            });
    }

    public static void TweenTextPunchScale(TMP_Text text, Vector3 punch, float duration)
    {
        text.transform.DOPunchScale(punch, duration);
    }

    public static void TweenTextPunchRotation(TMP_Text text, Vector3 punch, float duration)
    {
        text.transform.DOPunchRotation(punch, duration);
    }

    public static void TweenTextPunchPosition(TMP_Text text, Vector3 punch, float duration)
    {
        text.transform.DOPunchPosition(punch, duration);
    }

    public static void TweenTextRandomChars(TMP_Text text, float duration)
    {
        char[] chars = text.text.ToCharArray();
        char[] randomChars = new char[chars.Length];

        for (int i = 0; i < chars.Length; i++)
        {
            randomChars[i] = (char)UnityEngine.Random.Range(33, 127); // ASCII characters between 33 and 126
        }

        DOTween.To(() => 0f, (x) =>
        {
            int numChars = Mathf.RoundToInt(x * chars.Length);
            text.text = new string(randomChars, 0, numChars) + new string(chars, numChars, chars.Length - numChars);
        }, 1f, duration);
    }

    public static void TweenTextTypewriter(TMP_Text text, float duration)
    {
        text.maxVisibleCharacters = 0;
        int numChars = text.text.Length;

        DOTween.To(() => 0, (x) =>
        {
            text.maxVisibleCharacters = x;
        }, numChars, duration)
        .SetEase(Ease.Linear);
    }

    public static void TypingAnimation(TextMeshProUGUI textMeshPro, string text, float duration, Action callback = null)
    {
        int chars = text.Length;
        float delay = duration / chars;

        textMeshPro.text = "";

        for (int i = 0; i < chars; i++)
        {
            int charIndex = i;
            char c = text[charIndex];

            DOTween.Sequence()
                .AppendInterval(delay * i)
                .AppendCallback(() =>
                {
                    textMeshPro.text += c;
                })
                .Play()
                .OnComplete(() => { callback?.Invoke(); });
        }
    }

    public static void TweenTextWiggle(TMP_Text text, float strength, float duration)
    {
        float originalY = text.rectTransform.localPosition.y;
        text.rectTransform.DOLocalMoveY(originalY + strength, duration / 6f)
            .SetEase(Ease.OutQuad)
            .SetLoops(5, LoopType.Yoyo)
            .OnComplete(() => text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, originalY, text.rectTransform.localPosition.z));
    }

    public static void TweenTextWave(TMP_Text text, float amplitude, float frequency, float duration)
    {
        float originalY = text.rectTransform.localPosition.y;
        text.rectTransform.DOShakePosition(duration, amplitude, vibrato: Mathf.RoundToInt(frequency))
            .OnComplete(() => text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, originalY, text.rectTransform.localPosition.z));
    }

    public static void TweenTextRainbow(TMP_Text text, float duration)
    {
        Color originalColor = text.color;
        Color[] rainbowColors = new Color[7]
        {
            Color.red,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            Color.magenta,
            Color.red,
        };

        int numRainbowColors = rainbowColors.Length;
        int currentColorIndex = 0;

        DOTween.To(() => currentColorIndex, x => currentColorIndex = x, numRainbowColors - 1, duration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                float t = (float)currentColorIndex / (numRainbowColors - 1);
                text.color = Color.Lerp(originalColor, rainbowColors[currentColorIndex], t);
            })
            .OnComplete(() => text.color = originalColor);
    }

    public static void TweenTextSwing(TMP_Text text, float strength, float duration)
    {
        float originalZ = text.rectTransform.localEulerAngles.z;
        text.rectTransform.DOLocalRotate(new Vector3(0, 0, strength), duration / 6f, RotateMode.Fast)
            .SetEase(Ease.OutQuad)
            .SetLoops(5, LoopType.Yoyo)
            .OnComplete(() => text.rectTransform.localEulerAngles = new Vector3(text.rectTransform.localEulerAngles.x, text.rectTransform.localEulerAngles.y, originalZ));
    }

    #endregion TEXT

    #region Color

    public static void TweenColor(GameObject obj, Color targetColor, float duration)
    {
        obj.GetComponent<SpriteRenderer>().DOColor(targetColor, duration);
    }

    public static void TweenSpriteColor(SpriteRenderer spriteRenderer, Color targetColor, float duration)
    {
        spriteRenderer.material.DOColor(targetColor, duration);
    }

    public static void TweenSpriteAlpha(SpriteRenderer spriteRenderer, float targetAlpha, float duration)
    {
        Color initialColor = spriteRenderer.material.color;
        spriteRenderer.material.DOFade(targetAlpha, duration)
            .OnComplete(() => spriteRenderer.material.color = initialColor);
    }

    public static void TweenComplexColorChange(SpriteRenderer spriteRenderer, Color fromColor, Color toColor, float duration, float delay, int loops)
    {
        DOTween.Sequence()
            .Append(spriteRenderer.material.DOColor(toColor, duration * 0.5f).From(fromColor).SetEase(Ease.InOutSine))
            .Append(spriteRenderer.material.DOColor(fromColor, duration * 0.5f).SetEase(Ease.InOutSine))
            .SetLoops(loops, LoopType.Yoyo)
            .SetDelay(delay);
    }

    #endregion Color

    public static void MoveBetweenPoints(GameObject obj, Vector3[] path, float duration, PathType pathType = PathType.Linear, Ease ease = Ease.Linear)
    {
        obj.transform.DOLocalPath(path, duration, pathType).SetEase(ease);
    }

    public static void TweenPathMove(Transform transform, Vector3[] path, float duration, PathType pathType, PathMode pathMode, int resolution = 10, Color? gizmoColor = null)
    {
        TweenerCore<Vector3, Path, PathOptions> tweener = transform.DOPath(path, duration, pathType, pathMode, resolution, gizmoColor);
        tweener.SetOptions(true);
    }
}