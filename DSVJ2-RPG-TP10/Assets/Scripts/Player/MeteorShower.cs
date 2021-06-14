using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeteorShowerScript
{
    public class MeteorShower : MonoBehaviour
    {
        [Header("Meteorites data")]
        public GameObject meteoritePrefab;
        public int amountOfMeteorites = 20;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                for (int i = 0; i < amountOfMeteorites; i++)
                {
                    GameObject go = Instantiate(meteoritePrefab, new Vector3(Random.Range(-100, 101), 50, Random.Range(-100, 101)), Quaternion.identity).gameObject;
                    go.transform.parent = gameObject.transform;
                }
            }
        }
    }
}