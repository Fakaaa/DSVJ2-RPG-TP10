using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagerScript;
using LoaderManagerScript;
using UILoadingScreenScript;

namespace UIResultScript
{
    public class UIResult : MonoBehaviour
    {
        public Image winner;
        public Image loser;

        private void Start()
        {
            if (GameManager.Get() != null)
            {
                if (GameManager.Get().winner)
                {
                    winner.gameObject.SetActive(true);
                    loser.gameObject.SetActive(false);
                }
                else
                {
                    winner.gameObject.SetActive(false);
                    loser.gameObject.SetActive(true);
                }
            }
        }

        public void LoadMenuScene()
        {
            if (LoaderManager.Get() != null)
                LoaderManager.Get().LoadScene("MainMenu");
            if (UILoadingScreen.Get() != null)
                UILoadingScreen.Get().SetVisible(true);
            gameObject.SetActive(false);
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