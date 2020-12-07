using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChipPosition : MonoBehaviour
{
    [SerializeField]
    string m_ChipID = "";//ID of what chip will get placed at this postion
    [SerializeField]
    bool m_BetPool = false;//Wether this position is for the player or the bet pool
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.AddChipLocations(m_ChipID,m_BetPool, transform);//Adds this location to the chip locations dictionary
    }
}
