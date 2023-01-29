using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.UI;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem itemPrefab;
        [SerializeField]
        private RectTransform contentPanel;
        [SerializeField]
        private UIInventoryDescription itemDescription;

        [SerializeField]
        private MouseFollower mouseFollower;
        List<UIInventoryItem> listofUIItems = new List<UIInventoryItem>();
        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
        OnItemActionRequested, OnStartDragging;
        public event Action<int, int> OnSwapItems;

        [SerializeField]
        private ItemActionPanel actionPanel;

        private void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();

        }
        public void InitializeInventoryUI(int inventorysize)
        {
            for (int i = 0; i < inventorysize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                listofUIItems.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        public void UpdateData(int ItemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listofUIItems.Count > ItemIndex)
            {
                listofUIItems[ItemIndex].SetData(itemImage, itemQuantity);
            }
        }
        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            int index = listofUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            ResetDraggedItem();
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = listofUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            int index = listofUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = listofUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);

        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButton(actionName, performAction);
        }
        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listofUIItems[itemIndex].transform.position;
        }
        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listofUIItems)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listofUIItems[itemIndex].Select();
        }

        internal void ResetAllItems()
        {
            foreach (var item in listofUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
        public void DestroyUI(int inventorysize)
        {
            for (int i = 0; i < inventorysize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                listofUIItems.Add(uiItem);
                uiItem.OnItemClicked -= HandleItemSelection;
                uiItem.OnItemBeginDrag -= HandleBeginDrag;
                uiItem.OnItemDroppedOn -= HandleSwap;
                uiItem.OnItemEndDrag -= HandleEndDrag;
                uiItem.OnRightMouseBtnClick -= HandleShowItemActions;
            }
        }
    }
}