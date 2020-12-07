using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : Singleton<UIHandler>//UI is singleton for easy access from buttons and game manager
{
    [SerializeField]
    private GameObject m_BettingPanel;
    [SerializeField]
    private Image m_WinnerImage;

    public delegate void UIChangeDelegate();
    public UIChangeDelegate LockConfirmation;//Delegate that will get called when wanting to lock all Colour Selection Buttons
    public UIChangeDelegate UnlockConfirmation;//Delegate that will get called when wanting to unlock all Colour Selection Buttons
    public UIChangeDelegate ChangeMade;//Delegate that will get called when a change has been made to the chips that the betting buttons will need to check

    /// <summary>
    /// Shows the betting panel
    /// </summary>
    public void ShowUI()
    {
        m_BettingPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the betting panel
    /// </summary>
    public void HideUI()
    {
        m_BettingPanel.SetActive(false);
    }

    /// <summary>
    /// Shows the winner image
    /// </summary>
    /// <param name="winner"></param>
    public void ShowWinner(Color winner)
    {
        m_WinnerImage.gameObject.SetActive(true);
        m_WinnerImage.color = winner;
    }

    /// <summary>
    /// Hides the winning image 
    /// </summary>
    public void HideWinner()
    {
        m_WinnerImage.gameObject.SetActive(false);
    }
}
