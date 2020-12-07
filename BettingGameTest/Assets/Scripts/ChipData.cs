using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ChipData", menuName = "ScriptableObjects/ChipData", order = 1)]
public class ChipData : ScriptableObject
{
    public string ChipID;//The id of the chip
    public Color ChipColour;//Colour of the chip
}
