using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class UiItemSlot : MonoBehaviour
    {
        public enum PlayerList { Inventory, Outfit, Arms, None }

        [SerializeField] private UiInventory inv;
        [SerializeField] private PlayerList playerList = PlayerList.Inventory;
        [SerializeField] private int indexList; //index in respective list (inventory or Equipment.inv)
        [SerializeField] private int id; //id depends on item template / scriptable object
        [SerializeField] private int idDefaultSprite;

        public int GetID() => id;
        public int GetIndex() => indexList;
        public PlayerList GetPlayerList() => playerList;

        void Start()
        {
            inv.RefreshAllButtonsEvent += RefreshButton;
        }

        public void SetButton(int indexList, int id) //set sprite and active or deactive the text quantity
        {
            if (playerList == PlayerList.None)
            {
                id = -1;
                return;
            }
            this.indexList = indexList;
            this.id = id;

            if (id < 0) //if Button not initialized
            {
                if (playerList == PlayerList.Inventory) //if empty in inventory, default empty sprites
                    transform.GetChild(0).GetComponent<Image>().sprite = inv.defaultSprites[0];
                else //if NOT in inventory, use the sprite-empty of the specific slot
                {
                    transform.GetChild(0).GetComponent<Image>().sprite = inv.defaultSprites[idDefaultSprite];
                }
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            else //if initialized
            {
                Sprite sprite = GameplayManager.GetInstance().GetItemFromID(id).icon; //get sprite from item
                transform.GetChild(0).GetComponent<Image>().sprite = sprite;  //set sprite on button

                if (GameplayManager.GetInstance().GetItemFromID(id).maxStack > 1) //if can have more than one
                {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true); //set active quantity text
                    switch (playerList) //get qunatity from equipment OR inventory
                    {
                        case UiItemSlot.PlayerList.Arms:
                        case UiItemSlot.PlayerList.Outfit:
                            gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inv.equipment.GetSlot(indexList).amount.ToString();
                            break;
                        case UiItemSlot.PlayerList.Inventory:
                            gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = inv.inventory.GetSlot(indexList).amount.ToString();
                            break;
                    }
                }
                else //if only has 1
                {
                    gameObject.transform.GetChild(1).gameObject.SetActive(false); //set text quantity false
                }
            }

            PlayerInventory.OnRefreshMeshAsStatic?.Invoke();
        }

        private void Refresh(PlayerList playerlist)
        {
            switch (playerlist) //get id of original item depending if it's on equipment or inventory
            {
                case PlayerList.Arms:
                case PlayerList.Outfit:
                    id = inv.equipment.GetID(indexList);
                    break;
                case PlayerList.Inventory:
                    id = inv.inventory.GetID(indexList);
                    break;
            }
            SetButton(indexList, id); //set button again
        }

        private void Awake()
        {
            inv = FindObjectOfType<UiInventory>();
        }

        public void MouseDown(RectTransform btn)
        {
            if (id < 0)
                return;

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
            {
                if (playerList == PlayerList.Inventory)
                {
                    inv.inventory.Divide(indexList);
                    inv.RefreshAllButtons();
                }
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))
            {
                if (playerList == PlayerList.Inventory)
                {
                    inv.inventory.DeleteItem(indexList);
                    Refresh(playerList);
                }
            }
            else if (Input.GetMouseButton(0))
            {
                inv.MouseDown(btn);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                switch (playerList)
                {
                    case PlayerList.Inventory:
                        if (inv.inventory.UseItem(indexList))
                        {
                            inv.RefreshAllButtons();
                            inv.RefreshToolTip(btn);
                        }
                        else
                        {
                            Refresh(playerList);
                            if (id < 0)
                            {
                                inv.toolTip.gameObject.SetActive(false);
                            }
                        }
                        break;
                    case PlayerList.Outfit:
                    case PlayerList.Arms:
                        if (inv.equipment.RemoveEquipment(indexList))
                        {
                            inv.RefreshAllButtons();
                            inv.RefreshToolTip(btn);
                        }
                        break;
                    case PlayerList.None:
                    default:
                        break;
                }
            }
        }

        public void RefreshButton()
        {
            Refresh(playerList);
        }

        public void MouseDrag()
        {
            inv.MouseDrag();
        }

        public void MouseUp(RectTransform btn)
        {
            inv.MouseUp(btn);
        }

        public void MouseEnterOver(RectTransform btn)
        {
            if (inv.secondParameter)
            {
                Vector2 mousePos = Input.mousePosition;
                if (inv.mousePos == mousePos)
                {
                    inv.slotDrop = btn.GetComponent<UiItemSlot>();
                    inv.SwapButtonsIDs();
                }

                inv.secondParameter = false;
            }

            if (id < 0)
                return;

            if (playerList != PlayerList.None)
            {
                inv.toolTip.gameObject.SetActive(true);
                inv.MouseEnterOver(btn);
            }
        }

        public void MouseExitOver()
        {
            inv.toolTip.gameObject.SetActive(false);
        }
    }
}