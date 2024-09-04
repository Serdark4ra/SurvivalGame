using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{ 
    public GameObject CraftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI, processScreenUI;

    public List<string> InventoryItemList = new List<string>();

    // cattegory buttons
    Button toolsButton, survivalButton, processButton;

    //craft buttons
    Button craftAxeButton, craftPlankButton;

    public Text AxeReq1, AxeReq2, plankReq1;

    public bool isOpen ;

    // all blueprints

    private BluePrint AxeBLP = new BluePrint("Axe",1, "stone", "stick", 3, 3, 2);
    private BluePrint PlankBLP = new BluePrint("Plank",2, "log", 1,1);







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
        //tools button
        toolsButton = CraftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsButton.onClick.AddListener(delegate {OpenToolsScreen();});

        //survival button
        survivalButton = CraftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalButton.onClick.AddListener(delegate {OpenSurvivalScreen();});

        //process button
        processButton = CraftingScreenUI.transform.Find("ProcessButton").GetComponent<Button>();
        processButton.onClick.AddListener(delegate {OpenProcessScreen();});
        
        
        //axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeButton = toolsScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate {CraftAnyItem(AxeBLP);});

        //plank
        plankReq1 = processScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();
        
        craftPlankButton = processScreenUI.transform.Find("Plank").transform.Find("CraftButton").GetComponent<Button>();
        craftPlankButton.onClick.AddListener(delegate {CraftAnyItem(PlankBLP);});

        Cursor.visible = false;

    }

    private void OpenProcessScreen()
    {
        CraftingScreenUI.SetActive(false);
        processScreenUI.SetActive(true);
    }

    private void OpenSurvivalScreen()
    {
        CraftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
    }

    private void CraftAnyItem(BluePrint bluePrintToCraft)
    {
        AudioListenerManager.Instance.PlaySound(AudioListenerManager.Instance.craftItemSound);
        for (int i = 0; i < bluePrintToCraft.numberOfNewCreatedItem; i++)
        {
            InventorySystem.Instance.AddToInventory(bluePrintToCraft.ItemName);
        }

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
            survivalScreenUI.SetActive(false);
            processScreenUI.SetActive(false);
            InventorySystem.Instance.ItemInfoUI.SetActive(false);


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
        int Log_count = 0;

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
                case "log":
                    Log_count++;
                    break;
                default:
                    break;
            }
            
        }


        Debug.Log(AxeReq1 != null ? "AxeReq1 is assigned" : "AxeReq1 is null");
        Debug.Log(AxeReq2 != null ? "AxeReq2 is assigned" : "AxeReq2 is null");

        //-----Axe-----
        AxeReq1.text = "3 Stone: [" + stone_count + "]";
        AxeReq2.text = "3 Stick: [" + Stick_count + "]";

        if (stone_count >= 3 && Stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }

        //-----Plank-----
        plankReq1.text = "1 Log: [" + Log_count + "]";

        if (Log_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftPlankButton.gameObject.SetActive(true);
        }
        else
        {
            craftPlankButton.gameObject.SetActive(false);
        }

    }
}
