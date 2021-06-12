using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollectibleItemScript
{
    public class CollectibleItem : MonoBehaviour
    {
        public ParticleSystem particles;

        private void Start()
        {
            GameObject go = Instantiate(particles, transform.position, Quaternion.identity).gameObject;
            go.transform.parent = gameObject.transform;
        }
    }
}