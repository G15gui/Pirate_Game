using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public Attack attack;
    void Update()
    {
        //var transform = attack.attack.transform;
        transform.position = Vector2.MoveTowards(transform.position, attack.target, attack.speed * Time.deltaTime);
        if (transform.position == (Vector3)attack.target)
        {
            Destroy(this.gameObject);
        }
    }
}
