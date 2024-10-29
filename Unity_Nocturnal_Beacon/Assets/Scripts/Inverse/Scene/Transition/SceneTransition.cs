using System.Collections;

using UnityEngine;

namespace Minimalist.Scene.Transition
{
    public abstract class SceneTransition : MonoBehaviour
    {
        public abstract IEnumerator AnimateTransitionIn();
        public abstract IEnumerator AnimateTransitionOut();
    }
}
