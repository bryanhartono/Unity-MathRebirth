using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Inventory.Model;
using Inventory.UI;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryPage inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        [SerializeField]
        private AudioClip [] Clips;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private UIInventoryPage shopUI;

        [SerializeField]
        private InventorySO shopData;
        private bool inShopArea = false;
        private void Awake()
        {
            PrepareUI();
            PrepareShopUI();
            PrepareInventoryData();
            PrepareShopData();
        }

        private void PrepareInventoryData()
        {
            GlobalControl glob = GameObject.Find("GlobalObject").GetComponent<GlobalControl>();
            PlayerData data = SaveSystem.LoadGame();

            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;


            if (data != null && data.inventory.Length != 0)
            {
                foreach (KeyValuePair<int, InventoryItem> kvp in glob.InventoryDictGet())
                {
                    ItemSO item = kvp.Value.item;
                    int count = kvp.Value.quantity;

                    InventoryItem inventoryItem = new InventoryItem
                    {
                        item = item,
                        quantity = count
                    };

                    inventoryData.AddItem(inventoryItem);
                }

            }

        }
        private void PrepareShopData()
        {
            shopData.Initialize();
            shopData.OnInventoryUpdated += UpdateShopUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                shopData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void UpdateShopUI(Dictionary<int, InventoryItem> inventoryState)
        {
            shopUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                shopUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void PrepareShopUI()
        {
            shopUI.InitializeInventoryUI(shopData.Size);
            shopUI.OnDescriptionRequested += HandleShopDescriptionRequest;
            shopUI.OnItemActionRequested += HandleShopItemActionRequest;
        }
        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }

        }

        private void HandleShopItemActionRequest(int itemIndex)
        {
            shopUI.ShowItemAction(itemIndex);
            InventoryItem inventoryItem = shopData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                InventoryItem tempInventoryItem = new InventoryItem
                {
                    item = inventoryItem.item,
                    quantity = inventoryItem.quantity,
                };
                GlobalControl glob = GameObject.Find("GlobalObject").GetComponent<GlobalControl>();
                if(glob.playerCurrency >= inventoryItem.quantity){
                    shopUI.AddAction("Buy", () => BuyItem(tempInventoryItem));
                }
                else{
                    shopUI.AddAction("No IQ", () => CantBuyItem(tempInventoryItem));
                }
                
            }

        }
        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(Clips[0]);
        }
        private void CantBuyItem(InventoryItem inventoryItem)
        {
            shopUI.ResetSelection();
        }
        private void BuyItem(InventoryItem inventoryItem)
        {
            GlobalControl glob = GameObject.Find("GlobalObject").GetComponent<GlobalControl>();
            if (glob.playerCurrency >= inventoryItem.quantity)
            {
                glob.playerCurrency -= inventoryItem.quantity;
                InventoryItem tempInventoryItem = new InventoryItem
                {
                    item = inventoryItem.item,
                    quantity = 1,
                };
                inventoryData.AddItem(tempInventoryItem);
                audioSource.PlayOneShot(Clips[1]);
            }
            
            shopUI.ResetSelection();
            
        }
        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
                audioSource.PlayOneShot(Clips[2]);
                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryUI.ResetSelection();
            }
        }
        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;

            }
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, description);
        }

        private void HandleShopDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = shopData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;

            }
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            shopUI.UpdateDescription(itemIndex, item.ItemImage, item.name, description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
                    $": {inventoryItem.itemState[i].value} / " +
                    $"{inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void Update()
        {
            GameObject glob = GameObject.Find("GlobalObject");
            if (glob.GetComponent<GlobalControl>().inCombat || glob.GetComponent<GlobalControl>().inMap)
            {
                inventoryUI.Hide();
                glob.GetComponent<GlobalControl>().inInventory = false;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!glob.GetComponent<GlobalControl>().inCombat && !shopUI.isActiveAndEnabled && !glob.GetComponent<GlobalControl>().inCharPage && !glob.GetComponent<GlobalControl>().inSkillPage && !glob.GetComponent<GlobalControl>().inMap && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inOptions)
                {
                    glob.GetComponent<GlobalControl>().inInventory = true;
                    if (inventoryUI.isActiveAndEnabled == false)
                    {
                        inventoryUI.Show();
                        inventoryUI.Show();
                        foreach (var item in inventoryData.GetCurrentInventoryState())
                        {
                            inventoryUI.UpdateData(item.Key,
                            item.Value.item.ItemImage,
                            item.Value.quantity);
                        }

                    }
                    else
                    {
                        inventoryUI.Hide();
                        glob.GetComponent<GlobalControl>().inInventory = false;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && !glob.GetComponent<GlobalControl>().inCharPage && !glob.GetComponent<GlobalControl>().inSkillPage && !glob.GetComponent<GlobalControl>().inInventory && !GameObject.Find("GlobalObject").GetComponent<GlobalControl>().inOptions)
            {
                if (!glob.GetComponent<GlobalControl>().inCombat && inShopArea && !inventoryUI.isActiveAndEnabled)
                {
                    glob.GetComponent<GlobalControl>().inShop = true;
                    if (shopUI.isActiveAndEnabled == false)
                    {
                        shopUI.Show();
                        shopUI.Show();
                        foreach (var item in shopData.GetCurrentInventoryState())
                        {
                            shopUI.UpdateData(item.Key,
                            item.Value.item.ItemImage,
                            item.Value.quantity);
                        }

                    }
                    else
                    {
                        shopUI.Hide();
                        glob.GetComponent<GlobalControl>().inShop = false;
                    }
                }

            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Shop")
                inShopArea = true;
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Shop")
                inShopArea = false;
        }

        private void OnDisable()
        {
            inventoryUI.DestroyUI(inventoryData.Size);
            shopUI.DestroyUI(shopData.Size);

            inventoryData.OnInventoryUpdated -= UpdateInventoryUI;
            shopData.OnInventoryUpdated -= UpdateShopUI;

            inventoryUI.OnDescriptionRequested -= HandleDescriptionRequest;
            inventoryUI.OnSwapItems -= HandleSwapItems;
            inventoryUI.OnStartDragging -= HandleDragging;
            inventoryUI.OnItemActionRequested -= HandleItemActionRequest;

            shopUI.OnDescriptionRequested -= HandleShopDescriptionRequest;
            shopUI.OnItemActionRequested -= HandleShopItemActionRequest;


        }
    }
}