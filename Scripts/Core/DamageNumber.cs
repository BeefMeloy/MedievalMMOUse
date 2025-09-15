using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float fadeSpeed = 1f;
    public float lifetime = 2f;

    private TextMeshPro textMesh;
    private Color originalColor;
    private Vector3 floatDirection;
    private float startTime;

    public void Initialize(string damageText, Color color)
    {
        // Create text mesh
        GameObject textObject = new GameObject("DamageText");
        textObject.transform.SetParent(transform);
        textObject.transform.localPosition = Vector3.zero;

        textMesh = textObject.AddComponent<TextMeshPro>();
        textMesh.text = damageText;
        textMesh.fontSize = 4;
        textMesh.color = color;
        textMesh.alignment = TextAlignmentOptions.Center;

        // Fix: Access sorting layer through the renderer
        Renderer renderer = textMesh.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = "UI";
            renderer.sortingOrder = 100;
        }

        originalColor = color;
        floatDirection = Vector3.up + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
        startTime = Time.time;

        // Make text face camera
        if (Camera.main != null)
        {
            textObject.transform.LookAt(Camera.main.transform);
            textObject.transform.Rotate(0, 180, 0);
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Float upward
        transform.Translate(floatDirection * floatSpeed * Time.deltaTime, Space.World);

        // Fade out over time
        if (textMesh != null)
        {
            float elapsed = Time.time - startTime;
            float fadeProgress = elapsed / lifetime;

            Color color = textMesh.color;
            color.a = Mathf.Lerp(originalColor.a, 0, fadeProgress);
            textMesh.color = color;
        }

        // Always face the camera
        if (Camera.main != null && textMesh != null)
        {
            textMesh.transform.LookAt(Camera.main.transform);
            textMesh.transform.Rotate(0, 180, 0);
        }
    }
}