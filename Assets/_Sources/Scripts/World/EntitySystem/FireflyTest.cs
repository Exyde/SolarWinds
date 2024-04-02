using UnityEngine;
using Systems.Entities;
public class FireflyTest : Entity
{
    public GameObject _target; //Player
    public float _speed = 5f;
    public float timeBetweenTargetUpdate = 1f;

    Vector3 targetPos;

    void Start()
    {
        _target = FindObjectOfType<PlayerController>().gameObject;
        InvokeRepeating("UpdateTargetPos", 0f, timeBetweenTargetUpdate);

        _speed += Random.Range(-1f, 2f);
        timeBetweenTargetUpdate += Random.Range(.1f, 6f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime);
    }

    void UpdateTargetPos(){
        targetPos = _target.transform.position;
        transform.LookAt(targetPos);
    }

    public override void Tick()
    {
        base.Tick();
    }
}
