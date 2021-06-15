using UnityEngine;

namespace CollectibleItemScript
{
    public class CollectibleItem : MonoBehaviour
    {
        public ParticleSystem particles;
        public int itemID;

        private void Start()
        {
            GameObject go = Instantiate(particles, transform.position, particles.gameObject.transform.rotation).gameObject;
            go.transform.parent = gameObject.transform;
            ItemSO.Item item = Inventory.GameplayManager.GetInstance().GetItemFromID(itemID);
            gameObject.GetComponent<MeshFilter>().sharedMesh = item.mesh; 
            gameObject.GetComponent<MeshCollider>().sharedMesh = item.mesh; 
        }
    }
}