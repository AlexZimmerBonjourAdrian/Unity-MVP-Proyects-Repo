using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RetroFPS
{
    public interface IDamage
    {
        void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal);
    }
}
