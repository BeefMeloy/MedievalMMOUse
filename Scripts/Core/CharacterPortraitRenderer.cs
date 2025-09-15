using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[AddComponentMenu("UI/Character Portrait Renderer")]
public class CharacterPortraitRenderer : MonoBehaviour
{
    [Header("Rendering")]
    [SerializeField] private Image portraitDisplay;
    [SerializeField] private Camera portraitCamera;
    [SerializeField] private RenderTexture portraitTexture;
    [SerializeField] private Transform portraitStage;

    [Header("Settings")]
    [SerializeField] private Vector2 textureSize = new Vector2(128, 128);
    [SerializeField] private float cameraDistance = 2f;
    [SerializeField] private Vector3 portraitOffset = new Vector3(0, 0.5f, 0);

    private Dictionary<Targetable, Sprite> portraitCache = new Dictionary<Targetable, Sprite>();
    private GameObject currentPortraitModel;

    private void Awake()
    {
        SetupPortraitCamera();
        SetupRenderTexture();
    }

    private void SetupPortraitCamera()
    {
        if (!portraitCamera)
        {
            GameObject camObj = new GameObject("PortraitCamera");
            camObj.transform.SetParent(transform);
            portraitCamera = camObj.AddComponent<Camera>();
        }

        portraitCamera.enabled = false;
        portraitCamera.clearFlags = CameraClearFlags.SolidColor;
        portraitCamera.backgroundColor = Color.clear;
        portraitCamera.cullingMask = LayerMask.GetMask("UI"); // Use UI layer for portraits
        portraitCamera.orthographic = false;
        portraitCamera.fieldOfView = 30f;
    }

    private void SetupRenderTexture()
    {
        if (!portraitTexture)
        {
            portraitTexture = new RenderTexture((int)textureSize.x, (int)textureSize.y, 16);
            portraitTexture.format = RenderTextureFormat.ARGB32;
        }

        portraitCamera.targetTexture = portraitTexture;
    }

    public void RenderCharacter(Targetable targetable)
    {
        if (!targetable) return;

        // Check cache first
        if (portraitCache.TryGetValue(targetable, out Sprite cachedSprite))
        {
            portraitDisplay.sprite = cachedSprite;
            return;
        }

        // Get character model
        var characterModel = targetable.GetComponent<CharacterModel>();
        if (!characterModel)
        {
            // Fallback to static portrait
            portraitDisplay.sprite = targetable.portrait;
            return;
        }

        // Create portrait model instance
        CreatePortraitModel(characterModel);

        // Render and cache
        RenderPortrait();
        CachePortrait(targetable);
    }

    private void CreatePortraitModel(CharacterModel sourceModel)
    {
        // Clean up previous model
        if (currentPortraitModel)
        {
            DestroyImmediate(currentPortraitModel);
        }

        // Create simplified copy for portrait
        currentPortraitModel = new GameObject("PortraitModel");
        currentPortraitModel.transform.SetParent(portraitStage);
        currentPortraitModel.transform.localPosition = portraitOffset;
        currentPortraitModel.layer = LayerMask.NameToLayer("UI");

        // Copy visual components only (no colliders, scripts, etc.)
        CopyVisualComponents(sourceModel.gameObject, currentPortraitModel);

        // Position camera
        PositionCamera();
    }

    private void CopyVisualComponents(GameObject source, GameObject destination)
    {
        // Copy mesh renderers and their materials
        var sourceMeshes = source.GetComponentsInChildren<MeshRenderer>();
        foreach (var sourceMesh in sourceMeshes)
        {
            GameObject part = new GameObject(sourceMesh.name);
            part.transform.SetParent(destination.transform);
            part.transform.localPosition = sourceMesh.transform.localPosition;
            part.transform.localRotation = sourceMesh.transform.localRotation;
            part.transform.localScale = sourceMesh.transform.localScale;
            part.layer = LayerMask.NameToLayer("UI");

            var meshFilter = part.AddComponent<MeshFilter>();
            var meshRenderer = part.AddComponent<MeshRenderer>();

            meshFilter.mesh = sourceMesh.GetComponent<MeshFilter>()?.mesh;
            meshRenderer.materials = sourceMesh.materials;
        }
    }

    private void PositionCamera()
    {
        if (!currentPortraitModel) return;

        Bounds bounds = GetModelBounds(currentPortraitModel);
        Vector3 center = bounds.center;

        // Position camera to frame the head/upper torso
        Vector3 cameraPos = center + Vector3.back * cameraDistance;
        cameraPos.y = center.y + bounds.size.y * 0.3f; // Focus on upper portion

        portraitCamera.transform.position = cameraPos;
        portraitCamera.transform.LookAt(center);
    }

    private Bounds GetModelBounds(GameObject model)
    {
        var renderers = model.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds();

        Bounds bounds = renderers[0].bounds;
        foreach (var renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
    }

    private void RenderPortrait()
    {
        portraitCamera.enabled = true;
        portraitCamera.Render();
        portraitCamera.enabled = false;

        // Convert RenderTexture to Sprite
        Texture2D texture = new Texture2D(portraitTexture.width, portraitTexture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = portraitTexture;
        texture.ReadPixels(new Rect(0, 0, portraitTexture.width, portraitTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        portraitDisplay.sprite = newSprite;
    }

    private void CachePortrait(Targetable targetable)
    {
        if (portraitDisplay.sprite && !portraitCache.ContainsKey(targetable))
        {
            portraitCache[targetable] = portraitDisplay.sprite;
        }
    }

    public void ClearCache()
    {
        foreach (var sprite in portraitCache.Values)
        {
            if (sprite && sprite.texture)
            {
                DestroyImmediate(sprite.texture);
                DestroyImmediate(sprite);
            }
        }
        portraitCache.Clear();
    }

    private void OnDestroy()
    {
        ClearCache();
        if (portraitTexture) DestroyImmediate(portraitTexture);
    }
}