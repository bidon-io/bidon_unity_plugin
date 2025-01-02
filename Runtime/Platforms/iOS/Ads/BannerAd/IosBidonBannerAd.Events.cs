#if UNITY_IOS || BIDON_DEV

// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using AOT;

namespace Bidon.Mediation
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal partial class IosBidonBannerAd
    {
        private delegate void AdLoadedCallback(IntPtr iosBidonAdPtr, IntPtr iosBidonAuctionInfoPtr);
        private delegate void AdLoadFailedCallback(IntPtr iosBidonAuctionInfoPtr, int cause);
        private delegate void AdShownCallback(IntPtr iosBidonAdPtr);
        private delegate void AdShowFailedCallback(int cause);
        private delegate void AdClickedCallback(IntPtr iosBidonAdPtr);
        private delegate void AdExpiredCallback(IntPtr iosBidonAdPtr);
        private delegate void AdRevenueReceivedCallback(IntPtr iosBidonAdPtr, IntPtr iosBidonAdRevenuePtr);

        public event EventHandler<BidonAdLoadedEventArgs> OnAdLoaded;
        public event EventHandler<BidonAdLoadFailedEventArgs> OnAdLoadFailed;
        public event EventHandler<BidonAdShownEventArgs> OnAdShown;
        public event EventHandler<BidonAdShowFailedEventArgs> OnAdShowFailed;
        public event EventHandler<BidonAdClickedEventArgs> OnAdClicked;
        public event EventHandler<BidonAdExpiredEventArgs> OnAdExpired;
        public event EventHandler<BidonAdRevenueReceivedEventArgs> OnAdRevenueReceived;

        [MonoPInvokeCallback(typeof(AdLoadedCallback))]
        private static void AdLoaded(IntPtr iosBidonAdPtr, IntPtr iosBidonAuctionInfoPtr)
        {
            var ad = iosBidonAdPtr.ToBidonAd();
            var auctionInfo = iosBidonAuctionInfoPtr.ToBidonAuctionInfo();

            SyncContextHelper.Post(state => _instance?.OnAdLoaded?.Invoke(_instance, new BidonAdLoadedEventArgs(ad, auctionInfo)));
        }

        [MonoPInvokeCallback(typeof(AdLoadFailedCallback))]
        private static void AdLoadFailed(IntPtr iosBidonAuctionInfoPtr, int cause)
        {
            var auctionInfo = iosBidonAuctionInfoPtr.ToBidonAuctionInfo();
            var error = cause.ToEnum(BidonError.Unspecified);

            SyncContextHelper.Post(state => _instance?.OnAdLoadFailed?.Invoke(_instance, new BidonAdLoadFailedEventArgs(auctionInfo, error)));
        }

        [MonoPInvokeCallback(typeof(AdShownCallback))]
        private static void AdShown(IntPtr iosBidonAdPtr)
        {
            var ad = iosBidonAdPtr.ToBidonAd();

            SyncContextHelper.Post(state => _instance?.OnAdShown?.Invoke(_instance, new BidonAdShownEventArgs(ad)));
        }

        [MonoPInvokeCallback(typeof(AdShowFailedCallback))]
        private static void AdShowFailed(int cause)
        {
            var error = cause.ToEnum(BidonError.Unspecified);

            SyncContextHelper.Post(state => _instance?.OnAdShowFailed?.Invoke(_instance, new BidonAdShowFailedEventArgs(error)));
        }

        [MonoPInvokeCallback(typeof(AdClickedCallback))]
        private static void AdClicked(IntPtr iosBidonAdPtr)
        {
            var ad = iosBidonAdPtr.ToBidonAd();

            SyncContextHelper.Post(state => _instance?.OnAdClicked?.Invoke(_instance, new BidonAdClickedEventArgs(ad)));
        }

        [MonoPInvokeCallback(typeof(AdExpiredCallback))]
        private static void AdExpired(IntPtr iosBidonAdPtr)
        {
            var ad = iosBidonAdPtr.ToBidonAd();

            SyncContextHelper.Post(state => _instance?.OnAdExpired?.Invoke(_instance, new BidonAdExpiredEventArgs(ad)));
        }

        [MonoPInvokeCallback(typeof(AdRevenueReceivedCallback))]
        private static void AdRevenueReceived(IntPtr iosBidonAdPtr, IntPtr iosBidonAdRevenuePtr)
        {
            var ad = iosBidonAdPtr.ToBidonAd();
            var adValue = iosBidonAdRevenuePtr.ToBidonAdValue();

            SyncContextHelper.Post(state => _instance?.OnAdRevenueReceived?.Invoke(_instance, new BidonAdRevenueReceivedEventArgs(ad, adValue)));
        }
    }
}
#endif
