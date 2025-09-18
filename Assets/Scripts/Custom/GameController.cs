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

    private string _gameDataFile = "gameData.txt";


    private void Start()
    {
        LoadData();

        _bikeDataSource.MinAngle = _gameSettins.MinRawAngle;
        _bikeDataSource.MaxAngle = _gameSettins.MaxRawAngle;
        _bikeDataSource.AngleThreshold = _gameSettins.HandleSensibility;
        _bikeDataSource.SpeedMultiplier = _gameSettins.SpeedMultiplier;

        _uiMainMenuController.UpdateHandleThresholdDisplay(_bikeDataSource.AngleThreshold);
        _uiMainMenuController.UpdateSpeedMultiplierDisplay(_bikeDataSource.SpeedMultiplier);
        _uiMainMenuController.UpdateMinValueDisplay(_bikeDataSource.MinAngle);
        _uiMainMenuController.UpdateMaxValueDisplay(_bikeDataSource.MaxAngle);

        _bikeDataSource.OnMinAngleChanged.AddListener(args => { _gameSettins.MinRawAngle = args; SaveData(); });
        _bikeDataSource.OnMaxAngleChanged.AddListener(args => { _gameSettins.MaxRawAngle = args; SaveData(); });
        _bikeDataSource.OnHandleSensibility.AddListener(args => { _gameSettins.HandleSensibility = args; SaveData(); });
    }

    public void LoadData()
    {
        var path = Path.Combine(Application.persistentDataPath, _gameDataFile);

        if (!File.Exists(path)) return;

        var content = File.ReadAllText(path);

        var gameData = JsonUtility.FromJson<GameData>(content);

        _gameSettins.MinRawAngle = gameData._minHandleValue;
        _gameSettins.MaxRawAngle = gameData._maxHandleValue;
        _gameSettins.HandleSensibility = gameData._handleThreshold;
        _gameSettins.SpeedMultiplier = gameData._velocityMultiplier;


        Debug.Log($"[{GetType()}][LoadData] Configuracoes carregadas!");
    }

    public void SaveData()
    {
        var path = Path.Combine(Application.persistentDataPath, _gameDataFile);

        var gameData = new GameData(
            _gameSettins.MinRawAngle,
            _gameSettins.MaxRawAngle,
            _gameSettins.HandleSensibility,
            _gameSettins.SpeedMultiplier
            );


        var content = JsonUtility.ToJson( gameData );

        File.WriteAllText(path, content );
        
        Debug.Log($"[{GetType()}][SaveData] Configuracoes salvas!");
    }

    public GameSettingsSO GameSettings => _gameSettins;
}
