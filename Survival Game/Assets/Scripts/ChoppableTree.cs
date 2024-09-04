using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange ;
    public bool canBeChopped ;

    public float treeMaxHealth;
    public float treeCurrentHealth;

    public Animator animator;
    public float caloriesRequiredToChopTree = 10;

    private void Start(){
        treeCurrentHealth = treeMaxHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (treeCurrentHealth > 0)
        {
            animator.SetTrigger("vibrate");
            treeCurrentHealth -= damage;  
            PlayerState.Instance.currentCalories -= caloriesRequiredToChopTree;
        }
        else
        {
            TreeFall();
        }
        

        
    }

    private void TreeFall()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject treeRuin = Instantiate(Resources.Load<GameObject>("ChoppedTreee"),
            new Vector3(treePosition.x, treePosition.y + 1 ,treePosition.z), Quaternion.Euler(0,0,0));
    }


    private void Update()
    {
        if (canBeChopped)
        {
            GlobalState.Instance.ResourceHealth = treeCurrentHealth;
            GlobalState.Instance.ResourceMaxHealth = treeMaxHealth;
        }
    }
}
