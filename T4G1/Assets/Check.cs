using UnityEngine;
public class Checkpoint : MonoBehaviour
{
    private Checkpoints manager;
    private int myIndex;
    private Color touchedCol;
    private MeshRenderer meshRenderer;
    private bool touched = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Setup(Checkpoints mgr, int index, Color col)
    {
        manager = mgr;
        myIndex = index;
        touchedCol = col;
    }

    void OnTriggerEnter(Collider other)
    {
        if (touched || manager == null) return;

        if (other.CompareTag("Player"))
        {
            if (manager.IsCorrectCheckpoint(myIndex))
            {
                touched = true;
                manager.CheckpointTouched(myIndex);

                if (meshRenderer != null)
                {
                    meshRenderer.material.color = touchedCol;
                }
            }
        }
    }
}
