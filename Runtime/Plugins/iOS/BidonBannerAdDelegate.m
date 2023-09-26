//
//  BidonBannerAdDelegate.m
//  Bidon Unity Plugin
//
//  Created by Dmitrii Feshchenko on 15/09/2023.
//

#import "BidonBannerAdDelegate.h"
#import "BidonHelperMethods.h"

CFBDNUnityPluginBannerAdDelegateRef BDNUnityPluginBannerAdDelegateCreate(DidLoad didLoadCallback,
                                                                         DidFailToLoad didFailToLoadCallback,
                                                                         DidRecordImpression didRecordImpressionCallback,
                                                                         DidFailToPresent didFailToPresentCallback,
                                                                         DidClick didClickCallback,
                                                                         DidExpire didExpireCallback,
                                                                         DidPayRevenue didPayRevenueCallback) {
    BDNUnityPluginBannerAdDelegate* delegate = [BDNUnityPluginBannerAdDelegate new];
    delegate.bannerDidLoadCallback = didLoadCallback;
    delegate.bannerDidFailToLoadCallback = didFailToLoadCallback;
    delegate.bannerDidRecordImpressionCallback = didRecordImpressionCallback;
    delegate.bannerDidFailToPresentCallback = didFailToPresentCallback;
    delegate.bannerDidClickCallback = didClickCallback;
    delegate.bannerDidExpireCallback = didExpireCallback;
    delegate.bannerDidPayRevenueCallback = didPayRevenueCallback;

    return (__bridge_retained CFBDNUnityPluginBannerAdDelegateRef)delegate;
}

void BDNUnityPluginBannerAdDelegateDestroy(CFBDNUnityPluginBannerAdDelegateRef delegatePtr) {
    if (!delegatePtr) return;

    BDNUnityPluginBannerAdDelegate* delegate = (__bridge_transfer BDNUnityPluginBannerAdDelegate*)delegatePtr;

    delegate.bannerDidLoadCallback = nil;
    delegate.bannerDidFailToLoadCallback = nil;
    delegate.bannerDidRecordImpressionCallback = nil;
    delegate.bannerDidFailToPresentCallback = nil;
    delegate.bannerDidClickCallback = nil;
    delegate.bannerDidExpireCallback = nil;
    delegate.bannerDidPayRevenueCallback = nil;
}

@implementation BDNUnityPluginBannerAdDelegate

- (void)adObject:(id<BDNAdObject> _Nonnull)adObject didLoadAd:(id<BDNAd> _Nonnull)ad {
    if (!self.bannerDidLoadCallback) return;

    BDNUnityPluginAd unityAd = BDNUnityPluginHelperGetAd(ad);
    self.bannerDidLoadCallback(&unityAd);
}

- (void)adObject:(id<BDNAdObject> _Nonnull)adObject didFailToLoadAd:(NSError * _Nonnull)error {
    if (!self.bannerDidFailToLoadCallback) return;

    self.bannerDidFailToLoadCallback((int)error.code);
}

- (void)adObject:(id<BDNAdObject> _Nonnull)adObject didRecordImpression:(id<BDNAd> _Nonnull)ad {
    if (!self.bannerDidRecordImpressionCallback) return;

    BDNUnityPluginAd unityAd = BDNUnityPluginHelperGetAd(ad);
    self.bannerDidRecordImpressionCallback(&unityAd);
}

- (void)adObject:(id<BDNAdObject> _Nonnull)adObject didFailToPresentAd:(NSError * _Nonnull)error {
    if (!self.bannerDidFailToPresentCallback) return;

    self.bannerDidFailToPresentCallback((int)error.code);
}

- (void)adObject:(id<BDNAdObject> _Nonnull)adObject didRecordClick:(id<BDNAd> _Nonnull)ad {
    if (!self.bannerDidClickCallback) return;

    BDNUnityPluginAd unityAd = BDNUnityPluginHelperGetAd(ad);
    self.bannerDidClickCallback(&unityAd);
}

- (void)adObject:(id<BDNAdObject>)adObject didExpireAd:(id<BDNAd>)ad {
    if (!self.bannerDidExpireCallback) return;

    BDNUnityPluginAd unityAd = BDNUnityPluginHelperGetAd(ad);
    self.bannerDidExpireCallback(&unityAd);
}

- (void)adObject:(id<BDNAdObject> _Nonnull)adObject didPay:(id<BDNAdRevenue> _Nonnull)revenue ad:(id<BDNAd> _Nonnull)ad {
    if (!self.bannerDidPayRevenueCallback) return;

    BDNUnityPluginAd unityAd = BDNUnityPluginHelperGetAd(ad);
    BDNUnityPluginAdRevenue unityAdRevenue = BDNUnityPluginHelperGetAdRevenue(revenue);
    self.bannerDidPayRevenueCallback(&unityAd, &unityAdRevenue);
}

@end
