using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] private Volume _volume;

    public Volume Volume => _volume;

    public void Awake()
    {
        ProjectContext.Instance.Container.BindInstances(this);
    }

    public void OnDestroy()
    {
        ProjectContext.Instance.Container.Unbind<PostProcessingController>();
    }

    public void SwitchState()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
