using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.FloorTiles;
namespace Gameplay.HexGeneration
{
    [CreateAssetMenu(fileName = "Generation Data", menuName = "HEX Systems/Generation Data")]
    public class GenerationData : ScriptableObject
    {

        [Header("Generation Settings")]
        public List<Vector3> HexOffset;
        public List<Vector2> m_MapPositionOffsets = new List<Vector2>();
        public Vector3 HexScale;
        [Tooltip("These are the layers that are used byt the tool for the paint functionality, You will want to make sure props are on a layer that is not selected in this mask.")]
        public LayerMask PaintAndPlacementLayers;
        [Tooltip("These are the layers that are used byt the tool for the paint functionality, You will want to make sure props are on a layer that is not selected in this mask.")]
        public LayerMask PropSelectionLayers;

        [Header("Biome Settings")]
        public BiomeData DefaultBiomeData;
        public float ChanceForSameBiome;
        public List<BiomeData> BiomeSets;
        public List<PropListData> Props;

    }
}