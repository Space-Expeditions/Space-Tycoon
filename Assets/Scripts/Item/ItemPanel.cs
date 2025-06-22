using UnityEngine;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

public class ItemPanel : MonoBehaviour
{
    public ItemContainer inventory;
    public List<InventoryButton> buttons;
    private void Start()
    {
        if (inventory == null)
        {
            inventory = InventoryManager.instance.inventoryContainer;
        }

        Init();
    }

    public void Init()
    {
        SetIndex();
        Show();
    }

    private void OnEnable()
    {
        if (inventory == null)
        {
            inventory = InventoryManager.instance.inventoryContainer;
        }
        Show();
    }

    private void SetIndex()
    {
        for (int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            buttons[i].SetIndex(i);
        }
    }
    public void Show()
    {
        for (int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            if (inventory.slots[i].item == null)
            {
                buttons[i].Clean();
            }
            else
            {
                buttons[i].Set(inventory.slots[i]);
            }
        }
    }

    public virtual void Onclick(int id)
    {

    }
}
