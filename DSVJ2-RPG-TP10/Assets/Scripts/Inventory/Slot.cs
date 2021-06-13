﻿namespace Inventory
{
    [System.Serializable]
    public class Slot
    {
        public int ID; //id item (there is only one id per item / scriptable obj (1 for arrow, 1 for sword, etc.))
        public int amount;

        public Slot()
        {
            ID = -1;
            amount = 0;
        }
        public Slot(int ID, int amount)
        {
            this.ID = ID;
            this.amount = amount;
        }
        public int AddAmount(int amount)
        {
            this.amount += amount;
            int maxAmount = GameplayManager.GetInstance().GetItemFromID(ID).maxStack;
            if (this.amount > maxAmount)
            {
                int difference = this.amount - maxAmount;
                this.amount = maxAmount;
                return difference;
            }
            else if (this.amount <= 0)
            {
                EmptySlot();
            }
            return 0;
        }
        public void FillSlot(int ID, int amount)
        {
            this.ID = ID;
            this.amount = amount;
        }
        public void EmptySlot()
        {
            ID = -1;
            amount = 0;
        }
        public bool IsEmpty() { return ID < 0; }

        int SortSlotsByName(string str1, string str2)
        {
            return str1.CompareTo(str2);
        }
    }
}