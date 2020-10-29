using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction currrentAction;
        
        public void StartAction(IAction action)
        {
            if (currrentAction == action) return;
            if (currrentAction != null)
            {
                currrentAction.Cancel();
            }
            currrentAction = action;
        }

        public void CancenCurrentAction()
        {
            StartAction(null);
        }
    }
}