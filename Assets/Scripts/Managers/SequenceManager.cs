using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour //Todo : GameMode Manager ?
{
    public enum SequenceMode { PrePoetique, Tension, Poetique, TempsSuspendu}
    [SerializeField] SequenceMode _sequenceMode = SequenceMode.PrePoetique;

    private void Start() {
        if (_sequenceMode == SequenceMode.PrePoetique) return;
    }

}
