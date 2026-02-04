using UnityEngine;

[ExecuteAlways]
public class CheckpointGizmo : MonoBehaviour
{
    public Color gizmoColor = Color.green;
    public bool drawWhenNotSelected = true;

    void OnDrawGizmos()
    {
        if (!drawWhenNotSelected)
            return;

        Draw();
    }

    void OnDrawGizmosSelected()
    {
        if (drawWhenNotSelected)
            return;

        Draw();
    }

    void Draw()
    {
        Gizmos.color = gizmoColor;

        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null)
            return;

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(box.center, box.size);

        Gizmos.matrix = oldMatrix;
    }
}
