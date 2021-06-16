using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonoBehaviourSingletonScript;
using UIResultScript;

namespace GameManagerScript
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        [SerializeField] private List<GameObject> enemysAlive = new List<GameObject>();
        public bool winner = false;

        [Header("Result screen data")]
        public UIResult prefabResult;
        public UIResult resultScreen;

        public void InitializeResultScreen()
        {
            if (prefabResult != null)
            {
                resultScreen = Instantiate(prefabResult);
                resultScreen.gameObject.SetActive(false);
                winner = false;
            }
        }

        public void AddEnemyToList(GameObject enemy)
        {
            enemysAlive.Add(enemy);
        }

        public void RemoveEnemyToList(GameObject enemy)
        {
            enemysAlive.Remove(enemy);
            CheckConditionToWin();
        }

        public void CheckConditionToWin()
        {
            if (enemysAlive.Count == 0)
            {
                winner = true;
                GameIsOver();
            }
        }

        public void GameIsOver()
        {
            enemysAlive.Clear();
            resultScreen.gameObject.SetActive(true);
        }
    }
}
