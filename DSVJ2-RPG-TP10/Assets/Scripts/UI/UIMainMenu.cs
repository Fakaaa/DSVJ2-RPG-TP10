using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoaderManagerScript;
using UILoadingScreenScript;

namespace UIMainMenuScript
{
    public class UIMainMenu : MonoBehaviour
    {
        public void LoadGameScene()
        {
            LoaderManager.Get().LoadScene("Game"); 
            UILoadingScreen.Get().SetVisible(true);
        }

        public void LoadCreditsScene()
        {
            LoaderManager.Get().LoadScene("Credits"); 
            UILoadingScreen.Get().SetVisible(true);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}