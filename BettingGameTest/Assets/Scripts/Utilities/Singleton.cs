///Used this site https://wiki.unity3d.com/index.php/Singleton as a basis for this class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T m_Instance;

    public static T Instance { get { return m_Instance; } }

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this) //If this singleton already exists 
        {
            Destroy(this.gameObject);//Destroy this new instance of it 
        }
        else
        {
            m_Instance = FindObjectOfType<T>();//Find this Singleton and set it to the instance
        }
    }
}
