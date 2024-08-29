using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class drationBar : MonoBehaviour
{
    public Slider slider;
    public Text HydrationCounter;
    public GameObject PlayerState;
    public float currentHydraitonPercent , maxHydrationPercent;



    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        currentHydraitonPercent = PlayerState.GetComponent<PlayerState>().currentHydrationPercent;
        maxHydrationPercent = PlayerState.GetComponent<PlayerState>().maxHydrationPercent;

        float fillValue = currentHydraitonPercent / maxHydrationPercent; // 0-1 
        slider.value = fillValue;
        HydrationCounter.text = currentHydraitonPercent + "/" + maxHydrationPercent;

    }
}
