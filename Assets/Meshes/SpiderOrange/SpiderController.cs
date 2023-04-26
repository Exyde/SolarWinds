using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    [SerializeField] Transform _body;

    void FixedUpdate()
    {
        float multiplier = 1f;
        if (Input.GetKey(KeyCode.LeftShift))
            multiplier = 2f;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _body.position += move * Time.deltaTime * _speed * multiplier;
    }
}