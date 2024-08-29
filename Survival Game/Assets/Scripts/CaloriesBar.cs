using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaloriesBar : MonoBehaviour
{
    public Slider slider;
    public Text caloriesCounter;
    public GameObject PlayerState;
    public float currentCalories , maxCalories;



    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        currentCalories = PlayerState.GetComponent<PlayerState>().currentCalories;
        maxCalories = PlayerState.GetComponent<PlayerState>().maxCalories;

        float fillValue = currentCalories / maxCalories; // 0-1 
        slider.value = fillValue;
        caloriesCounter.text = currentCalories + "/" + maxCalories;

    }
}
