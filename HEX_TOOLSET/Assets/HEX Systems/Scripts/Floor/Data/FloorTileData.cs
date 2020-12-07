using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.FloorTiles
{
    [CreateAssetMenu(fileName = "Floor Tile Data", menuName = "HEX Systems/Floor Tile Data")]
    public class FloorTileData : ScriptableObject
    {
        [Tooltip("ID of biome this tile is apart of.")]
        public string biome;
        [Tooltip("ID for what type of floor this is.")]
        public string floorAudioID;
        [Tooltip("Is this Tile Able to be built on")]
        public bool Buildable;
        [Tooltip("Material Of this tile")]
        public Material SurfaceMaterial;
        [Tooltip("Material Of the Body for this tile")]
        public Material BodyMaterial;
        [Tooltip("Prefab for this tile.")]
        public GameObject tilePrefab;
    }
}