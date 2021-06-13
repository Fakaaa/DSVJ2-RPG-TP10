using UnityEngine;

namespace ItemSO
{
    [CreateAssetMenu(fileName = "Consumible", menuName = "Items/Consumible")]
    public class Consumible : Item
    {
        public override ItemType GetItemType() { return ItemType.Consumible; }

        public override string ItemToString()
        {
            string text = base.ItemToString();
            text += "\nType: Consumible";
            return text;
        }
    }
}