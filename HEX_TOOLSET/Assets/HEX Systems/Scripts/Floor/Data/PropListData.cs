using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.FloorTiles
{
    [CreateAssetMenu(fileName = "Prop List Data", menuName = "HEX Systems/Prop List Data")]
    public class PropListData : ScriptableObject
    {
        public List<PropData> Props;
    }
}
