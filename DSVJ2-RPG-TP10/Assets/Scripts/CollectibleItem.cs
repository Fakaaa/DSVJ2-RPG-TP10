using UnityEngine;

namespace CollectibleItemScript
{
    public class CollectibleItem : MonoBehaviour
    {
        public ParticleSystem particles;
        public int itemID;

        private void Start()
        {
            ItemSO.Item item = Inventory.GameplayManager.GetInstance().GetItemFromID(itemID);
            ChangeColorParticle(item.GetItemType());
            GameObject go = Instantiate(particles, transform.position, particles.gameObject.transform.rotation).gameObject;
            go.transform.parent = gameObject.transform;
            gameObject.GetComponent<MeshFilter>().sharedMesh = item.mesh; 
            gameObject.GetComponent<MeshCollider>().sharedMesh = item.mesh;

            if (item.GetItemType() != ItemSO.ItemType.Outfit)
                gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }

        void ChangeColorParticle(ItemSO.ItemType itemType)
        {
            ParticleSystem.MainModule coloAux = particles.main;
            switch (itemType)
            {
                case ItemSO.ItemType.Arms:
                    coloAux.startColor = Color.magenta;
                    break;
                case ItemSO.ItemType.Outfit:
                    coloAux.startColor = Color.green;
                    break;
            }
        }
    }
}