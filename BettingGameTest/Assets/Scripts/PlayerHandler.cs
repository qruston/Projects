using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private Dictionary<string, StoredChips> m_PlayerChips = new Dictionary<string, StoredChips>();

    /// <summary>
    /// changes the players chip count based on given amount. Returned bool tells weather the change chip has dropped below 0
    /// </summary>
    /// <param name="ChipIdentifier"></param>
    /// <param name="amount"></param>
    public bool AdjustChips(ChipData Chip, int amount)
    {
        
        //Check if player already has this type of chip
        if (m_PlayerChips.ContainsKey(Chip.ChipID))
        {
            //Update Chip Amount
            m_PlayerChips[Chip.ChipID].Amount += amount;
        }
        else
        {
            //Add Chip to Players Chips
            m_PlayerChips.Add(Chip.ChipID, new StoredChips(Chip));
            //Update the amount
            m_PlayerChips[Chip.ChipID].Amount += amount;
        }

        if (m_PlayerChips[Chip.ChipID].Amount <= 0)
        {
            m_PlayerChips[Chip.ChipID].Amount = 0;
            return false;
        }


        return true;
    }

    /// <summary>
    /// Returns a chip count for a specific type of chip
    /// </summary>
    /// <param name="ChipID"></param>
    /// <returns></returns>
    public int GetChipCount(string ChipID)
    {
        if (m_PlayerChips.ContainsKey(ChipID))
            return m_PlayerChips[ChipID].Amount;
        else
            return 0;
    }

    /// <summary>
    /// Go through the dictionary and Check the amounts on all chips
    /// </summary>
    /// <returns></returns>
    public bool VerifyChipCounts()
    {
        foreach (KeyValuePair<string, StoredChips> keyValuePair in m_PlayerChips)// loop through each key value pair in the the player chips 
        {
            if (keyValuePair.Value.Amount > 0)//If the amount is greater then 0
            {
                return true;//Exit out early
            }
        }

        //All Chip amounts are at or below 0 
        return false;
    }
}


