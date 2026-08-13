using System;
using System.Collections.Generic;

[Serializable]
public class CustomPackSaveData
{
    public string packName;
    public string description;

    public string iconSourceType;
    public string iconSourceName;

    public List<string> spells = new();
    public List<string> accessories = new();
}