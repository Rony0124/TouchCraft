using UnityEngine;

namespace GameplayAbilitySystem
{
    public enum CastAbilityResult
    {
        Success,
        Fail,
        NoTarget,
        AlreadyExecuting,
        NotGranted,
        NoCost,
        Cooldown,
        Block,
        AlreadyCastingSameAbilityTarget,
    }
}
