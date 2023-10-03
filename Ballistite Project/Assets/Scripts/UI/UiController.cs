using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class uiController : MonoBehaviour
{
    private bool visible;
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

    public GameObject slowdownEffect;
    public GameObject player;
    public GameObject ReloadIndicator;
    public GameObject CaseShellL;
    public GameObject CaseShellR;

    public GameObject indicator1;
    public GameObject indicator2;
    public GameObject indicator3;

    public GameObject winPanel;

    public CutsceneController cutsceneController;

    // Start is called before the first frame update
    void Start()
    {
        visible = false;
        if (hideToggle != null)
        {
            hideToggle.SetActive(visible);
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

        if (slowdownEffect != null && Time.timeScale < 1)
        {
            slowdownEffect.SetActive(true);
        }
        else if (slowdownEffect != null && Time.timeScale >= 1)
        {
            slowdownEffect.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePauseMenu();

        }

            switch (ammoCountInt)
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



    void togglePauseMenu()
    {
        exitactive = !exitactive;
        quitmenu.SetActive(exitactive);

        Time.timeScale = exitactive ? 0 : 1;
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
        togglePauseMenu();
    }

    public void RestartGame()
    {
        togglePauseMenu();

        if (SceneManager.sceneCountInBuildSettings > 1)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void RestartLevel()
    {
        togglePauseMenu();
        if (SceneManager.sceneCountInBuildSettings > 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
