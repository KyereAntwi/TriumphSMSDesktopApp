using MediatR;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Triumph.SMS.Remote.App.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    protected readonly ISender Sender;
    bool isBusy = false;
    string title = string.Empty;

    public bool IsBusy
    {
        get { return isBusy; }
        set { SetProperty(ref isBusy, value);  }
    }

    public string Title
    {
        get { return title; }
        set { SetProperty(ref title, value); }
    }

    public BaseViewModel(ISender sender)
    {
        Sender = sender;
    }

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null)
    {
        if(EqualityComparer<T>.Default.Equals(backingStore, value))
        {
            return false; 
        }

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
