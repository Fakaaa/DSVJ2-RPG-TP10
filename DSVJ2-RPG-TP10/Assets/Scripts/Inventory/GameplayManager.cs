using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class GameplayManager : MonoBehaviour
    {

        [SerializeField] ItemSO.ItemList allItems;

        PlayerInventory player;

        static private GameplayManager instance;

        static public GameplayManager GetInstance() { return instance; }

        string savePath = "SaveFile.json";

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            LoadJson();
        }
        public int GetRandomItemID() //get the id from a random item template (get a random id)
        {
            return Random.Range(0, allItems.List.Count);
        }

        public int GetRandomAmmountOfItem(int id)
        {
            return Random.Range(1, allItems.List[id].maxStack);
        }

        public ItemSO.Item GetItemFromID(int id)
        {
            return allItems.List[id];
        }

        public void SetPlayer(PlayerInventory p)
        {
            player = p;
        }

        public void SaveJson() //save LIST of slots
        {
            List<Slot> playerItems = player.GetSaveSlots();
            string json = "";
            for (int i = 0; i < playerItems.Count; i++)
            {
                json += JsonUtility.ToJson(playerItems[i]);
            }

            FileStream fs;

            if (!File.Exists(savePath)) //create new file if previous one doesn't exist
                fs = File.Create(savePath);
            else
                fs = File.Open(savePath, FileMode.Truncate); //overwrite old file

            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(json);
            fs.Close();
            bw.Close();
        }

        public void LoadJson()
        {
            if (!File.Exists(savePath)) { return; }

            string savedData;
            FileStream fs;
            fs = File.Open(savePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            savedData = br.ReadString();
            fs.Close();
            br.Close();
            List<Slot> newList = new List<Slot>();
            for (int i = 0; i < savedData.Length; i++)
            {
                if (savedData[i] == '{') //each slot starts with { & ends with }, so the func reads what is in between
                {
                    string slotString = "";
                    int aux = 0;
                    while (savedData[i + aux] != '}')
                    {
                        slotString += savedData[i + aux];
                        aux++;
                    }
                    slotString += '}';
                    Slot newSlot = JsonUtility.FromJson<Slot>(slotString);
                    newList.Add(newSlot); //add saved slots to new list
                }
            }
            player.SetSaveSlots(newList); //add new list to equipment & inventory
        }
        void OnDestroy()
        {
            SaveJson();
        }
    }
}