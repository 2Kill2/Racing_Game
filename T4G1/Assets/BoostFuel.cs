using UnityEngine;
public class BoostFuel : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().AddFuel();
            this.gameObject.SetActive(false);
        }
    }
}

