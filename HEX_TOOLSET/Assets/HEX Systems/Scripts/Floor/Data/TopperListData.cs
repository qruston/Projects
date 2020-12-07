using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.FloorTiles
{
    [CreateAssetMenu(fileName = "Tile Topper List Data", menuName = "HEX Systems/Tile Topper List Data")]
    public class TopperListData : ScriptableObject
    {
        public List<TileTopperData> TileToppers;
    }
}