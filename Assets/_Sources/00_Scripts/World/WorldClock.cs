using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldClock : MonoBehaviour
{
    public static WorldClock _instance;

    [SerializeField] bool _enableClock;
    [SerializeField][Range(1, 120)] int _tickPerMinute = 60;
    public float timeBtwTicks;

    [SerializeField] List<ITickable> _tickablesObjects;

    void Awake() {
        if (_instance == null){
            _instance = this;
        } else{
            Destroy(this);
        }

        _tickablesObjects = new List<ITickable>();
    }
    void Start()
    {
        StartCoroutine(DispatchTicks());
    }

    private IEnumerator DispatchTicks(){

        timeBtwTicks = 60f / _tickPerMinute;

        if (_enableClock){
            foreach (ITickable tickable in _tickablesObjects){
                tickable.Tick();
            }
        }

        yield return new WaitForSeconds(timeBtwTicks);

        StartCoroutine(DispatchTicks());
    }

    public void RegisterTickable(ITickable item){
        _tickablesObjects.Add(item);
    }

    public void UnregisterTickable(ITickable item){
        _tickablesObjects.Remove(item);
    }
}

public interface ITickable{
    public void Tick();
}
