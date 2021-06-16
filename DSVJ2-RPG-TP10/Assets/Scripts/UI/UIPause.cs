using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoaderManagerScript;
using UILoadingScreenScript;

namespace UIPauseScript
{
    public class UIPause : MonoBehaviour
    {     
        public void LoadMenuScene()
        {
            gameObject.SetActive(false);
            if (LoaderManager.Get() != null)
                LoaderManager.Get().LoadScene("MainMenu");
            if (UILoadingScreen.Get() != null)
                UILoadingScreen.Get().SetVisible(true);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}