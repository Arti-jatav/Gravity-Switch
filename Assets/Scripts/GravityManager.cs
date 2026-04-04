using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance { get; private set; }

    [Header("Gravity Settings")]
    [SerializeField] private Vector3 _gravity = new Vector3(0, -9.81f, 0);

    public Vector3 Gravity => _gravity;

    private void Awake()
    {
        // Singleton pattern for easy access
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetGravityDirection(Vector3 newDirection)
    {
        _gravity = newDirection.normalized * 9.81f;
    }
}