using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }
 
    // -- UI -- //
    public GameObject quickSlotsPanel;
 
    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemList = new List<string>();
 
   public GameObject NumbersHolder;

   public GameObject ToolHolder;

   public int selectedNumber = -1;
   public GameObject selectedItem;

    GameObject SelectedItemModel;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
 
    private void Start()
    {
        PopulateSlotList();
    }
 
    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           SelectQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);
        }
       
    }

    private void SelectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number))
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;
        
                // Deselecting the previous item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }
                selectedItem = getSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquipedItem(selectedItem);
        
                // Changing color of the numbers
                if (NumbersHolder != null)
                {
                    foreach (Transform child in NumbersHolder.transform)
                    {
                        child.transform.Find("Text").GetComponent<Text>().color = Color.white;
                    }
        
                    Transform numberTransform = NumbersHolder.transform.Find("number" + number);
                    if (numberTransform != null)
                    {
                        Text toBeChanged = numberTransform.GetChild(0).GetComponent<Text>();
                        if (toBeChanged != null)
                        {
                            toBeChanged.color = Color.red;
                            Debug.Log("set to red");
                        }
                        else
                        {
                            Debug.LogError("Text component not found for number " + number);
                        }
                    }
                    else
                    {
                        Debug.LogError("Number transform not found for number " + number);
                    }
                }
                else
                {
                    Debug.LogError("NumbersHolder is null");
                }
            }else
            {
                // try to select the same item
                selectedNumber = -1; // null

                // Deselecting the previous item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    DestroyImmediate(SelectedItemModel.gameObject);
                    SelectedItemModel = null;
                }

                foreach (Transform child in NumbersHolder.transform)
                    {
                        Text textComponent = child.transform.Find("Text")?.GetComponent<Text>();
                        if (textComponent != null)
                        {
                            textComponent.color = Color.white;
                            Debug.Log("set to grey");
                        }
                    }


            }
        }
    }

    private void SetEquipedItem(GameObject selectedItem)
    {
        if (SelectedItemModel != null)
        {
            DestroyImmediate(SelectedItemModel.gameObject);
            SelectedItemModel = null;
        }
        string selectedItemName = selectedItem.name.Replace("(Clone)","");
        
        SelectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
        new Vector3(0,2.975f,1.52f), Quaternion.Euler(-8f,-101.4f,118.8f));

        SelectedItemModel.transform.SetParent(ToolHolder.transform,false);
    }

    private GameObject getSelectedItem(int number)
    {
        return quickSlotsList[number - 1].transform.GetChild(0).gameObject;
    }
    
    private bool checkIfSlotIsFull(int number)
    {
        if (quickSlotsList[number - 1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // Adding item to list
        itemList.Add(cleanName);
 
        InventorySystem.Instance.ReCalculateList();
 
    }
 
 
    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    
 
    public bool CheckIfFull()
    {
 
        int counter = 0;
 
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }
 
        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}