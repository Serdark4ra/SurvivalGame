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
            StartCoroutine(Hit(damage));  
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

        GameObject treeRuin = Instantiate(Resources.Load<GameObject>("ChoppedTreee"), treePosition, Quaternion.Euler(0,0,0));
    }

    public IEnumerator Hit(float damage)
    {
        yield return new WaitForSeconds(0.6f);
        animator.SetTrigger("vibrate");
        treeCurrentHealth -= damage;  
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
