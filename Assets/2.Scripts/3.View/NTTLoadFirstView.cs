using System.Collections;
using UnityEngine;

public class NTTLoadFirstView : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Application.runInBackground = true;//run in background
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//never sleep screen
        Application.targetFrameRate = 60;

        NTTControl.Api.Init();

#if PLATFORM_ANDROID
        //StartCoroutine(CheckForBackButton());
#endif
    }

    private IEnumerator CheckForBackButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if main or login on
            //if (!NTTControl.Api.CheckMainOrLoginOn())
            //{
            //    NTTControl.Api.BackToMainScene(unloadAll: true);
            //}
            //else
            //{
            //    Application.Quit();
            //}
        }
        yield return null;
    }
}