using UnityEngine;

public class LocalAnimator
{
    private readonly Animator _animator;

    public LocalAnimator(Animator animator)
    {
        _animator = animator;
    }

    public bool GetBoolAnimator(string name)
    {
        return _animator.GetBool(name);
    }

    public void SetBoolAnimator(string name, bool value)
    {
        _animator.SetBool(name, value);
    }
}