using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHealthBar : MonoBehaviour
{
    private Slider slider;
    private float treeMaxHealth, treeCurrentHealth;

    public GameObject globalState;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        
    }

    void Update()
    {
        treeCurrentHealth = globalState.GetComponent<GlobalState>().ResourceHealth;
        treeMaxHealth = globalState.GetComponent<GlobalState>().ResourceMaxHealth;

        float fillValue = treeCurrentHealth / treeMaxHealth; // 0-1 
        slider.value = fillValue;
        Debug.Log("Current Health: " + treeCurrentHealth);


    }   

}
