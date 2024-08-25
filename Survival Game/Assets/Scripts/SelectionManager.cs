using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class SelectionManager : MonoBehaviour
{

    public static SelectionManager Instance{ get; set;}
 
    public GameObject interaction_Info_UI;
    Text interaction_text;

    public bool onTarget;

    public GameObject selectedObject;

    
 
    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        }else{
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
 
            if (ourInteractableObj && ourInteractableObj.playerInRange )
            {
                onTarget = true;
                selectedObject = ourInteractableObj.gameObject;
                interaction_text.text = ourInteractableObj.GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else 
            { 
                onTarget = false;
                interaction_Info_UI.SetActive(false);
            }
 
        }else{
            onTarget = false;
            interaction_Info_UI.SetActive(false);
        }
    }
}