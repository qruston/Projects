using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.FloorTiles
{
    [CreateAssetMenu(fileName = "Tile Topper Data", menuName = "HEX Systems/Tile Topper Data")]
    public class TileTopperData : ScriptableObject
    {
        public string ID;
        public string DisplayName;
        [Tooltip("The Icon that is used in the Hex Terrain Generation Inspector")]
        public Texture EditorIcon;
        public GameObject TopperPrefab;
    }
}