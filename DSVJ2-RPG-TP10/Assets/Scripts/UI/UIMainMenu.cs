using UnityEngine;
using LoaderManagerScript;
using UILoadingScreenScript;
using AudioManagerScript;

namespace UIMainMenuScript
{
    public class UIMainMenu : MonoBehaviour
    {
        private void Start()
        {
            if (AudioManager.Get() != null)
                AudioManager.Get().Play("MainMenuSong");
        }
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