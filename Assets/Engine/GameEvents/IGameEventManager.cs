
namespace Core.GameEvents{
    public interface IGameEventManager{
        public void HandleTriggerEvents(EventType eventType, string senderName);
        public void HandleCollisionEvents(EventType eventType, string senderName);
        public void HandleRaycastEvents(EventType eventType, string senderName);
        public void HandleSpecialCases(EventType eventType, string senderName);
    }
}