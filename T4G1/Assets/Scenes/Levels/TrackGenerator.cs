using UnityEngine;
using UnityEngine.Splines;

public class TrackGenerator : MonoBehaviour
{
    [Header("References")]
    public SplineContainer splineContainer;
    public GameObject trackSegmentPrefab;

    [Header("Default settings")]
    public float spacing = 2f;
    public float TrackWidth = 4f;
    public float maxSpacing;

    [Header("Checkpoint settings")]
    public GameObject checkpointPrefab;
    [Tooltip("If > 0, overrides distance spacing for checkpoints")]
    public int checkpointCount = 0;
    public float checkpointOffset = 25f;


    public void Generate()
    {

        if(splineContainer == null || trackSegmentPrefab == null)
        {
            Debug.LogWarning("Please assign all fields.");
            return;
        }

        Clear();
        GenerateTrack();
        GenerateCheckpoints();

    }
        void GenerateTrack()
        {
            Spline spline = splineContainer.Spline;
            float length = spline.GetLength();

            int SegmentCount = Mathf.Max(2, Mathf.CeilToInt(length / spacing));

            for (int i = 0; i < SegmentCount; i++)
            {
                float distance = i * spacing;
                float t = spline.ConvertIndexUnit(distance, PathIndexUnit.Distance, PathIndexUnit.Normalized);

                Vector3 position = splineContainer.EvaluatePosition(t);
                Vector3 tangent  = splineContainer.EvaluateTangent(t);
            
              Quaternion rotation = Quaternion.LookRotation(tangent, Vector3.up);

                 GameObject segment = Instantiate(
                 trackSegmentPrefab,
                 position,
                 rotation,
                 transform
            );
            
                 Vector3 scale = segment.transform.localScale;
                 scale.x = TrackWidth;
                  segment.transform.localScale = scale;
            }
        }

    void GenerateCheckpoints()
    {
        if (checkpointPrefab == null)
            return;

        Spline spline = splineContainer.Spline;
        float length = spline.GetLength();

        GameObject checkpointParent = new GameObject("Checkpoints");
        checkpointParent.transform.SetParent(transform);

        int count;
        float spacing;

        if (checkpointCount > 0)
        {
            count = checkpointCount;
            spacing = length / checkpointCount;
        }
        else
        {
            spacing = checkpointOffset;
            count = Mathf.FloorToInt(length / spacing);
        }

        for (int i = 0; i < count; i++)
        {
            float distance = i * spacing;
            float t = spline.ConvertIndexUnit(distance, PathIndexUnit.Distance, PathIndexUnit.Normalized);

            Vector3 position = splineContainer.EvaluatePosition(t);
            Vector3 tangent  = splineContainer.EvaluateTangent(t);

            Quaternion rotation = Quaternion.LookRotation(tangent, Vector3.up) * Quaternion.Euler(0f, 90f, 0f);

            GameObject checkpoint = Instantiate(
                checkpointPrefab,
                position,
                rotation,
                checkpointParent.transform
            );

            checkpoint.name = "Checkpoint_" + i;
        }
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
