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
            LoaderManager.Get().LoadScene("MainMenu");  
            UILoadingScreen.Get().SetVisible(true);
            gameObject.SetActive(false);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}