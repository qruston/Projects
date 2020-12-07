using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationButton : BaseButton
{
    [SerializeField]
    private int m_ColourID = 0;

    protected override void Start()
    {
        base.Start();
        UIHandler.Instance.LockConfirmation += LockButton;//Hook in Lock button to delegate
        UIHandler.Instance.UnlockConfirmation += UnlockButton;//Hook in unlock button to delegate
    }

    public override void ButtonClick()
    {
        GameManager.Instance.SelectColour(m_ColourID);
    }
}
