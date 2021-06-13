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