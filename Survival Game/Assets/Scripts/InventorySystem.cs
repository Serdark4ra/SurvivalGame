using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<String> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquipt;

    public bool isOpen;

    public bool isFull;


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


    void Start()
    {
        isOpen = false;
        isFull = false;
        PopulateSlotList();

    }
    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
        }
    }

    public void AddToInventory(String ItemName)
    {

        whatSlotToEquipt = FindNextEmptySLot();

        Debug.Log(ItemName);
        itemToAdd = Instantiate(Resources.Load<GameObject>(ItemName), whatSlotToEquipt.transform.position, whatSlotToEquipt.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquipt.transform);

        itemList.Add(ItemName);

    }

    private GameObject FindNextEmptySLot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return new GameObject();
    }

    public bool checkIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }


        }
        if (counter == 21)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal void RemoveItem(string nameToRemove, int amountToRemove)
    {
        Debug.Log("Removing " + amountToRemove + " " + nameToRemove);
        for (int i = 0; i < amountToRemove; i++)
        {
            foreach (GameObject slot in slotList)
            {
                if (slot.transform.childCount > 0)
                {
                    if (slot.transform.GetChild(0).name == nameToRemove + "(Clone)")
                    {
                        Debug.Log("Removing " + nameToRemove);
                        Destroy(slot.transform.GetChild(0).gameObject);
                       
                    }
                }
            }
        }
    }

    public void ReCalculateList()
    {   
        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                itemList.Add(slot.transform.GetChild(0).name.Replace("(Clone)", ""));
            }
        }
    }
}


