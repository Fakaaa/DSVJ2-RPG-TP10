﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MonoBehaviourSingletonScript;
using AudioManagerScript;

namespace LoaderManagerScript
{
    public class LoaderManager : MonoBehaviourSingleton<LoaderManager>
    {
        public float loadingProgress;
        public bool fakeLoad;
        public float timeLoading;
        public float minTimeToLoad = 2;

        public void LoadScene(string sceneName)
        {
            if (fakeLoad)
                StartCoroutine(AsynchronousLoadWithFake(sceneName));
            else
                StartCoroutine(AsynchronousLoad(sceneName));
        }

        IEnumerator AsynchronousLoad(string scene)
        {
            loadingProgress = 0;

            yield return null;

            AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
            ao.allowSceneActivation = false;

            while (!ao.isDone)
            {
                loadingProgress = ao.progress + 0.1f;

                // Se completo la carga
                if (ao.progress >= 0.9f)
                    ao.allowSceneActivation = true;

                yield return null;
            }
        }

        IEnumerator AsynchronousLoadWithFake(string scene)
        {
            loadingProgress = 0;
            timeLoading = 0;

            yield return null;

            AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
            ao.allowSceneActivation = false;

            while (!ao.isDone)
            {
                timeLoading += Time.deltaTime;
                loadingProgress = ao.progress + 0.1f;
                loadingProgress = loadingProgress * timeLoading / minTimeToLoad;

                // Se completo la carga
                if (loadingProgress >= 1)
                    ao.allowSceneActivation = true;

                yield return null;
            }
            if (AudioManager.Get() != null)
            {
                switch (scene)
                {
                    case "MainMenu":
                        AudioManager.Get().Play("MainMenuSong");
                        AudioManager.Get().Stop("CreditsSong");
                        AudioManager.Get().Stop("GameSong");
                        break;
                    case "Credits":
                        AudioManager.Get().Play("CreditsSong");
                        AudioManager.Get().Stop("MainMenuSong");
                        break;
                    case "Game":
                        AudioManager.Get().Play("GameSong");
                        AudioManager.Get().Stop("MainMenuSong");
                        break;
                }
            }
        }
    }
}