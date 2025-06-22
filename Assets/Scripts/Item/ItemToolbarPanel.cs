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

    // 선택된 슬롯의 아이템 반환
    public Item GetSelectedItem()
    {
        if (currentSelectedTool >= 0 && currentSelectedTool < inventory.slots.Count)
        {
            return inventory.slots[currentSelectedTool].item;
        }
        return null;
    }

    // 선택된 슬롯 인덱스 반환 (선택사항)
    public int GetSelectedIndex()
    {
        return currentSelectedTool;
    }

    // 매 프레임마다 UI 갱신
    private void Update()
    {
        Show();
    }
}
