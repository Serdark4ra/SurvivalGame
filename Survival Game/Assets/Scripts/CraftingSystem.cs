using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{ 
    public GameObject CraftingScreenUI;
    public GameObject toolsScreenUI;

    public List<string> InventoryItemList = new List<string>();

    // cattegory buttons
    Button toolsButton;

    //craft buttons
    Button craftAxeButton;

    public Text AxeReq1, AxeReq2;

    public bool isOpen ;

    // all blueprints

    private BluePrint AxeBLP = new BluePrint("Axe", "stone", "stick", 3, 3, 2);







    public static CraftingSystem Instance {get ; set;}
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        
    }
  

  
  
  
  
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Crafting System Started");
        isOpen = false;
        toolsButton = CraftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsButton.onClick.AddListener(delegate {OpenToolsScreen();});

        //axe

        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeButton = toolsScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate {CraftAnyItem(AxeBLP);});

        Cursor.visible = false;

    }

    private void CraftAnyItem(BluePrint bluePrintToCraft)
    {
        InventorySystem.Instance.AddToInventory(bluePrintToCraft.ItemName);

        if (bluePrintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req1, bluePrintToCraft.Req1Amount);
        }
        else if (bluePrintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req1, bluePrintToCraft.Req1Amount);
            InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req2, bluePrintToCraft.Req2Amount);
        }

        StartCoroutine(calculate());

       

        
    }

    private void OpenToolsScreen()
    {
        CraftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.Log("Crafting System updated");
        //RefreshNeededItems();
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
 
			Debug.Log("i is pressed");
            CraftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;

 
        }
        else if ((Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Escape) ) && isOpen)
        {
            CraftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
            
        }

    }

    public IEnumerator calculate()
    {
        yield return 0;
        InventorySystem.Instance.ReCalculateList();
         RefreshNeededItems();
    }

    public void RefreshNeededItems()
    {       
        int stone_count = 0;
        int Stick_count = 0;

        InventoryItemList = InventorySystem.Instance.itemList;

        foreach (string item in InventoryItemList)
        {
            switch (item)
            {
                case "stone":
                    stone_count++;
                    break;
                case "stick":
                    Stick_count++;
                    break;
                default:
                    break;
            }
            
        }


        Debug.Log(AxeReq1 != null ? "AxeReq1 is assigned" : "AxeReq1 is null");
        Debug.Log(AxeReq2 != null ? "AxeReq2 is assigned" : "AxeReq2 is null");

        AxeReq1.text = "3 Stone: [" + stone_count + "]";
        AxeReq2.text = "3 Stick: [" + Stick_count + "]";

        if (stone_count >= 3 && Stick_count >= 3)
        {
            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }

    }
}
