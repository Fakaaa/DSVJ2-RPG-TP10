using UnityEngine.SceneManagement;
using MonoBehaviourSingletonScript;

namespace LoaderManagerScript
{
    public class LoaderManager : MonoBehaviourSingleton<LoaderManager>
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}