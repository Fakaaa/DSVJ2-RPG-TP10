using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeteoriteScript
{
    public class Meteorite : MonoBehaviour
    {
        private float time = 0;
        public float maxTimeAlive = 5;

        private void Update()
        {
            time += Time.deltaTime;

            if (time > maxTimeAlive)
                Destroy(gameObject);
        }
    }
}