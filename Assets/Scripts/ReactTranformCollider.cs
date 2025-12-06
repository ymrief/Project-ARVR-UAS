using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class RectTransformCollider : MonoBehaviour
{
    public float depth = 0.01f;

    void Update() { Sync(); }
    void OnValidate() { Sync(); }

    void Sync()
    {
        var rt = GetComponent<RectTransform>();
        if (rt == null) return;

        var bc = GetComponent<BoxCollider>();
        if (bc == null) bc = gameObject.AddComponent<BoxCollider>();

        Vector3 worldScale = transform.lossyScale;
        Vector2 size = rt.rect.size;

        float width = Mathf.Abs(size.x * worldScale.x);
        float height = Mathf.Abs(size.y * worldScale.y);

        if (width < 0.001f) width = 0.1f;
        if (height < 0.001f) height = 0.1f;

        bc.size = new Vector3(width, height, depth);
        bc.center = Vector3.zero;
        bc.isTrigger = false;
    }
}
