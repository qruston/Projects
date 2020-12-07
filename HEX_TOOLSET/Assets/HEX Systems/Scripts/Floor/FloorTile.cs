using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gameplay.FloorTiles
{
    public class FloorTile : MonoBehaviour
    {
        private FloorTileData m_TileData;

        
        [HideInInspector]
        public Vector2 m_MapPosition; // The position of this tile in the Entire Map.
        [HideInInspector]
        public bool OuterTile; //Weather this tile is an outer tile
        [HideInInspector]
        public TileTopperData m_TopperData;
        //[HideInInspector]
        public GameObject m_Topper;
        public GameObject m_HexParent;
        public GameObject m_HexBody;
        public PropBase m_Prop;

        [SerializeField]
        private Renderer m_SurfaceRenderer;
        [SerializeField]
        private Renderer m_BodyRenderer;

        [SerializeField]
        private GameObject m_SelectionObject;

        private float m_SetHeight;
        private float m_TempHeight;

        [SerializeField]
        private List<FloorTile> m_ConnectedTiles = new List<FloorTile>(6);

        public List<FloorTile> ConnectedTiles
        {
            get
            {
                return m_ConnectedTiles;
            }
            set
            {
                m_ConnectedTiles = value;
            }
            
        }

        /// <summary>
        /// Initializes the Tile, Call when First Created 
        /// </summary>
        /// <param name="tileData"></param>
        public void InitializeTile(FloorTileData tileData)
        {
            //Set up connected tile list 
            for (int i = 0; i < 6; i++)
            {
                m_ConnectedTiles.Add(null);
            }
            //Set the Given Tile Data
            m_TileData = tileData;
            //set the Material for the tile
            m_SurfaceRenderer.material = m_TileData.SurfaceMaterial;
            m_BodyRenderer.material = m_TileData.BodyMaterial;
            ////Destroy any Topper that is Currently Attached to this tile
            //if (m_Topper != null)
            //{
            //    DestroyImmediate(m_Topper);
            //}
            ////Check if the Tile Data Holds any tile toppers 
            //if (m_TileData.tileToppers != null && m_TileData.tileToppers.Count > 0)
            //{
            //    m_TopperData = m_TileData.GetTopper();
            //    //Create the Tile topper and position it 
            //    m_Topper = Instantiate(m_TopperData.TopperPrefab, transform);
            //    m_Topper.transform.localPosition = Vector3.zero;
            //    m_Topper.transform.localScale = new Vector3( m_HexParent.transform.localScale.x, m_HexParent.transform.localScale.x, m_HexParent.transform.localScale.x);
            //}
        }

        /// <summary>
        /// Similat to the initialization Method, used to Change the Biome of the Tile
        /// </summary>
        /// <param name="tileData"></param>
        public void SetNewBiome(FloorTileData tileData)
        {
            //Set the Tile Data
            m_TileData = tileData;
            //Set the Material of the tile
            m_SurfaceRenderer.material = m_TileData.SurfaceMaterial;
            m_BodyRenderer.material = m_TileData.BodyMaterial;
            ////Destroy Tile topper if there is one
            //if (m_Topper != null)
            //{
            //    DestroyImmediate(m_Topper);
            //}
            ////Check if the Tile Data Holds any tile toppers 
            //if (m_TileData.tileToppers != null && m_TileData.tileToppers.Count > 0)
            //{
            //    m_TopperData = m_TileData.GetTopper();
            //    //Create the Tile topper and position it 
            //    m_Topper = Instantiate(m_TopperData.TopperPrefab, transform);
            //    m_Topper.transform.localPosition = Vector3.zero;
            //    m_Topper.transform.localScale = new Vector3(m_HexParent.transform.localScale.x, m_HexParent.transform.localScale.x, m_HexParent.transform.localScale.x);
            //}
        }

        /// <summary>
        /// Retrieve the Biome that this tile is apart of 
        /// </summary>
        /// <returns></returns>
        public string GetBiome()
        {
            return m_TileData.biome;
        }

        public void SetTileTopper(TileTopperData tileTopperData,bool  RandomizeRotation = false)
        {
            //Destroy Tile topper if there is one
            if (m_Topper != null)
            {
                DestroyImmediate(m_Topper);
            }
            //Check if the Tile Data Holds any tile toppers 
            if (tileTopperData != null)
            {
                m_TopperData = tileTopperData;
                if (m_TopperData.TopperPrefab)
                {

#if UNITY_EDITOR
                    m_Topper = (GameObject)PrefabUtility.InstantiatePrefab(m_TopperData.TopperPrefab);
                    m_Topper.transform.SetParent(transform);
#else
                    //Create the Tile topper and position it 
                    m_Topper = Instantiate(m_TopperData.TopperPrefab, transform);
#endif

                    m_Topper.transform.localPosition = Vector3.zero;
                    m_Topper.transform.localScale = new Vector3(m_HexParent.transform.localScale.x, m_HexParent.transform.localScale.x, m_HexParent.transform.localScale.x);
                    if (RandomizeRotation)
                    {
                        Vector3 NewRot = m_Topper.transform.localEulerAngles;
                        NewRot.y = Random.Range(-360, 360);
                        m_Topper.transform.localEulerAngles = NewRot;
                    }
                }
            }
        }

        public void AttachProp(PropBase prop)
        {
            if (m_Topper)
            {
                DestroyImmediate(m_Topper);
            }
            if (m_Prop && m_Prop != prop)
            {
                DestroyImmediate(m_Prop.gameObject);
            }

            m_Prop = prop;
            m_Prop.transform.SetParent(transform);
        }

        public void DetachAndKeepProp()
        {
            m_Prop = null;
        }

        /// <summary>
        /// Return wether this tile is buildable or not
        /// </summary>
        /// <returns></returns>
        public bool IsBuildable()
        {
            return m_TileData.Buildable;
        }

        /// <summary>
        /// Method called when this tile gets selected
        /// </summary>
        /// <param name="Selected"></param>
        public void SetSelected(bool Selected)
        {
            if (m_SelectionObject)
                m_SelectionObject.SetActive(Selected);
        }


        public void SetHeight(float Height)
        {
            //Set the Tiles Position to the new height 
            transform.localPosition = new Vector3(transform.localPosition.x, Height, transform.localPosition.z);
            //Set the Tiles Hex Scale to the new height
            m_HexBody.transform.localScale = new Vector3(
                m_HexBody.transform.localScale.x,
                ((Height + 1 + m_HexBody.transform.localPosition.y) / (1 + m_HexBody.transform.localPosition.y)),
                m_HexBody.transform.localScale.z
                );
            m_SetHeight = Height;
        }

        public void SetTempHeight(float Height)
        {
            //Set the Tiles Position to the new height 
            transform.localPosition = new Vector3(transform.localPosition.x, Height, transform.localPosition.z);
            //Set the Tiles Hex Scale to the new height
            m_HexBody.transform.localScale = new Vector3(
                m_HexBody.transform.localScale.x,
                ((Height + 1 + m_HexBody.transform.localPosition.y) / (1 + m_HexBody.transform.localPosition.y)),
                m_HexBody.transform.localScale.z
                );
            m_TempHeight = Height;
        }

        public void SetTempToMainHeight()
        {
            SetHeight(m_TempHeight);
        }

        public void ResetHeight()
        {
            SetHeight(m_SetHeight);
        }
    }
}