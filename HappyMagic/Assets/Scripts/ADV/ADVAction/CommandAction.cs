
namespace ADV.Command
{
    public class CommandAction : IAdvActionable
    {
        System.Action action;

        public CommandAction(System.Action action)
        {
            this.action = action;
        }

        public void Action()
        {
            action();
        }
    }
}