using Unity.VisualScripting;
using UnityEngine;

public class MovingBGScript : MonoBehaviour
{
    public float scrollSpeed = 2f;

    void Start()
    {
    }

    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * scrollSpeed);
    }


}
