using Minimalist.Audio;
using Minimalist.Audio.Sound;

using System.Collections;

using UnityEngine;

namespace Minimalist.Scene.Transition.Effect
{
    public class Clapper : SceneTransition
    {
        [SerializeField] private Animator _transitionAnimator;

        private void Awake()
        {
            if (_transitionAnimator == null)
            {
                _transitionAnimator = GetComponent<Animator>();
            }
        }

        public override IEnumerator AnimateTransitionIn()
        {
            _transitionAnimator.ResetTrigger("fade_in");
            _transitionAnimator.SetTrigger("start");

            yield return new WaitUntil(() => _transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle_In_Screen"));
        }

        public override IEnumerator AnimateTransitionOut()
        {
            _transitionAnimator.SetTrigger("end");

            //while (!_transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Clap"))
            //{
            //    yield return null;
            //}

            //while (!_transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle_In_Screen"))
            //{
            //    yield return null;
            //}

            //_transitionAnimator.SetTrigger("end");

            yield return new WaitUntil(() => _transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        }

        public void PlayClap()
        {
            AudioManager.PlaySFX(SoundType.Transition_Clap);
        }

        private static void D(string message)
        {
            Debug.Log("<<Clapper>> " + message);
        }
    }
}
