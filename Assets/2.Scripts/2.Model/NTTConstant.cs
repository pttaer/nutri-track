
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
    public static string SCENE_MENU
    {
        get { return "NTTMenu"; }
    }
    public static string SCENE_LOGIN
    {
        get { return "NTTLogin"; }
    }
    public static string SCENE_HOME
    {
        get { return "NTTHome"; }
    }
    public static string SCENE_MY_HEALTH
    {
        get { return "NTTMyHealth"; }
    }
    public static string SCENE_STATISTIC
    {
        get { return "NTTStatistic"; }
    }
    public static string SCENE_PROFILE
    {
        get { return "NTTProfile"; }
    }
    public static string SCENE_SHOP
    {
        get { return "NTTShop"; }
    }
    public static string SCENE_CHAT
    {
        get { return "NTTChat"; }
    }
    public static string SCENE_WALLET_MAIN
    {
        get { return "NTTWalletMain"; }
    }
    public static string SCENE_REGISTER
    {
        get { return "NTTRegister"; }
    }

    public static string SCENE_WELCOME
    {
        get { return "NTTWelcome"; }
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

    public const string BASE_DOMAIN = "http://14.225.211.111:3000/v1/"; // base domain name, in case hosting service is changed

    public const string LOGIN_ROUTE = BASE_DOMAIN + "auth/login";
    public const string REGISTER_ROUTE = BASE_DOMAIN + "auth/register";
    public const string REGISTER_EXPERT_ROUTE = BASE_DOMAIN + "auth/register-expert";

    public const string BMI_RECORDS_ROUTE = BASE_DOMAIN + "bmi-records";
    public const string BMI_RECORDS_ROUTE_FORMAT = BASE_DOMAIN + "bmi-records/{0}";
    public const string BMI_RECORDS_ROUTE_GET_ALL_FORMAT = BASE_DOMAIN + "bmi-records?limit={0}&page={1}";
    public const string BMI_RECORDS_ROUTE_GET_ALL_SORT_FORMAT = BASE_DOMAIN + "bmi-records?sortBy={0}";
    public const string BMI_RECORDS_ROUTE_GET_ALL_SORT_LIMIT_FORMAT = BASE_DOMAIN + "bmi-records?sortBy={0}&limit={1}&page={2}";

    public const string DAILY_CAL_ROUTE = BASE_DOMAIN + "daily-cals";
    public const string DAILY_CAL_ROUTE_FORMAT = BASE_DOMAIN + "daily-cals/{0}";
    public const string DAILY_CAL_ROUTE_GET_ALL_FORMAT = BASE_DOMAIN + "daily-cals?limit={0}&page={1}";
    public const string DAILY_CAL_ROUTE_GET_ALL_SORT_FORMAT = BASE_DOMAIN + "daily-cals?sortBy={0}";
    public const string DAILY_CAL_ROUTE_GET_ALL_SORT_LIMIT_FORMAT = BASE_DOMAIN + "daily-cals?sortBy={0}&limit={1}&page={2}";

    public const string CAL_RECORDS_ROUTE = BASE_DOMAIN + "cal-records";
    public const string CAL_RECORDS_ROUTE_FORMAT = BASE_DOMAIN + "cal-records/{0}";

    public const string APPLICATION_ROUTE = BASE_DOMAIN + "applications/{0}";
    public const string APPLICATION_STATUS_ROUTE = BASE_DOMAIN + "applications/{0}/status";

    public const string USER_ROUTE = BASE_DOMAIN + "user/{0}";

    public const string FOOD_ROUTE = BASE_DOMAIN + "foods";

    #endregion API_URL

    #region ICON_CODE
    public const string ICON_CLOSE = "\ue5cd";
    public const string ICON_MENU = "\ue5d2";
    #endregion

    #region SERVICE_KEY
    public const string PARAM_DATE = "date";
    #endregion

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

    // unit constants
    public const string BMI_UNIT = "kg/m^2";

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
#elif UNITY_STANDALONE
    public static string BEARER_TOKEN = "";
#endif
    public static string BEARER_TOKEN_EDITOR;
}