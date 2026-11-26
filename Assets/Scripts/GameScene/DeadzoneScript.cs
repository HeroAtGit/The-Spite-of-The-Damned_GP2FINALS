using UnityEngine;

public class DeadzoneScript : MonoBehaviour
{
    [SerializeField] private float deadZone = -14;
    void Start()
    {
        
    }
    void Update()
    {
        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
