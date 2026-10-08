using UnityEngine;

/// <summary>Hull armour affects direct hits. Splash damage is unaffected.</summary>
public static class DuelArmor
{
    public static float GetDirectHitMultiplier(Transform tank, Vector3 impactPoint, Vector3 incomingVelocity)
    {
        Vector3 direction = impactPoint - tank.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.01f)
        {
            direction = -incomingVelocity;
            direction.y = 0f;
        }
        if (direction.sqrMagnitude < 0.01f) return 1f;
        Vector3 forward = tank.forward;
        forward.y = 0f;
        float facing = Vector3.Dot(forward.normalized, direction.normalized);
        if (facing >= 0.5f) return 0.75f;
        if (facing <= -0.5f) return 1.5f;
        return 1f;
    }
}
