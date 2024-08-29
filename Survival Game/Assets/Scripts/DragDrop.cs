using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    // Dictionary to map item names to their corresponding 3D prefab
    public Dictionary<string, GameObject> itemPrefabs;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Initialize the dictionary
        itemPrefabs = new Dictionary<string, GameObject>();

        // Load all prefabs from the Prefabs folder located in Resources
        LoadAllPrefabsFromAssets();
    }

    // Loads all prefabs from the Resources/Prefabs folder
    private void LoadAllPrefabsFromAssets()
{
    string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });

    foreach (string guid in guids)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        itemPrefabs[prefab.name] = prefab;  // Add to the dictionary using the prefab's name
    }

    Debug.Log("Loaded " + guids.Length + " prefabs.");
}

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false; // So the ray cast will ignore the item itself.
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta; // Make item move with the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;

        // Check if the item was not dropped on a valid slot
        if (transform.parent == startParent || transform.parent == transform.root)
        {
            if (!IsPointerOverInventory(eventData))
            {
                Drop3DObjectToGround();
                Destroy(gameObject); // Destroy the dragged item from the UI
            }
            else
            {
                transform.position = startPosition;
                transform.SetParent(startParent);
            }
        }

        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    // Check if the mouse is over the inventory area
    private bool IsPointerOverInventory(PointerEventData eventData)
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    // Instantiate the corresponding 3D prefab in the game world
    private void Drop3DObjectToGround()
    {
        string itemName = gameObject.name.Replace("(Clone)",""); // Use the name of the dragged item to find its prefab

        if (itemPrefabs.TryGetValue(itemName, out GameObject prefabToDrop))
        {
            Vector3 dropPosition = PlayerState.Instance.player.transform.position; // Use the player's position as the drop position
            // Adjust this based on your game's ground level
            dropPosition.x += Random.Range(0.5f, 2f); // Randomize the Y position to avoid overlapping objects
            dropPosition.z += Random.Range(0.5f, 3f); // Randomize the Z position to avoid overlapping objects


            Instantiate(prefabToDrop, dropPosition, Quaternion.identity); // Instantiate the 3D object in the world
            InventorySystem.Instance.ReCalculateList(); // Recalculate the inventory list
            CraftingSystem.Instance.RefreshNeededItems(); // Refresh the needed items for crafting

        }
        else
        {
            Debug.LogWarning("Prefab for item " + itemName + " not found.");
        }
    }
}
