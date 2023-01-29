using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageScroll : MonoBehaviour
{
    // TODO: Implement image scrolling logic.

    
    [SerializeField] private RawImage _img;
    [SerializeField] private GameObject twiggy;
    [SerializeField] private float _y;

    Rigidbody2D position_;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        position_ = twiggy.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(-position_.velocity.x/50f,_y) * Time.deltaTime, _img.uvRect.size);

    }
}
