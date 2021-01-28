using UnityEngine;
using UnityEditor;

public class AnimcationController
{

    Animator _animator { get; }

    public AnimcationController(Animator animator)
    {
        _animator = animator;
    }

    public void PerformAnimation(string animhash)
    {
        if (!_animator.enabled)
            _animator.enabled = true;

        _animator.Play(animhash);
    }

    public void StopAllanimations()
    {
        _animator.enabled = false;
    }


    public void SetAnimationState(string animation, bool value)
    {

    }
}