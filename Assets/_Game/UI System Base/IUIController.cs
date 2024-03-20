namespace UISystem
{
    public interface IUIController
    {
        float DelayQuitBack { get; set; }
        bool Initialized { get; set; }
        int QuitCountBack { get; set; }
        bool QuitInMenu { get; set; }
        GameUI StartScreen { get; set; }
        string UIPath { get; set; }

        void Start();
    }
}