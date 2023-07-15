namespace HowTo.Entities.Interactive;

public class InteractiveResponse
{
    public InteractiveResponse(InteractivePublic interactive, LastInteractivePublic lastInteractive)
    {
        Interactive = interactive;
    }
    public InteractivePublic Interactive { get; set; }
}