using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoostable
{
    void Boost(Vector2 force, ForceMode2D forceType);
}
