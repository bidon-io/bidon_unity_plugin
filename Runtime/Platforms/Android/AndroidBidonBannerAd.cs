#if UNITY_ANDROID || BIDON_DEV_ANDROID
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Bidon.Mediation
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    internal class AndroidBidonBannerAd : IBidonBannerAd, IAndroidBannerListener
    {
        private AndroidJavaObject _bannerAdJavaObject;
        private AndroidJavaObject _activityJavaObject;

        private bool _disposed;

        internal AndroidBidonBannerAd()
        {
            try
            {
                _bannerAdJavaObject = new AndroidJavaObject("org.bidon.sdk.ads.banner.BannerManager");
                _activityJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            }
            catch (Exception e)
            {
                Debug.LogError($"BidonSdk operation is not possible due to incorrect integration: {e.Message}");
                return;
            }

            _bannerAdJavaObject.Call("setBannerListener", new AndroidBannerListener(this));
        }

        ~AndroidBidonBannerAd() => Dispose(false);

        public event EventHandler<BidonAdLoadedEventArgs> OnAdLoaded;
        public event EventHandler<BidonAdLoadFailedEventArgs> OnAdLoadFailed;
        public event EventHandler<BidonAdShownEventArgs> OnAdShown;
        public event EventHandler<BidonAdShowFailedEventArgs> OnAdShowFailed;
        public event EventHandler<BidonAdClickedEventArgs> OnAdClicked;
        public event EventHandler<BidonAdExpiredEventArgs> OnAdExpired;
        public event EventHandler<BidonAdRevenueReceivedEventArgs> OnAdRevenueReceived;

        public void SetFormat(BidonBannerFormat format)
        {
            if (IsDisposed()) return;
            _bannerAdJavaObject?.Call("setBannerFormat", AndroidBidonJavaHelper.GetBannerFormatJavaObject(format));
        }

        public BidonBannerFormat GetFormat()
        {
            if (IsDisposed()) return BidonBannerFormat.Banner;
            return AndroidBidonJavaHelper.GetBannerFormat(_bannerAdJavaObject?.Call<AndroidJavaObject>("getBannerFormat"));
        }

        public void SetPredefinedPosition(BidonBannerPosition position)
        {
            if (IsDisposed()) return;
            _bannerAdJavaObject?.Call("setPosition", AndroidBidonJavaHelper.GetBannerPositionJavaObject(position));
        }

        public void SetCustomPositionAndRotation(Vector2Int positionOffset, int rotationAngle, Vector2 anchorPoint)
        {
            if (IsDisposed()) return;
            _bannerAdJavaObject?.Call("setCustomPosition",
                AndroidBidonJavaHelper.GetPointJavaObject(positionOffset),
                rotationAngle,
                AndroidBidonJavaHelper.GetPointFJavaObject(anchorPoint));
        }

        public void SetCustomPositionAndRotation(Vector2Int positionOffset, int rotationAngle)
        {
            if (IsDisposed()) return;
            SetCustomPositionAndRotation(positionOffset, rotationAngle, new Vector2(0.5f, 0.5f));
        }

        public void Load(double priceFloor)
        {
            if (IsDisposed()) return;
            _bannerAdJavaObject?.Call("loadAd", _activityJavaObject, priceFloor);
        }

        public bool IsReady()
        {
            if (IsDisposed()) return false;
            return _bannerAdJavaObject?.Call<bool>("isReady") ?? false;
        }

        public void Show()
        {
            if (IsDisposed()) return;
            _bannerAdJavaObject?.Call("showAd", _activityJavaObject);
        }

        public bool IsShowing()
        {
            if (IsDisposed()) return false;
            return _bannerAdJavaObject?.Call<bool>("isDisplaying") ?? false;
        }

        public void Hide()
        {
            if (IsDisposed()) return;
            _bannerAdJavaObject?.Call("hideAd", _activityJavaObject);
        }

        public void SetExtraData(string key, object value)
        {
            if (IsDisposed()) return;
            _bannerAdJavaObject?.Call("addExtra", key,
                value == null ? null : AndroidBidonJavaHelper.GetJavaObject(value));
        }

        public IDictionary<string, object> GetExtraData()
        {
            if (IsDisposed()) return new Dictionary<string, object>();
            return AndroidBidonJavaHelper.GetDictionaryFromJavaMap(_bannerAdJavaObject?.Call<AndroidJavaObject>("getExtras"));
        }

        public void NotifyLoss(string winnerDemandId, double ecpm)
        {
            if (IsDisposed()) return;
            _bannerAdJavaObject?.Call("notifyLoss", _activityJavaObject, winnerDemandId, ecpm);
        }

        public void NotifyWin()
        {
            if (IsDisposed()) return;
            _bannerAdJavaObject?.Call("notifyWin");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if(disposing)
            {
                _bannerAdJavaObject?.Call("destroyAd", _activityJavaObject);
                _bannerAdJavaObject?.Dispose();
                _bannerAdJavaObject = null;
                _activityJavaObject?.Dispose();
                _activityJavaObject = null;
            }

            _disposed = true;
        }

        private bool IsDisposed()
        {
            if (!_disposed) return false;
            Debug.LogError($"[BidonPlugin] {GetType().FullName} instance is disposed. Calling any methods on this instance is not allowed.");
            return true;
        }

        #region Callbacks

        public void onAdLoaded(AndroidJavaObject ad)
        {
            OnAdLoaded?.Invoke(this, new BidonAdLoadedEventArgs(AndroidBidonJavaHelper.GetBidonAd(ad)));
        }

        public void onAdLoadFailed(AndroidJavaObject cause)
        {
            OnAdLoadFailed?.Invoke(this, new BidonAdLoadFailedEventArgs(AndroidBidonJavaHelper.GetBidonError(cause)));
        }

        public void onAdShown(AndroidJavaObject ad)
        {
            OnAdShown?.Invoke(this, new BidonAdShownEventArgs(AndroidBidonJavaHelper.GetBidonAd(ad)));
        }

        public void onAdShowFailed(AndroidJavaObject cause)
        {
            OnAdShowFailed?.Invoke(this, new BidonAdShowFailedEventArgs(AndroidBidonJavaHelper.GetBidonError(cause)));
        }

        public void onAdClicked(AndroidJavaObject ad)
        {
            OnAdClicked?.Invoke(this, new BidonAdClickedEventArgs(AndroidBidonJavaHelper.GetBidonAd(ad)));
        }

        public void onAdExpired(AndroidJavaObject ad)
        {
            OnAdExpired?.Invoke(this, new BidonAdExpiredEventArgs(AndroidBidonJavaHelper.GetBidonAd(ad)));
        }

        public void onRevenuePaid(AndroidJavaObject ad, AndroidJavaObject adValue)
        {
            OnAdRevenueReceived?.Invoke(this, new BidonAdRevenueReceivedEventArgs(AndroidBidonJavaHelper.GetBidonAd(ad), AndroidBidonJavaHelper.GetBidonAdValue(adValue)));
        }

        #endregion
    }
}
#endif
