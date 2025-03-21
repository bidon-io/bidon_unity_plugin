#if UNITY_ANDROID || BIDON_DEV

// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace Bidon.Mediation
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal interface IAndroidInInitializationListener
    {
        void onFinished();
    }
}
#endif
