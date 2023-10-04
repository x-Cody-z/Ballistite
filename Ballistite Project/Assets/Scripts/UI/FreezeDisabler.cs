using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreezeDisabler : MonoBehaviour
{
    private static FreezeDisabler freezeInstance;
    public bool masterFreezeEnabled;
    private bool currentFreezeState;

    private ChargeTutorial chargeTutScript;
    private ChargeTutorial airShotChargeTutScript;
    private AirShotTutorial airShotTutScript;
    private DestructionTutorial destructionTutScript;

    private void Awake()
    {
        //lets the testing object persist across scenes
        DontDestroyOnLoad(gameObject);

        //stops duplicate testing objects being created when scene gets reloaded
        if (freezeInstance == null)
            freezeInstance = this;
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        masterFreezeEnabled = true;
        getTutObjects();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        chargeTutScript = null;
        airShotChargeTutScript = null;
        airShotTutScript = null;
        destructionTutScript = null;

        getTutObjects();
        //setFreezeStates(masterFreezeEnabled);
    }

    // Update is called once per frame
    void Update()
    {
        if (masterFreezeEnabled != currentFreezeState)
            setFreezeStates(masterFreezeEnabled);
        else if (chargeTutScript != null)
            if (chargeTutScript.freezeEnabled != masterFreezeEnabled)
                setFreezeStates(masterFreezeEnabled);
    }

    private void setFreezeStates(bool b)
    {
        currentFreezeState = b;
        if (chargeTutScript != null)
            chargeTutScript.freezeEnabled = b;
        if (airShotChargeTutScript != null)
            airShotChargeTutScript.freezeEnabled = b;
        if (airShotTutScript != null)
            airShotTutScript.freezeEnabled = b;
        if (destructionTutScript != null)
            destructionTutScript.freezeEnabled = b;
    }

    private void getTutObjects()
    {
        if (GameObject.Find("ChargeTutorial"))
            chargeTutScript = GameObject.Find("ChargeTutorial").GetComponent<ChargeTutorial>();

        if (GameObject.Find("AirShotCharge"))
            airShotChargeTutScript = GameObject.Find("AirShotCharge").GetComponent<ChargeTutorial>();

        if (GameObject.Find("AirShotComponent"))
            airShotTutScript = GameObject.Find("AirShotComponent").GetComponent<AirShotTutorial>();

        if (GameObject.Find("DestructionTutorial"))
            destructionTutScript = GameObject.Find("DestructionTutorial").GetComponent<DestructionTutorial>();
    }
}
