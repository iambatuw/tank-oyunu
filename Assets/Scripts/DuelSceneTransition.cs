using UnityEngine;

// A scene may consume its launch request once. Requests never survive a game restart.
public static class DuelSceneTransition
{
    public static bool IsLoading { get; private set; }
    private static string destination;
    private static bool startMatch;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset()
    {
        IsLoading = false;
        destination = null;
        startMatch = false;
    }

    public static bool TryBegin(string scene, bool play)
    {
        if (IsLoading || string.IsNullOrEmpty(scene)) return false;
        IsLoading = true;
        destination = scene;
        startMatch = play;
        return true;
    }

    public static bool ConsumeMatchStart(string scene)
    {
        if (!IsLoading || destination != scene || !startMatch) return false;
        startMatch = false;
        return true;
    }

    public static void Complete(string scene)
    {
        if (destination == scene) Reset();
    }
}
