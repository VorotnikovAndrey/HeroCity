using CameraSystem;
using ResourceSystem;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using Utils;
using Zenject;

public class CheatsManager
{
    private readonly GameResourceManager _gameResourceManager;
    private readonly TimeTicker _timeTicker;
    private MainCanvas _mainCanvas;
    private PostProcessingController _postProcessingController;
    private Camera _camera;

    public CheatsManager()
    {
        _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
        _gameResourceManager = ProjectContext.Instance.Container.Resolve<GameResourceManager>();
        _mainCanvas = ProjectContext.Instance.Container.Resolve<MainCanvas>();
        _postProcessingController = ProjectContext.Instance.Container.Resolve<PostProcessingController>();
        _camera = (ProjectContext.Instance.Container.Resolve<CameraManager>().ActiveCameraView as LocationCamera)?.Camera;

        _timeTicker.OnTick += Update;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _gameResourceManager.AddResourceValue(ResourceType.Coins, 1000);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _gameResourceManager.AddResourceValue(ResourceType.Coins, -1000);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _gameResourceManager.AddResourceValue(ResourceType.Gems, -1000);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _gameResourceManager.AddResourceValue(ResourceType.Gems, 1000);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _mainCanvas.gameObject.SetActive(!_mainCanvas.gameObject.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            _postProcessingController.SwitchState();
        }  
        else if (Input.GetKeyDown(KeyCode.S))
        {
            var data = _camera.transform.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
            data.renderShadows = !data.renderShadows;
        }
    }

    ~CheatsManager()
    {
        _timeTicker.OnTick -= Update;
    }
}