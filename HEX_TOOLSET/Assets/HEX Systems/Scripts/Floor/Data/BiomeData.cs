using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.FloorTiles
{
    [CreateAssetMenu(fileName = "Biome Data", menuName = "HEX Systems/Biome Data")]
    public class BiomeData : ScriptableObject
    {
        public string BiomeID;
        public string DisplayName;
        public Texture DispplayTexture;
        public List<FloorTileData> BiomeTiles;
        public List<TopperListData> BiomeToppers;

        public FloorTileData GetFloorTile()
        {
            int index = Random.Range(0, BiomeTiles.Count);
            return BiomeTiles[index];
        }

        public TileTopperData GetTileTopper()
        {
            List<TileTopperData> Toppers = new List<TileTopperData>();
            foreach (TopperListData topperlist in BiomeToppers)
            {
                if (topperlist != null)
                {
                    Toppers.AddRange(topperlist.TileToppers);
                }
            }
            if (Toppers.Count > 0)
            {
                return Toppers[Random.Range(0, Toppers.Count)];
            }
            else
            {
                return null;
            }
        }
    }
}