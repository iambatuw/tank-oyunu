using UnityEngine;

/// <summary>Frames moving tanks in the playable area below the HUD.</summary>
public sealed class DuelCameraFraming
{
    public const float MinimumSize = 15f;
    public const float PlayerScreenHeight = 0.43f;
    private Vector3 focus;
    private Vector3 velocity;
    private float zoomVelocity;
    private bool initialized;

    public void Update(Camera camera, Transform[] targets, bool solo, float deltaTime, bool snap = false)
    {
        if (camera == null || targets == null || targets.Length == 0) return;
        // SmoothDamp's zoom velocity must never divide by a paused frame's zero time.
        if (initialized && !snap && deltaTime <= 0f) return;
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (var target in targets)
        {
            if (target == null || !target.gameObject.activeInHierarchy) continue;
            desired += target.position;
            count++;
        }
        if (count == 0) return;
        desired /= count;
        if (solo && targets[0] != null && targets[0].gameObject.activeInHierarchy)
            desired = targets[0].position;

        if (snap || !initialized)
        {
            focus = desired;
            velocity = Vector3.zero;
            zoomVelocity = 0f;
        }
        else
            focus = Vector3.SmoothDamp(focus, desired, ref velocity, 0.12f,
                Mathf.Infinity, Mathf.Max(0f, deltaTime));

        // A consistent heading makes moving toward the top of the arena easy to read.
        camera.transform.rotation = Quaternion.Euler(55f, 0f, 0f);
        camera.orthographic = true;
        float required = MinimumSize;
        foreach (var target in targets)
        {
            if (target == null || !target.gameObject.activeInHierarchy) continue;
            // Solo play follows the player's tank at a useful zoom. A distant bot
            // is indicated at the screen edge instead of shrinking the whole arena.
            if (solo && targets[0] != null && targets[0].gameObject.activeInHierarchy && target != targets[0]) continue;
            Vector3 offset = target.position - focus;
            float horizontal = Mathf.Abs(Vector3.Dot(offset, camera.transform.right));
            float vertical = Vector3.Dot(offset, camera.transform.up);
            // Reserve 16% at the top for the HUD and leave room for each tank's hull.
            required = Mathf.Max(required, (horizontal + 3f) / (0.8f * camera.aspect));
            required = Mathf.Max(required, (Mathf.Abs(vertical) + 3f) / (vertical >= 0f ? 0.82f : 0.66f));
        }

        // Zoom out immediately to keep moving targets visible; zoom back in gently.
        float size = snap || !initialized || required > camera.orthographicSize ? required :
            Mathf.SmoothDamp(camera.orthographicSize, required, ref zoomVelocity, 0.35f,
                Mathf.Infinity, Mathf.Max(0f, deltaTime));
        camera.orthographicSize = size;
        camera.transform.position = focus - camera.transform.forward * 100f +
            camera.transform.up * (size * (1f - 2f * PlayerScreenHeight));
        initialized = true;
    }
}
