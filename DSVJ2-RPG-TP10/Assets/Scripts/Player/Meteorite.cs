using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeteoriteScript
{
    public class Meteorite : MonoBehaviour
    {
        public static Action<int> OnCollisionEnemy;

        private float time = 0;
        public int damage = 20;
        public float maxTimeAlive = 5;

        private void Update()
        {
            time += Time.deltaTime;

            if (time > maxTimeAlive)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Enemy"))
                OnCollisionEnemy?.Invoke(damage);
        }
    }
}