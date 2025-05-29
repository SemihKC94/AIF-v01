using System.Collections.Generic;
using SKC.AIF.NaughtyAttributes_2._1._4.Core.DrawerAttributes;
using UnityEngine;

namespace SKC.AIF.NaughtyAttributes_2._1._4.Test
{
    //[CreateAssetMenu(fileName = "TestScriptableObjectA", menuName = "NaughtyAttributes/TestScriptableObjectA")]
    public class _TestScriptableObjectA : ScriptableObject
    {
        [Expandable]
        public List<_TestScriptableObjectB> listB;
    }
}