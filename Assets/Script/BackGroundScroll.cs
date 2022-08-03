using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    Material material;
    Vector2 offset;

    public float xVelocity, yVelocity;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    public void Start()
    {
        offset = new Vector2(xVelocity, yVelocity);
    }

    public void Update()
    {
        material.mainTextureOffset += offset * Time.deltaTime;
    }


}
