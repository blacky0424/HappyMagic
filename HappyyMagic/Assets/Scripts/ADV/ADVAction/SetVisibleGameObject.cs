using UnityEngine;

namespace ADV.Command
{
    public class SetVisibleGameObject : IAdvActionable
    {
        GameObject target;
        bool enabled;

        public SetVisibleGameObject(GameObject target, bool enabled)
        {
            this.target = target;
            this.enabled = enabled;
        }

        public void Action()
        {
            target.SetActive(enabled);
        }
    }

}
