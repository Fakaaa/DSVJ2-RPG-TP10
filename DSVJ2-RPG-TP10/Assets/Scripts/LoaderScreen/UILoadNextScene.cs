using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoaderManagerScript;
using UILoadingScreenScript;

namespace UILoadNextSceneScript
{
    public class UILoadNextScene : MonoBehaviour
    {
        public void LoadNextScene(string sceneName)
        {
            LoaderManager.Get().LoadScene(sceneName);
            UILoadingScreen.Get().SetVisible(true);
        }
    }
}