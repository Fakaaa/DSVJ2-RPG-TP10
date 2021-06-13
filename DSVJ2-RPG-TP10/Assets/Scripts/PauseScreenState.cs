using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PauseScreenStateScript
{
    public class PauseScreenState : MonoBehaviour
    {
        public Canvas pauseScreen; 

        private float originalTimeScale = 0;

        private void Start()
        {
            originalTimeScale = Time.timeScale;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))   // Activar/Desactivar el canvas de pausa
                pauseScreen.gameObject.SetActive(!pauseScreen.gameObject.activeSelf);

            if (pauseScreen.gameObject.activeSelf)
                Time.timeScale = 0;
            else
                Time.timeScale = originalTimeScale;
        }
    }
}