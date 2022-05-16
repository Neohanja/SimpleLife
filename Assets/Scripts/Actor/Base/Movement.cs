using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Base Stats")]
    public float speed;
    protected Vector3 momentum;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        GameLoop();
    }

    public virtual void BuildLocomotion()
    {

    }

    protected virtual void Initialize()
    {

    }

    protected virtual void GameLoop()
    {
        GetMovement();

        transform.position += momentum * speed * Time.deltaTime;
        momentum = Vector3.zero;
    }

    protected virtual void GetMovement()
    {

    }

    public virtual void SetMovement(Vector2 direction)
    {
        direction.Normalize();

        momentum = direction;
    }
}
