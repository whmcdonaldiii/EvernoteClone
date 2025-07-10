namespace EvernoteClone.Services;

public interface IToastService
{
    event Action<string, ToastType>? OnToastShow;
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInfo(string message);
    void ShowWarning(string message);
}

public enum ToastType
{
    Success,
    Error,
    Info,
    Warning
}

public class ToastService : IToastService
{
    public event Action<string, ToastType>? OnToastShow;

    public void ShowSuccess(string message)
    {
        OnToastShow?.Invoke(message, ToastType.Success);
    }

    public void ShowError(string message)
    {
        OnToastShow?.Invoke(message, ToastType.Error);
    }

    public void ShowInfo(string message)
    {
        OnToastShow?.Invoke(message, ToastType.Info);
    }

    public void ShowWarning(string message)
    {
        OnToastShow?.Invoke(message, ToastType.Warning);
    }
}
