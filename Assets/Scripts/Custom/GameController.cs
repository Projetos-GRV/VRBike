using System.IO;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [Header("Component to load Data")]
    [SerializeField] private UIMainMenuController _uiMainMenuController;
    [SerializeField] private CustomBikeMovimentSource _bikeDataSource;

    [SerializeField] private GameSettingsSO _gameSettins;

    private string _gameDataFile = "gameData.json";
    private string _gameDataPath;

    private void Awake()
    {
        _gameDataPath = Path.Combine(Application.persistentDataPath, _gameDataFile);
    }

    private void Start()
    {
        LoadData();

        _bikeDataSource.MinAngle = _gameSettins.MinRawAngle;
        _bikeDataSource.MaxAngle = _gameSettins.MaxRawAngle;
        _bikeDataSource.AngleThreshold = _gameSettins.HandleSensibility;
        _bikeDataSource.BaseSpeedMultiplier = _gameSettins.BaseSpeedMultiplier;

        _uiMainMenuController.UpdateHandleThresholdDisplay(_bikeDataSource.AngleThreshold);
        _uiMainMenuController.UpdateSpeedMultiplierDisplay(_bikeDataSource.SpeedMultiplier);
        _uiMainMenuController.UpdateMinValueDisplay(_bikeDataSource.MinAngle);
        _uiMainMenuController.UpdateMaxValueDisplay(_bikeDataSource.MaxAngle);

        _bikeDataSource.OnMinAngleChanged.AddListener(args => { _gameSettins.MinRawAngle = args; SaveData(); });
        _bikeDataSource.OnMaxAngleChanged.AddListener(args => { _gameSettins.MaxRawAngle = args; SaveData(); });
        _bikeDataSource.OnHandleSensibility.AddListener(args => { _gameSettins.HandleSensibility = args; SaveData(); });
        _bikeDataSource.OnBaseSpeedMultiplierChanged.AddListener(args => { _gameSettins.BaseSpeedMultiplier = args; SaveData(); });
    }

    public void LoadData()
    {
        if (!File.Exists(_gameDataPath)) return;

        var content = File.ReadAllText(_gameDataPath);

        var gameData = JsonUtility.FromJson<GameData>(content);

        _gameSettins.MinRawAngle = gameData._minHandleValue;
        _gameSettins.MaxRawAngle = gameData._maxHandleValue;
        _gameSettins.HandleSensibility = gameData._handleThreshold;
        _gameSettins.BaseSpeedMultiplier = gameData._velocityMultiplier;


        Debug.Log($"[{GetType()}][LoadData] Configuracoes carregadas!");
    }

    public void SaveData()
    {
        var gameData = new GameData(
            _gameSettins.MinRawAngle,
            _gameSettins.MaxRawAngle,
            _gameSettins.HandleSensibility,
            _gameSettins.BaseSpeedMultiplier
            );


        var content = JsonUtility.ToJson( gameData );

        File.WriteAllText(_gameDataPath, content );
        
        Debug.Log($"[{GetType()}][SaveData] Configuracoes salvas!");
    }

    public GameSettingsSO GameSettings => _gameSettins;
}
