using SKC.AIF.Storage;
using UnityEngine;

namespace SKC.AIF.Processors
{
    [CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Processors) + "/" + ("Transformers") + "/" + nameof(TransformerRuleset))]
    public class TransformerRuleset : ScriptableObject
    {
        public ObjectDefinitionCountPair[] Inputs;
        public ObjectDefinitionCountPair[] Outputs;
    }
}
