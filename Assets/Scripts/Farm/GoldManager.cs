using UnityEngine;
using System;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int Gold { get; private set; }

    public event Action<int> OnGoldChanged;

    const string GoldKey = "PlayerGold";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGold(); // ���� �� ��� �ҷ�����
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        SaveGold(); // ����� ��� ����
        OnGoldChanged?.Invoke(Gold);
    }

    public bool TrySpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            SaveGold(); // ����� ��� ����
            OnGoldChanged?.Invoke(Gold);
            return true;
        }
        return false;
    }

    /// <summary>
    /// �ܼ��� ��尡 ��������� Ȯ�� (���� ����)
    /// </summary>
    public bool HasGold(int amount)
    {
        return Gold >= amount;
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt(GoldKey, Gold);
        PlayerPrefs.Save(); // ���� ���� ����
    }

    private void LoadGold()
    {
        Gold = PlayerPrefs.GetInt(GoldKey, 0); // �⺻���� 0
    }

    //public static GoldManager Instance { get; private set; }

    //public int Gold { get; private set; }

    //public event Action<int> OnGoldChanged;

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    Instance = this;
    //    DontDestroyOnLoad(gameObject);
    //}

    //public void AddGold(int amount)
    //{
    //    Gold += amount;
    //    OnGoldChanged?.Invoke(Gold);
    //}

    //public bool TrySpendGold(int amount)
    //{
    //    if (Gold >= amount)
    //    {
    //        Gold -= amount;
    //        OnGoldChanged?.Invoke(Gold);
    //        return true;
    //    }
    //    return false;
    //}
}
