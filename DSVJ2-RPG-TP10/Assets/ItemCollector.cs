using UnityEngine;
using ItemSO;

namespace ItemCollectorScript
{
    public class ItemCollector : MonoBehaviour
    {
        private int itemPickedUpID = -1;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Item")) { return; }

            itemPickedUpID = other.gameObject.GetComponent<CollectibleItemScript.CollectibleItem>().itemID;
            Destroy(other.gameObject);
        }
        public bool ItemPicked()
        {
            return itemPickedUpID > 0;
        }
        public int ReturnItemToPlayer()
        {
            int itemToReturn = itemPickedUpID;
            itemPickedUpID = -1;
            return itemToReturn;
        }
    }
}