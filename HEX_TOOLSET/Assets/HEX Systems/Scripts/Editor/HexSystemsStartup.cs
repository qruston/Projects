#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class HexSystemsStartup
{
    static HexSystemsStartup()
    {
        
        if (!System.IO.File.Exists(Application.dataPath + "/Editor Default Resources/SingleBrush.png"))
        {
            if(!System.IO.Directory.Exists(Application.dataPath + "/Editor Default Resources"))
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/Editor Default Resources");
            }
            FileUtil.CopyFileOrDirectory(Application.dataPath + "/Hex Systems/Editor Default Resources/SingleBrush.png", Application.dataPath + "/Editor Default Resources/SingleBrush.png");
            FileUtil.CopyFileOrDirectory(Application.dataPath + "/Hex Systems/Editor Default Resources/SmallBrush.png", Application.dataPath + "/Editor Default Resources/SmallBrush.png");
            FileUtil.CopyFileOrDirectory(Application.dataPath + "/Hex Systems/Editor Default Resources/MediumBrush.png", Application.dataPath + "/Editor Default Resources/MediumBrush.png");
            FileUtil.CopyFileOrDirectory(Application.dataPath + "/Hex Systems/Editor Default Resources/LargeBrush.png", Application.dataPath + "/Editor Default Resources/LargeBrush.png");
            FileUtil.CopyFileOrDirectory(Application.dataPath + "/Hex Systems/Editor Default Resources/FillBrush.png", Application.dataPath + "/Editor Default Resources/FillBrush.png");
        }
        Debug.Log("Hex System Startup Complete");
    }
}
#endif