using UnityEngine;

public class ItemToolbarPanel : ItemPanel
{
    [SerializeField] ToolbarController toolbarController;

    int currentSelectedTool;

    private void Start()
    {
        Init();
        toolbarController.onChange += Highlight;
        Highlight(0);
    }

    public override void Onclick(int id)
    {
        toolbarController.Set(id);
        Highlight(id);
    }

    public void Highlight(int id)
    {
        if (buttons[currentSelectedTool] != null)
            buttons[currentSelectedTool].Highlight(false);

        currentSelectedTool = id;

        if (buttons[currentSelectedTool] != null)
            buttons[currentSelectedTool].Highlight(true);
    }

    // ���õ� ������ ������ ��ȯ
    public Item GetSelectedItem()
    {
        if (currentSelectedTool >= 0 && currentSelectedTool < inventory.slots.Count)
        {
            return inventory.slots[currentSelectedTool].item;
        }
        return null;
    }

    // ���õ� ���� �ε��� ��ȯ (���û���)
    public int GetSelectedIndex()
    {
        return currentSelectedTool;
    }

    // �� �����Ӹ��� UI ����
    private void Update()
    {
        Show();
    }

    //[SerializeField] ToolbarController toolbarController;

    //private void Start()
    //{
    //    Init();
    //    toolbarController.onChange += Highlight;
    //    Highlight(0);
    //}

    //public override void Onclick(int id)
    //{
    //    toolbarController.Set(id);
    //    Highlight(id);
    //}

    //int currentSelectedTool;

    //public void Highlight(int id)
    //{
    //    buttons[currentSelectedTool].Highlight(false);
    //    currentSelectedTool = id;
    //    buttons[currentSelectedTool].Highlight(true);
    //}
}
