using UnityEngine;

public class WheelSpin : MonoBehaviour
{
    [SerializeField] private Transform[] wheels;
    [SerializeField] private float rotationSpeed = 360f;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController == null) return;
        float speed = playerController.CurrentSpeed();
        float rotation = speed * rotationSpeed * Time.deltaTime;

        foreach (Transform wheel in wheels)
        {
            if (wheel != null)
            {
                wheel.Rotate(0, 0, rotation);
            }
        }
    }
}
