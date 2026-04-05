using UnityEngine;

public class CollectibleCube : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            GameManager.Instance.AddScore(value);
            Destroy(gameObject);
        }
    }
}