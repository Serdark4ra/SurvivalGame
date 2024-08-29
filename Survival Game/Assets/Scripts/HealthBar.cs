using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text healthCounter;
    public GameObject PlayerState;
    public float currentHealth , maxHealth;



    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = PlayerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = PlayerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth; // 0-1 
        slider.value = fillValue;
        healthCounter.text = currentHealth + "/" + maxHealth;

    }
}
