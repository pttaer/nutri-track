using System.Collections.Generic;
using UnityEngine;

public class NTTConstant
{
    #region SCENE_NAMES

    // Scene names

    public static string SCENE_LOADFIRST
    {
        get { return "NTTLoadFirst"; }
    }
    
    //TODO: UPDATE NEW SCENES

    #endregion SCENE_NAMES

    #region PREFABS

    // prefab

    public static string CONFIG_PREFAB_LOADING // name prefab loading
    {
        get { return "Loading"; }
    }

    public static string CONFIG_PREFAB_LOADING_BG // name prefab loading with background
    {
        get { return "LoadingBG"; }
    }

    public static string CONFIG_PREFAB_POPUP_MESSAGE // name prefab popup message
    {
        get { return "PopupMessage"; }
    }

    public static string CONFIG_PREFAB_FAM_POPUP // name prefab FAM popup
    {
        get { return "FAMPopup"; }
    }

    #endregion PREFABS

    #region FORMAT

    // format
    public static string FORMAT_DATETIME_12_HOURS
    {
        get { return "hh:mm tt"; }
    }

    public static string FORMAT_TIMESPAN_IN_DAY // format timespan 24h for timespan
    {
        get { return @"hh\:mm"; }
    }

    public static string FORMAT_TIMESPAN_DURATION_TO_HOUR // format timespan duration with total hours > 0
    {
        get { return @"hh\:mm\:ss"; }
    }

    public static string FORMAT_TIMESPAN_DURATION_TO_MINUTE // format timespan duration with total hours <= 0
    {
        get { return @"mm\:ss"; }
    }

    #endregion FORMAT

    #region API_URL

    public const string BASE_DOMAIN = ""; // base domain name, in case hosting service is changed
    public const string ROUTE = BASE_DOMAIN + ""; // service route

    #endregion API_URL

    // Methods
    public const string METHOD_GET = "GET";
    public const string METHOD_POST = "POST";
    public const string METHOD_PUT = "PUT";
    public const string METHOD_DELETE = "DELETE";
    public const string METHOD_PATCH = "PATCH";


    // playerprefs
    public const string EMAIL_CACHE = "email_cache";

    public const string PASSWORD_CACHE = "password_cache";

    public const string BEARER_TOKEN_CACHE = "bearer_token_cache";

    public const string PROFILE_CACHE = "profile_cache";

    public const string USER_ID = "user_id";

    public const string USER_EMAIL_CACHE = "user_email";

    public const string USER_FULLNAME_CACHE = "full_name";

    // tween constant
    public const string TIME_UNIT = " mins";

    public const float TWEEN_DURATION = 0.3f;

    // date format
    public const string DATE_FORMAT_SHORT = "dd MMM";

    public const string DATE_FORMAT_FULL = "d/M/yyyy";

    // colors
    public static Color MAIN_COLOR_GREEN { get => m_MainColor; }

    public static Color MAIN_LIGHT_COLOR_GREEN { get => m_MainLightColor; }

    private static Color m_MainColor = new(0f, 0.69f, 0.31f);

    private static Color m_MainLightColor = new(115f / 255f, 209f / 255f, 61f / 255f, 86f / 255f);

    private static Color32 m_RedTextColor = new(202, 33, 43, 255);

    private static Color32 m_GreenTextColor = new(34, 130, 34, 255);

#if UNITY_EDITOR
    public static string BEARER_TOKEN = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0IiwiZW1haWwiOiJ1c2VyMUBnbWFpbC5jb20iLCJyb2xlIjoiVXNlciIsIm5iZiI6MTY5NjQ3NzYwMCwiZXhwIjoxNjk2NzM2ODAwLCJpYXQiOjE2OTY0Nzc2MDB9.0wIsn-p-Cwtod9otv3cWMDO-Rx20LbsktHMDmZF9UgY";
#elif UNITY_ANDROID
    public static string BEARER_TOKEN = "";
#endif
    public static string BEARER_TOKEN_EDITOR;
}