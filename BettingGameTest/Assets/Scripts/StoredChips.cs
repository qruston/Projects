using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoredChips
{
    public ChipData chipData;
    public int Amount;

    public StoredChips(ChipData chip)
    {
        chipData = chip;
    }
}
