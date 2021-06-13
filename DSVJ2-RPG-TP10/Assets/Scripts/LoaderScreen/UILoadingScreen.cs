using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MonoBehaviourSingletonScript;
using LoaderManagerScript;

namespace UILoadingScreenScript
{
    public class UILoadingScreen : MonoBehaviourSingleton<UILoadingScreen>
    {
        public Text loadingText;

        public override void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
        }

        public void SetVisible(bool show)
        {
            gameObject.SetActive(show);
        }

        public void Update()
        {
            int loadingVal = (int)(LoaderManager.Get().loadingProgress * 100);
            loadingText.text = "Loading: " + loadingVal;

            if (LoaderManager.Get().loadingProgress >= 1)
                SetVisible(false);
        }
    }
}