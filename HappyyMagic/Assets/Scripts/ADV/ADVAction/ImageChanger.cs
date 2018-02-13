using UnityEngine;
using UnityEngine.UI;

namespace ADV.Command
{
    public class ImageChanger : IResourceUnloadable, IAdvActionable
    {
        Image image;
        Sprite sprite;

        public ImageChanger(Image image, string filePath)
        {
            this.image = image;
            sprite = Resources.Load<Sprite>(filePath);
        }

        public void Action()
        {
            image.sprite = sprite;
        }

        public void Unload()
        {
            if(sprite == null) { return; }

            Resources.UnloadAsset(sprite);
            sprite = null;
        }
    }
}