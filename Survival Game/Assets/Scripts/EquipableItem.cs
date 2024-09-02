using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !SelectionManager.Instance.isHandIconActive)
        {
            GameObject selectedTree = SelectionManager.Instance.selectedTree;
        
            if (selectedTree != null)
            {
                ChoppableTree choppableTree = selectedTree.GetComponent<ChoppableTree>();
                if (choppableTree != null)
                {
                    choppableTree.TakeDamage(10);
                }
            }
            animator.SetTrigger("hit");
        }
    }
}
