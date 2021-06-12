using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonoBehaviourSingletonScript;
using EnemyAIFSMScript;

namespace GameManagerScript
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        [SerializeField] private List<GameObject> enemysAlive = new List<GameObject>();

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
                GameIsOver();
        }

        public void GameIsOver()
        {
            // Cambiar a la escena de resultado
            Debug.Log("Juego terminado");
        }
    }
}
