using UnityEngine;
using TMPro;

public class uiController : MonoBehaviour
{
    private bool visisble;
    public GameObject hideToggle;

    public TMP_Text ammoCountText;
    public TMP_Text reloadTimeText;
    
    private int ammoCountInt;
    private float reloadTimeFloat;

    public TMP_Text velocityText;
    public TMP_Text hVelocityText;
    public TMP_Text vVelocityText;

    private float velocityFloat;
    private float hVelocityFloat;
    private float vVelocityFloat;

    public GameObject slowdownEffect;


    // Start is called before the first frame update
    void Start()
    {
        visisble = false;
        if (hideToggle != null)
        {
            hideToggle.SetActive(visisble);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (hideToggle != null && Input.GetButtonDown("Enable Debug Button 1"))
        {
            visisble = !visisble;
            hideToggle.SetActive(visisble);
        }

        if (slowdownEffect != null && Time.timeScale < 1)
        {
            slowdownEffect.SetActive(true);
        }
        else if (slowdownEffect != null && Time.timeScale >= 1)
        {
            slowdownEffect.SetActive(false);
        }

        
        setText();
        
    }

    private void setText()
    {
        ammoCountText.text = "Ammo: " + ammoCountInt.ToString();
        reloadTimeText.text = "Reloading: " + reloadTimeFloat.ToString();

        velocityText.text = "velo: " + velocityFloat.ToString();
        hVelocityText.text = "h velo: " + hVelocityFloat.ToString();
        vVelocityText.text = "v velo: " + vVelocityFloat.ToString();

    }

    public void updateAmmoValues(int ammoC, float realoadT)
    {
        ammoCountInt = ammoC;
        reloadTimeFloat = realoadT;
    }

    public void updateVelocityValues(float v, float hV, float vV)
    {
        velocityFloat = v;
        hVelocityFloat = hV;
        vVelocityFloat = vV;
    }
}
