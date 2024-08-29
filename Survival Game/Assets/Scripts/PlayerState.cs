using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }
    public float currentHealth, maxHealth;

    public float currentCalories, maxCalories;

    public float currentHydrationPercent, maxHydrationPercent;

    float distanceTravelled;
    Vector3 lastPosition;

    public GameObject player;


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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        StartCoroutine(DecreaseHydration());

    }

    private IEnumerator DecreaseHydration()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            currentHydrationPercent -= 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += Vector3.Distance(player.transform.position, lastPosition);
        lastPosition = player.transform.position;

        if (distanceTravelled >= 10)
        {
            currentCalories -= 10;
            distanceTravelled = 0;
        }


        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHealth -= 10;
        }
        
    }

    public void setHealth(float health)
    {
        currentHealth = health;
    }

    public void setCalories(float calories)
    {
        currentCalories = calories;
    }

    public void setHydration(float hydration)
    {
        currentHydrationPercent = hydration;
    }
}
