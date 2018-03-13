
namespace ADV.Command
{
    public class ADVClose : IAdvActionable
    {
        ADVSystem system;

        public ADVClose(ADVSystem system)
        {
            this.system = system;
        }

        public void Action()
        {
            system.ADVClose();
        }
    }
}