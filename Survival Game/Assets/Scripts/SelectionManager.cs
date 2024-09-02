using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager Instance { get; set; }

    public GameObject interaction_Info_UI;
    Text interaction_text;

    public bool onTarget;

    public GameObject selectedObject;

    public Image centerDotIcon;
    public Image handIcon;

    public bool isHandIconActive;
    public GameObject selectedTree;
    public GameObject chopHolder;


    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
        centerDotIcon.gameObject.SetActive(true);
        handIcon.gameObject.SetActive(false);
        isHandIconActive = false;

        if(chopHolder != null)
        {
            Debug.Log("Chop holder is not null");
        }
        else
        {
            Debug.Log("Chop holder is null");
        }
    }

    void Awake()
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

    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject ourInteractableObj = selectionTransform.GetComponent<InteractableObject>();

            ChoppableTree ourChoppableTree = selectionTransform.GetComponent<ChoppableTree>();

            if (ourChoppableTree && ourChoppableTree.playerInRange)
            {
                onTarget = true;
                selectedTree = ourChoppableTree.gameObject;
                ourChoppableTree.canBeChopped = true;
                interaction_text.text = "Press E to chop";
                interaction_Info_UI.SetActive(true);
                handIcon.gameObject.SetActive(false);
                centerDotIcon.gameObject.SetActive(true);
                isHandIconActive = false;
                chopHolder.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
                {
                    
                }
                
            }

            if (ourInteractableObj && ourInteractableObj.playerInRange)
            {
                onTarget = true;
                selectedObject = ourInteractableObj.gameObject;
                interaction_text.text = ourInteractableObj.GetItemName();
                interaction_Info_UI.SetActive(true);
                if (ourInteractableObj.CompareTag("Pickable"))
                {
                    handIcon.gameObject.SetActive(true);
                    centerDotIcon.gameObject.SetActive(false);
                    isHandIconActive = true;
                }
                else
                {
                    handIcon.gameObject.SetActive(false);
                    centerDotIcon.gameObject.SetActive(true);
                    isHandIconActive = false;
                }
            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                handIcon.gameObject.SetActive(false);
                centerDotIcon.gameObject.SetActive(true);
                isHandIconActive = false;

            }

        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            handIcon.gameObject.SetActive(false);
            centerDotIcon.gameObject.SetActive(true);
            isHandIconActive = false;
            

            

        }
    }

    internal void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotIcon.enabled = false;
        interaction_Info_UI.SetActive(false);

        selectedObject = null;
    }

    internal void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotIcon.enabled = true;
        interaction_Info_UI.SetActive(true);

    }
}