using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gameplay.FloorTiles
{
    public class PropBase : MonoBehaviour
    {
        public PropData m_PropData;

        private List<FloorTile> m_AttachedFloorTiles = new List<FloorTile>();

        public void AttachFloorTile(FloorTile tile)
        {
            if (!m_AttachedFloorTiles.Contains(tile))
                m_AttachedFloorTiles.Add(tile);
        }

        public void DetachFloorTile(FloorTile tile)
        {
            if(m_AttachedFloorTiles.Contains(tile))
                m_AttachedFloorTiles.Remove(tile);
        }

        public void DetachFromAllFloorTiles()
        {
            foreach (FloorTile tile in m_AttachedFloorTiles)
            {
                tile.DetachAndKeepProp();
            }
            m_AttachedFloorTiles.Clear();
        }

        public void SetMutualHeight(float height)
        {
            foreach (FloorTile tile in m_AttachedFloorTiles)
            {
                tile.SetHeight(height);
            }
        }
    }
}