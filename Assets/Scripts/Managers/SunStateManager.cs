using UnityEngine;

public class SunStateManager : MonoBehaviour
{
    public enum SequenceMode { Sunborn, Sunset, Sunrise, Sunburn, Sundeath, Eclipse}
    [SerializeField] SequenceMode _sequenceMode = SequenceMode.Sunborn;

    private void Start() {
        if (_sequenceMode == SequenceMode.Sunborn) return;
    }
}
