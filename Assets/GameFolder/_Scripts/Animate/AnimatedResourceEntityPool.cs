using SKC.AIF.Animate;
using UnityEngine;

namespace SKC.AIF.Pool
{
    [CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Pool) + "/" + nameof(AnimatedResourceEntityPool))]
    public class AnimatedResourceEntityPool : Pool<AnimatedResourceEntity>
    {
    }
}
