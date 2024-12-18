public interface IInteractable
{
    public void OnInteract();
    public string GetInteractPrompt();
}

public interface IHideable
{
    public void OnHide();
    public void OnExit();
}
