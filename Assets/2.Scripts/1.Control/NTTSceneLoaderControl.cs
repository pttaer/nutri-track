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

    /// <summary>
    /// To load and stack scenes, use this for multiple scenes stacking
    /// </summary>
    /// <param name="sceneName"></param>
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
        if (SceneStack.Count >= 1 && !m_ExcludedSceneNames.Contains(SceneStack.First()))
        {
            SceneManager.UnloadSceneAsync(SceneStack.First());

            SceneStack.Pop();
        }
    }

    /// <summary>
    /// To load a singular scene, not to have scenes stack
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSingularScene(string sceneName)
    {
        UnloadLastScene();
        LoadScene(sceneName);
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
