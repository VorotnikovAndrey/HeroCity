namespace InputSystem
{
    public interface IInputSystem
    {
        public bool IsInputLocked { get; }

        void Update();
    }
}