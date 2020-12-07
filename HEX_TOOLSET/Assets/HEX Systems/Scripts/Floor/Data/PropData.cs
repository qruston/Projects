using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.FloorTiles
{
    [CreateAssetMenu(fileName = "Prop Data", menuName = "HEX Systems/Prop Data")]
    public class PropData : ScriptableObject
    {
        public string ID;
        public string DisplayName;
        public float FlattenRadius = 1;
        [Tooltip("The Icon that is used in the Hex Terrain Generation Inspector")]
        public Texture EditorIcon;
        public GameObject PropPrefab;
    }
}