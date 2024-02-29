using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NTTSceneLoaderControl : MonoBehaviour
{
    public static NTTSceneLoaderControl Api;

    public static Stack<string> SceneStack;

    private Stack<string> m_ExcludedSceneNames;


    public void Init()
    {
        SceneStack = new Stack<string>();
        m_ExcludedSceneNames = new Stack<string>();

        // INCLUDE EXCLUDED SCENES HERE
        m_ExcludedSceneNames.Push(NTTConstant.SCENE_LOADFIRST); // EXAMPLE
        m_ExcludedSceneNames.Push(NTTConstant.SCENE_MENU); // EXAMPLE
    }

    public void LoadScene(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneStack.Push(sceneName);

            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public void UnloadLastScene()
    {
        if (SceneStack.Count > 1)
        {
            SceneManager.UnloadSceneAsync(SceneStack.First());

            SceneStack.Pop();
        }
    }

    public void UnloadAllSceneExcept()
    {
        // UNLOAD ALL SCENES EXCEPT FOR EXCLUDED SCENES
        foreach (string sceneName in SceneStack)
        {
            if (!m_ExcludedSceneNames.Contains(sceneName))
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }
        // CLEAR THE STACK
        SceneStack.Clear();

        // PUSH BACK ALL THE EXCLUDED SCENES TO THE STACK
        foreach (string sceneName in m_ExcludedSceneNames)
        {
            SceneStack.Push(sceneName);
        }
    }
}
