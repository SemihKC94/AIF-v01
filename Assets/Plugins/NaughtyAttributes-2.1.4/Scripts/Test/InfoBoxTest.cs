using SKC.AIF.NaughtyAttributes_2._1._4.Core.DrawerAttributes;
using UnityEngine;

namespace SKC.AIF.NaughtyAttributes_2._1._4.Test
{
    public class InfoBoxTest : MonoBehaviour
    {
        [InfoBox("Normal", EInfoBoxType.Normal)]
        public int normal;

        public InfoBoxNest1 nest1;
    }

    [System.Serializable]
    public class InfoBoxNest1
    {
        [InfoBox("Warning", EInfoBoxType.Warning)]
        public int warning;

        public InfoBoxNest2 nest2;
    }

    [System.Serializable]
    public class InfoBoxNest2
    {
        [InfoBox("Error", EInfoBoxType.Error)]
        public int error;
    }
}
