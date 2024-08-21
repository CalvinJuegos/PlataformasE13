using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICollisionHandler
{
    void HandleCollision(Collider2D other);
}
