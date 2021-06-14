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
            gameObject.SetActive(false);
        }

        public void LoadCreditsScene()
        {
            LoaderManager.Get().LoadScene("Credits"); 
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