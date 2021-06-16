using UnityEngine;

namespace PauseScreenStateScript
{
    public class PauseScreenState : MonoBehaviour
    {
        public Canvas pauseScreen;
        [SerializeField] private GameObject inventoryUI;

        private float originalTimeScale = 0;

        private void Start()
        {
            originalTimeScale = Time.timeScale;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))   // Activar/Desactivar el canvas de pausa
                pauseScreen.gameObject.SetActive(!pauseScreen.gameObject.activeSelf);
            
            //perdon Osom, no sabemos cual es el problema
            if (pauseScreen.gameObject.activeSelf || inventoryUI.activeSelf)
                Time.timeScale = 0;
            else
                Time.timeScale = originalTimeScale;
        }
    }
}