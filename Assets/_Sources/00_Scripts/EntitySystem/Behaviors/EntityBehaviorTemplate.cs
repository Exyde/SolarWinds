using EnhancedEditor;
using UnityEngine;

namespace Systems.Entities
{
    [CreateAssetMenu(fileName = "EBT_New_Template", menuName = "Entity/EntityBehaviorTemplate")]
    public class EntityBehaviorTemplate : ScriptableObject
    {
        [SerializeField] public PolymorphValue<EntityBehavior> behavior;
    }
}