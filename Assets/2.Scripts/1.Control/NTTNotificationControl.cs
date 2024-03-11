using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NTTNotificationControl
{
    public static NTTNotificationControl Api;

    private const string DEFAULT_CHANNEL_ID = "default_channel";

    public int ScheduleNotification(DateTime triggerDateTime, string title, string text, string intentData, TimeSpan? repeatInterval)
    {
        if (triggerDateTime < DateTime.Now)
        {
            return -1;
        }
        return AndroidNotificationCenter.SendNotification(CreateUnityNotification(triggerDateTime, title, text, intentData, repeatInterval, DEFAULT_CHANNEL_ID), DEFAULT_CHANNEL_ID);
    }

    private AndroidNotification CreateUnityNotification(DateTime triggerDateTime, string title, string text, string intentData, TimeSpan? repeatInterval, string defaultChannelId)
    {
        TimeSpan durationToTimeReminder = triggerDateTime - DateTime.Now;

        var defaultNotification = new AndroidNotificationChannel()
        {
            Id = defaultChannelId,
            Name = "Default_Channel",
            Description = "Generic notification",
            Importance = Importance.Default
        };
        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotification);

        AndroidNotification notification = new AndroidNotification()
        {
            Title = title,
            Text = text,
            SmallIcon = "app_icon_small",
            LargeIcon = "app_icon_large",
            FireTime = triggerDateTime,
            //ShouldAutoCancel = true,
            IntentData = intentData,
            RepeatInterval = repeatInterval
        };
        return notification;
    }
}
