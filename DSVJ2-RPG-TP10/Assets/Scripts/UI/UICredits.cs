using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoaderManagerScript;
using UILoadingScreenScript;

namespace UICreditsScript
{
    public class UICredits : MonoBehaviour
    {
        public void LoadMenuScene()
        {
            LoaderManager.Get().LoadScene("MainMenu");
            UILoadingScreen.Get().SetVisible(true);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}