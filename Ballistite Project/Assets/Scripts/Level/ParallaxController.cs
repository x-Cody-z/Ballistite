using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;
    Vector2 distance;

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthestBack;

    [Range(0f, 2f)]
    public float parallaxSpeed;

    [Range(0f, 30f)]
    public float backgroundHeight;

    public bool ScrollerMode;

    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }
        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }

        }

        for (int i = 0; i < backCount; i++)
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }


    private void LateUpdate() //TODO: clean this up, im doing unnecessary math
    {
        distance = cam.position - camStartPos;

        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speedX = backSpeed[i] * parallaxSpeed;
            float speedY = speedX / 0.3f;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance.x * speedX, 0));
            backgrounds[i].transform.position = new Vector3(transform.position.x, 0 - ((distance.y * speedY) - backgroundHeight), 6);
        }
    }
}