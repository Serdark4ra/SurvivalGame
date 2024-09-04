using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject ItemInfoUI;

    //pup op    
    public GameObject pickUpAlert;
    public Text pickUpName;
    public Image pickUpImage;



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
        Cursor.visible = false;

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
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        }
        else if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }

    public void AddToInventory(String ItemName)
    {

        whatSlotToEquipt = FindNextEmptySLot();

        Debug.Log(ItemName);
        itemToAdd = Instantiate(Resources.Load<GameObject>(ItemName), whatSlotToEquipt.transform.position, whatSlotToEquipt.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquipt.transform);

        itemList.Add(ItemName);

        TriggerPopUp(ItemName, itemToAdd.GetComponent<Image>().sprite);
        

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();

        

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

    IEnumerator PopUpWait()
    {
        yield return new WaitForSeconds(3);
        pickUpAlert.SetActive(false);
    }

    void TriggerPopUp(String ItemName, Sprite ItemSprite)
    {
        pickUpAlert.SetActive(true);
        pickUpName.text = ItemName;
        pickUpImage.sprite = ItemSprite;
        StartCoroutine(PopUpWait());
    }
    

    public bool CheckSlotsAvailable(int emptySlotsNeeded)
    {
        int counter = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }


        }

        if (counter <= 21 - emptySlotsNeeded)
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
        int itemsRemoved = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                Transform item = slot.transform.GetChild(0);
                if (item.name.StartsWith(nameToRemove))
                {
                    Debug.Log("Removing " + nameToRemove);
                    Destroy(item.gameObject);
                    Debug.Log("Removed " + nameToRemove);
                    itemsRemoved++;
                    if (itemsRemoved >= amountToRemove)
                    {
                        break;
                    }
                }
            }
        }

        if (itemsRemoved < amountToRemove)
        {
            Debug.LogWarning("Could not remove the requested amount of " + nameToRemove + ". Only " + itemsRemoved + " items were removed.");
        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
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


