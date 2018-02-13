using UnityEngine.UI;

namespace ADV.Command
{
    public class SetVisible : IAdvActionable
    {
        Image image;
        bool enabled = false;

        public SetVisible(Image image, bool enabled)
        {
            this.image = image;
            this.enabled = enabled;
        }

        public void Action()
        {
            image.enabled = enabled;
        }
    }
}