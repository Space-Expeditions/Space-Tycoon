public class InventoryPanel : ItemPanel
{
    public override void Onclick(int id)
    {
        InventoryManager.instance.dragAndDropController.Onclick(inventory.slots[id]);
        Show();
    }
}