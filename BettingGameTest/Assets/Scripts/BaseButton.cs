using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    Button button;
    protected virtual void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();//Get the button component 
        }
    }

    /// <summary>
    /// Method that fires when the button is clicked
    /// </summary>
    public virtual void ButtonClick()
    {

    }

    /// <summary>
    /// Locks this button when called
    /// </summary>
    public void LockButton()
    {
        button.interactable = false;
    }

    /// <summary>
    /// Unlocks this button when called
    /// </summary>
    public void UnlockButton()
    {
        button.interactable = true;
    }
}
