using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameOrchestrator : MonoBehaviour
{
    private GameObject XRDeviceSimulatorResource;
    private GameObject XRInputActionManagerResource;
    private GameObject XRInteractonManagerResource;
    private GameObject XROriginResource;

    private GameObject BicycleControllersResource;
    private GameObject BicycleResource;
    private GameObject CityGeneratorResource;


    private GameObject XRDeviceSimulatorPrefab;
    private GameObject XRInputActionManagerPrefab;
    private GameObject XRInteractonManagerPrefab;
    private GameObject XROriginPrefab;

    private GameObject BicycleControllersPrefab;
    private GameObject BicyclePrefab;
    private GameObject CityGeneratorPrefab;

    private readonly Vector3 Position = Vector3.zero;
    private const float BicycleScale = 0.69f;
    private const float CityScale = 0.675f;
    private const float CarSpawnChance = 0.6f;
    private const float CameraYOffset = 1.62f; // 1.7m menos aproximadamente 8cm de testa...
    private const float SpeedMultiplier = 0.675f;
    private const float MaxSpeed = Mathf.Infinity;
    private const int ChunkRadius = 3;
    private const bool ShouldAnimate = true;

    private static readonly string[] ControllersNames =
    {
        "SimpleController",             // 0
        "SimpleControllerForwardOnly",  // 1
        "BetterController",             // 2
        "ExternalController",           // 3
        "UDPController",                // 4
    };
    private readonly string ControllerName = ControllersNames[3];

    // Start is called before the first frame update
    void Start()
    {
        LoadResources();
        // pre-ativar o controlador desejado e desativar os nao-desejados
        foreach (Transform child in this.BicycleControllersResource.transform)
        {
            if (child.gameObject.name.Equals(ControllerName))
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        InstantiateResources();

        //this.BicycleResource.SetActive(false);

        SetupPlayerAndMovement();
        SetupBike();
        SetupCityGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
        UnloadResources();
    }

    private void SetupBike()
    {
        this.BicyclePrefab.transform.position = Position;
        this.BicyclePrefab.transform.localScale = Vector3.one * BicycleScale;

        this.BicyclePrefab.GetComponent<VRFollowBicycle>().bicycle = this.XROriginPrefab.transform;
    }

    private void SetupCityGenerator()
    {
        this.CityGeneratorPrefab.transform.position = Position;
        this.CityGeneratorPrefab.transform.localScale = Vector3.one * CityScale;

        CityGenerator cityScript = this.CityGeneratorPrefab.GetComponent<CityGenerator>();
        cityScript.player = this.BicyclePrefab.transform;
        cityScript.chunkRadius = ChunkRadius;
        cityScript.carSpawnChance = CarSpawnChance;
    }

    private void SetupPlayerAndMovement()
    {
        this.XROriginPrefab.transform.position = Position;

        Unity.XR.CoreUtils.XROrigin originScript = this.XROriginPrefab.GetComponent<Unity.XR.CoreUtils.XROrigin>();
        originScript.RequestedTrackingOriginMode = Unity.XR.CoreUtils.XROrigin.TrackingOriginMode.Device;
        originScript.CameraYOffset = CameraYOffset;

        MovePlayerWithMovementSource movePlayerScript = this.XROriginPrefab.GetComponent<MovePlayerWithMovementSource>();

        movePlayerScript.bicycleControllersObject = this.BicycleControllersPrefab;
        movePlayerScript.handlebar = this.BicyclePrefab.transform.Find("HandlebarPivot");
        movePlayerScript.backWheel = this.BicyclePrefab.transform.Find("Wheel b");
        movePlayerScript.frontWheel = this.BicyclePrefab.transform.Find("HandlebarPivot/Bicycle.001/Wheel f");
        movePlayerScript.pedals = this.BicyclePrefab.transform.Find("Pedals");

        movePlayerScript.animate = ShouldAnimate;
        movePlayerScript.speedMultiplier = SpeedMultiplier;
        movePlayerScript.maxSpeed = MaxSpeed;
    }

    private void LoadResources()
    {
        this.BicycleControllersResource = (GameObject) Resources.Load("Prefabs/BicycleControllers");
        this.BicycleResource = (GameObject) Resources.Load("Prefabs/Bicycle");
        this.CityGeneratorResource = (GameObject) Resources.Load("Prefabs/CityGenerator");

        this.XRInputActionManagerResource = (GameObject) Resources.Load("XR Input Action Manager");
        this.XRInteractonManagerResource = (GameObject) Resources.Load("XR Interaction Manager");
        this.XROriginResource = (GameObject) Resources.Load("XR Origin (XR Rig)");
        this.XRDeviceSimulatorResource = (GameObject) Resources.Load("XR Device Simulator");
    }

    private void UnloadResources()
    {

    }

    private void InstantiateResources()
    {
        this.BicyclePrefab = Instantiate(this.BicycleResource, Vector3.zero, Quaternion.identity, this.transform);
        this.BicycleControllersPrefab = Instantiate(this.BicycleControllersResource, Vector3.zero, Quaternion.identity, this.transform);
        this.CityGeneratorPrefab = Instantiate(this.CityGeneratorResource, Vector3.zero, Quaternion.identity, this.transform);
        this.XRInputActionManagerPrefab = Instantiate(this.XRInputActionManagerResource, Vector3.zero, Quaternion.identity, this.transform);
        this.XRInteractonManagerPrefab = Instantiate(this.XRInteractonManagerResource, Vector3.zero, Quaternion.identity, this.transform);
        this.XROriginPrefab = Instantiate(this.XROriginResource, Vector3.zero, Quaternion.identity, this.transform);
        //this.XRDeviceSimulatorPrefab = Instantiate(this.XRDeviceSimulatorResource, Vector3.zero, Quaternion.identity, this.transform);
    }
}
