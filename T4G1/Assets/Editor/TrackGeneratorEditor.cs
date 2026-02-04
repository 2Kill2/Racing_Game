using UnityEngine;
using UnityEditor;
using UnityEngine.Splines;

[CustomEditor(typeof(TrackGenerator))]
public class TrackGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TrackGenerator generator = (TrackGenerator)target;

        EditorGUILayout.LabelField("Track Generator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // ---------- REFERENCES ----------
        generator.splineContainer = (SplineContainer)EditorGUILayout.ObjectField(
            "Spline Container",
            generator.splineContainer,
            typeof(SplineContainer),
            true
        );

        generator.trackSegmentPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Track Segment Prefab",
            generator.trackSegmentPrefab,
            typeof(GameObject),
            false
        );

        generator.checkpointPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Checkpoint Prefab",
            generator.checkpointPrefab,
            typeof(GameObject),
            false
        );

        EditorGUILayout.Space(10);

        // ---------- TRACK SETTINGS ----------
        EditorGUILayout.LabelField("Track Settings", EditorStyles.boldLabel);

        generator.spacing = EditorGUILayout.Slider(
            "Spacing",
            generator.spacing,
            0.1f,
            10f
        );

        generator.TrackWidth = EditorGUILayout.Slider(
            "Track Width",
            generator.TrackWidth,
            0.1f,
            20f
        );

        EditorGUILayout.Space(10);

        // ---------- CHECKPOINT SETTINGS ----------
        EditorGUILayout.LabelField("Checkpoint Settings", EditorStyles.boldLabel);

        generator.checkpointCount = EditorGUILayout.IntSlider(
            "Checkpoint Count",
            generator.checkpointCount,
            0,
            50
        );

        generator.checkpointOffset = EditorGUILayout.Slider(
            "Checkpoint Spacing",
            generator.checkpointOffset,
            1f,
            100f
        );

        EditorGUILayout.HelpBox(
            "If Checkpoint Count > 0, checkpoints are evenly distributed.\n" +
            "Otherwise, spacing is used.",
            MessageType.Info
        );

        EditorGUILayout.Space(15);

        // ---------- VALIDATION ----------
        if (generator.splineContainer == null)
        {
            EditorGUILayout.HelpBox("Spline Container cannot be null.", MessageType.Warning);
            return;
        }

        if (generator.trackSegmentPrefab == null)
        {
            EditorGUILayout.HelpBox("Track Segment Prefab cannot be null.", MessageType.Warning);
            return;
        }

        if (generator.checkpointPrefab == null)
        {
            EditorGUILayout.HelpBox("Checkpoint Prefab cannot be null.", MessageType.Warning);
            return;
        }

        // ---------- BUTTONS ----------
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Track", GUILayout.Height(40)))
        {
            Undo.RegisterFullObjectHierarchyUndo(generator.gameObject, "Generate Track");
            generator.Generate();
            EditorUtility.SetDirty(generator);
        }

        if (GUILayout.Button("Clear Track", GUILayout.Height(40)))
        {
            Undo.RegisterFullObjectHierarchyUndo(generator.gameObject, "Clear Track");
            generator.Clear();
            EditorUtility.SetDirty(generator);
        }

        EditorGUILayout.EndHorizontal();
    }
}
