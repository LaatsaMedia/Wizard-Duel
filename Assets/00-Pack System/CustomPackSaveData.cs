using System;
using System.Collections.Generic;

[Serializable]
public class CustomPackSaveData
{
    public string packName;
    public string description;

    public List<string> spells = new();
    public List<string> accessories = new();
}