using DG.Tweening;
using System;

public class NTTDashboardControl
{
    public static NTTDashboardControl Api;
    public Action<bool, Tween> OnMenuExpandEvent;
    public Action<string> OnUpdateIconEvent;

    public void OnMenuExpand(bool isExpanded, Tween firstSquence)
    {
        OnMenuExpandEvent?.Invoke(isExpanded, firstSquence);
    }

    public void OnUpdateIcon(string iconCode)
    {
        OnUpdateIconEvent?.Invoke(iconCode);
    }
}
