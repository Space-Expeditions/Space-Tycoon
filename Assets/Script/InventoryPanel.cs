using System.Collections.Generic;
using System.Dynamic;
using UnityEditor.Search;
using UnityEngine;

public class InventoryPanel : ItemPanel
{
    public override void Onclick(int id)
    {
        GameManager.instance.dragAndDropController.Onclick(inventory.slots[id]);
        Show();
    }
}
