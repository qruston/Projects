using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingButton : BaseButton
{
    [SerializeField]
    private ChipData m_ChipData;//The chip that this button bets 
    [SerializeField]
    private int m_Amount; // Amount that gets changed 
    [SerializeField]
    private bool m_Increase = true;//Wether this button increases the bet or decreases it 
    

    protected override void Start()
    {
        base.Start();

        UIHandler.Instance.ChangeMade += OnChangeMade;//Hook the Change Made method into the on change made delegate
    }

    /// <summary>
    /// Method called when the button is clicked
    /// </summary>
    public override void ButtonClick()
    {
        GameManager.Instance.AdjustBet(m_ChipData, m_Amount);//Adjust the betting with the chipdata
    }

    /// <summary>
    /// When a change is made to the betting this button needs to check if it should still be able to be used
    /// </summary>
    public void OnChangeMade()
    {
        if ((m_Increase && GameManager.Instance.CanChipIncrease(m_ChipData)) || (!m_Increase && GameManager.Instance.CanChipDecrease(m_ChipData)))//Check wether this chip can use this betting button
        {
            UnlockButton();//Unlock this button
        }
        else
        {
            LockButton();//Lock this button
        }
    }
}
