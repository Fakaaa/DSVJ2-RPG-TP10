using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioManagerScript;

namespace MeteorShowerScript
{
    public class MeteorShower : MonoBehaviour
    {
        [Header("Meteorites data")]
        public GameObject meteoritePrefab;
        public int amountOfMeteorites = 20;

        private int minRange = -100;
        private int maxRange = 101;
        private int posY = 50;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (AudioManager.Get() != null)
                    AudioManager.Get().Play("Meteorite");

                for (int i = 0; i < amountOfMeteorites; i++)
                {
                    GameObject go = Instantiate(meteoritePrefab, new Vector3(Random.Range(minRange, maxRange), posY, Random.Range(minRange, maxRange)), Quaternion.identity).gameObject;
                    go.transform.parent = gameObject.transform;
                }
            }
        }
    }
}