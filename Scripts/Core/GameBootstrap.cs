<<<<<<< HEAD
// Unity C# – MonoBehaviour Component
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameBootstrap : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("Scene NAME only (no path, no .unity).")]
    public string worldSceneName = "World_01";

    [Tooltip("Tag on the spawn point object inside the world scene.")]
    public string playerSpawnTag = "PlayerSpawn";

    [Tooltip("Prefab to spawn as the player.")]
    public GameObject playerPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Boot());
    }

    private IEnumerator Boot()
    {
        // 1) Load the world scene (replace current)
        var op = SceneManager.LoadSceneAsync(worldSceneName, LoadSceneMode.Single);
        while (!op.isDone) yield return null;

        // 2) Find spawn
        var spawn = GameObject.FindGameObjectWithTag(playerSpawnTag);
        if (!spawn)
        {
            Debug.LogError($"No object tagged '{playerSpawnTag}' found in the world scene.");
            yield break;
        }

        // 3) Validate prefab
        if (!playerPrefab)
        {
            Debug.LogError("Player Prefab not assigned on GameBootstrap (_Runtime).");
            yield break;
        }

        // 4) Spawn player
        var player = Instantiate(playerPrefab, spawn.transform.position, spawn.transform.rotation);
        player.name = "Player";

        // 5) Ensure a Main Camera exists (and only one AudioListener)
        var mainCam = Camera.main;
        if (!mainCam)
        {
            var camGO = new GameObject("Main Camera");
            mainCam = camGO.AddComponent<Camera>();
            mainCam.tag = "MainCamera";
            camGO.AddComponent<AudioListener>();
        }
        else
        {
            // Make sure only one AudioListener is enabled
            var listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None); // FIXED: Complete the call
            bool kept = false;
            foreach (var l in listeners)
            {
                if (!kept) { l.enabled = true; kept = true; }
                else l.enabled = false;
            }
        }

        // 6) Put camera near the player immediately (so you see something)
        mainCam.transform.position = player.transform.position + new Vector3(0f, 4.5f, -7f);
        mainCam.transform.LookAt(player.transform.position + Vector3.up * 1.6f);

        // 7) Add our orbit controller and point it at the player
        var orbit = mainCam.GetComponent<MMBOrbitCamera>();
        if (!orbit) orbit = mainCam.gameObject.AddComponent<MMBOrbitCamera>();
        orbit.target = player.transform;
        orbit.SetYaw(player.transform.eulerAngles.y);   // start behind player
        orbit.SetPitch(15f);                            // slight downward look

        // 8) NEW: hand the camera to TargetEnemy so hover/click works immediately
        var targetEnemy = FindFirstObjectByType<TargetEnemy>(); // Unity 6.x
        if (targetEnemy != null) targetEnemy.SetCamera(mainCam);
        // (Unity 2021 fallback would be: FindObjectOfType<TargetEnemy>() )
    }
=======
// Unity C# – MonoBehaviour Component
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameBootstrap : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("Scene NAME only (no path, no .unity).")]
    public string worldSceneName = "World_01";

    [Tooltip("Tag on the spawn point object inside the world scene.")]
    public string playerSpawnTag = "PlayerSpawn";

    [Tooltip("Prefab to spawn as the player.")]
    public GameObject playerPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Boot());
    }

    private IEnumerator Boot()
    {
        // 1) Load the world scene (replace current)
        var op = SceneManager.LoadSceneAsync(worldSceneName, LoadSceneMode.Single);
        while (!op.isDone) yield return null;

        // 2) Find spawn
        var spawn = GameObject.FindGameObjectWithTag(playerSpawnTag);
        if (!spawn)
        {
            Debug.LogError($"No object tagged '{playerSpawnTag}' found in the world scene.");
            yield break;
        }

        // 3) Validate prefab
        if (!playerPrefab)
        {
            Debug.LogError("Player Prefab not assigned on GameBootstrap (_Runtime).");
            yield break;
        }

        // 4) Spawn player
        var player = Instantiate(playerPrefab, spawn.transform.position, spawn.transform.rotation);
        player.name = "Player";

        // 5) Ensure a Main Camera exists (and only one AudioListener)
        var mainCam = Camera.main;
        if (!mainCam)
        {
            var camGO = new GameObject("Main Camera");
            mainCam = camGO.AddComponent<Camera>();
            mainCam.tag = "MainCamera";
            camGO.AddComponent<AudioListener>();
        }
        else
        {
            // Make sure only one AudioListener is enabled
            var listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None); // FIXED: Complete the call
            bool kept = false;
            foreach (var l in listeners)
            {
                if (!kept) { l.enabled = true; kept = true; }
                else l.enabled = false;
            }
        }

        // 6) Put camera near the player immediately (so you see something)
        mainCam.transform.position = player.transform.position + new Vector3(0f, 4.5f, -7f);
        mainCam.transform.LookAt(player.transform.position + Vector3.up * 1.6f);

        // 7) Add our orbit controller and point it at the player
        var orbit = mainCam.GetComponent<MMBOrbitCamera>();
        if (!orbit) orbit = mainCam.gameObject.AddComponent<MMBOrbitCamera>();
        orbit.target = player.transform;
        orbit.SetYaw(player.transform.eulerAngles.y);   // start behind player
        orbit.SetPitch(15f);                            // slight downward look

        // 8) NEW: hand the camera to TargetEnemy so hover/click works immediately
        var targetEnemy = FindFirstObjectByType<TargetEnemy>(); // Unity 6.x
        if (targetEnemy != null) targetEnemy.SetCamera(mainCam);
        // (Unity 2021 fallback would be: FindObjectOfType<TargetEnemy>() )
    }
>>>>>>> 80f95843f249f1039bc541cb63c6e1bbbb3f5fac
}