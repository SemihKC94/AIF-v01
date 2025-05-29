using System.Collections.Generic;
using SKC.AIF.NaughtyAttributes_2._1._4.Core.DrawerAttributes;
using UnityEngine;

namespace SKC.AIF.NaughtyAttributes_2._1._4.Test
{
    //[CreateAssetMenu(fileName = "NaughtyScriptableObject", menuName = "NaughtyAttributes/_NaughtyScriptableObject")]
    public class _NaughtyScriptableObject : ScriptableObject
    {
        [Expandable]
        public List<_TestScriptableObjectA> listA;
    }
}
