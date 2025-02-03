#if UNITY_ANDROID || BIDON_DEV

// ReSharper disable CheckNamespace

namespace Bidon.Mediation
{
    internal interface IAndroidRewardedAdListener :
        IAndroidAdListener,
        IAndroidFullscreenAdListener,
        IAndroidAdRevenueListener,
        IAndroidUserRewardListener
    { }
}
#endif
