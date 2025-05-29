using UnityEngine;
using SKC.AIF.Storage;
using ObjectItem = SKC.AIF.Storage.ObjectItem;

namespace SKC.AIF.Pool
{
    [CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Pool) + "/" + nameof(ObjectPool))]
    public class ObjectPool : Pool<ObjectItem>
    {
        public ObjectDefinition ItemDefinition => Behaviour.Definition;
    }
}
