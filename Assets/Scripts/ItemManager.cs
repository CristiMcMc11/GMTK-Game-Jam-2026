using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] private Item[] allItemTypes;
    [SerializeField] private List<Item> currentItems = new List<Item>();
    [SerializeField] private List<Item> currentActiveItems = new List<Item>();

    //Input Events
    public static event Action OnJumpPressed;
    public static event Action OnABK1Pressed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddItem(Item item)
    {
        if (!item.passive) currentActiveItems.Add(item);
        currentItems.Add(item);
        item.isEnabled = true;
    }

    public void RemoveItem(Item item)
    {
        if (!item.passive) currentActiveItems.Remove(item);
        currentItems.Remove(item);
        item.isEnabled = false;
    }

    #region Input Functions
    
    public void OnABK1Input(InputAction.CallbackContext context)
    {
        bool val = context.ReadValueAsButton();
        print(val);
        if (val)
        {
            OnABK1Pressed?.Invoke();
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnJumpPressed?.Invoke();
        }
    }

    #endregion
}
