namespace WebApp.Layout;

public partial class NavMenu
{
    private bool isOpen;

    private void ToggleMenu()
    {
        isOpen = !isOpen;
    }
}