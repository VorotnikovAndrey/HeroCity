using Defong.Utils;
using Gameplay;
using UnityEngine;
using UserSystem;
using Zenject;

public class CheatsManager
{
    private readonly UserManager _userManager;
    private readonly TimeTicker _timeTicker;

    public CheatsManager()
    {
        _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
        _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();

        _timeTicker.OnTick += Update;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _userManager.CurrentUser.AddResourceValue(ResourceType.Coins, 1000);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _userManager.CurrentUser.AddResourceValue(ResourceType.Coins, -1000);
        }
    }

    ~CheatsManager()
    {
        _timeTicker.OnTick -= Update;
    }
}