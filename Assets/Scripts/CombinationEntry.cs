using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombinationEntry
{
    public List<string> ingredients; // ���� ID 2�� (��: "Tomato", "Potato")
    public string resultSeedId;     // ��� ���� ID (��: "TomatoPotato")
}
