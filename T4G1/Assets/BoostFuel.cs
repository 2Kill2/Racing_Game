using UnityEngine;
public class BoostFuel : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 50, 0);
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().AddFuel();
            this.gameObject.SetActive(false);
        }
    }
}

