using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform objectToFollow;
    public float offsetY = 0.5f;
    public float offsetX;
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        
    }
    void Update()
    {
        transform.position = new Vector3(objectToFollow.position.x + offsetX, objectToFollow.position.y + offsetY, -2.2f);
        transform.localRotation = objectToFollow.rotation;
    }
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        
    }
}
