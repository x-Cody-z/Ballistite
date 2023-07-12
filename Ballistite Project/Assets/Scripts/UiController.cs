using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

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
    public GameObject quitmenu;

    private float velocityFloat;
    private float hVelocityFloat;
    private float vVelocityFloat;

    private GameObject player;
    public GameObject ReloadIndicator;
    public GameObject CaseShellL;
    public GameObject CaseShellR;

    public GameObject indicator1;
    public GameObject indicator2;
    public GameObject indicator3;

    public GameObject winPanel;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        visisble = false;
        if (hideToggle != null)
        {
            hideToggle.SetActive(visisble);
        }
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    private bool exitactive = false;
    // Update is called once per frame
    void Update()
    {
        ReloadIndicator.transform.position = player.transform.position;
        /*
        if (hideToggle != null && Input.GetButtonDown("Enable Debug Button 1"))
        {
            visisble = !visisble;
            hideToggle.SetActive(visisble);
        }
        */
        
        setText();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitactive = !exitactive;
            quitmenu.SetActive(exitactive);
            
            if (exitactive)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;

        }

        switch(ammoCountInt)
        {
            case 0:
                CaseShellL.SetActive(false);
                CaseShellR.SetActive(false);
                break;
            case 1:
                CaseShellL.SetActive(false);
                CaseShellR.SetActive(true);
                break;
            case 2:
                CaseShellL.SetActive(true);
                CaseShellR.SetActive(true);
                break;
        }
        
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

    public void updateChargeValues(bool ind1, bool ind2, bool ind3)
    {
        indicator1.SetActive(ind1);
        indicator2.SetActive(ind1);
        indicator3.SetActive(ind1);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void CloseMenu()
    {
        exitactive = !exitactive;
        quitmenu.SetActive(exitactive);
    }
}
