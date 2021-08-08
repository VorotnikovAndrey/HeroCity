namespace Defong.GameStageSystem
{
    public interface IStage
    { 
        void Initialize(object data);
        void DeInitialize();
        void Show();
        void Hide();
    }
}