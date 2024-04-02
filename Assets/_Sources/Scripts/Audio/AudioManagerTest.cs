using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerTest : MonoBehaviour
{

    [SerializeField] GameObject _guitarLoop;
    [SerializeField] GameObject _bassLoop;
    [SerializeField] GameObject _guitarLicks;

    [SerializeField] float _bassComeInAfter = 8f;
    [SerializeField] float _guitarLicksComeInAfter = 16f;


    float musicTimer = 0f;
    void Start()
    {
        _bassLoop.SetActive(false);
        _guitarLicks.SetActive(false);
    }

    void Update()
    {
        musicTimer += Time.deltaTime;

        if (musicTimer >= _bassComeInAfter){
            _bassLoop.SetActive(true);
        }

        if (musicTimer >= _guitarLicksComeInAfter){
            _guitarLicks.SetActive(true);
        }
    }
}
