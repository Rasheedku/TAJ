using Microsoft.UI.Xaml.Controls;

namespace Taj.BuildingCostApp.Services;

public class NavigationService
{
    public Frame? RootFrame { get; private set; }

    public void Initialize(Frame frame)
    {
        RootFrame = frame;
    }

    public void Navigate<T>() where T : class
    {
        RootFrame?.Navigate(typeof(T));
    }
}
