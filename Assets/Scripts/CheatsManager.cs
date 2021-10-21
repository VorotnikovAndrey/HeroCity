using Gameplay;
using ResourceSystem;
using UnityEngine;
using UserSystem;
using Utils;
using Zenject;

public class CheatsManager
{
    private readonly GameResourceManager _gameResourceManager;
    private readonly TimeTicker _timeTicker;

    public CheatsManager()
    {
        _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
        _gameResourceManager = ProjectContext.Instance.Container.Resolve<GameResourceManager>();

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
    }

    ~CheatsManager()
    {
        _timeTicker.OnTick -= Update;
    }
}