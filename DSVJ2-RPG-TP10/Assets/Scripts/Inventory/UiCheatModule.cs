using UnityEngine;

namespace Inventory
{
    public class UiCheatModule : MonoBehaviour
    {
        public UiInventory uiInventory;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                AddNewItem();
            }
        }

        public void AddNewItem()
        {
            int randomID = GameplayManager.GetInstance().GetRandomItemID();
            int randomAmount = GameplayManager.GetInstance().GetRandomAmmountOfItem(randomID);
            if (uiInventory.inventory.AddNewItem(randomID, randomAmount))
                uiInventory.RefreshAllButtons();
        }
    }
}