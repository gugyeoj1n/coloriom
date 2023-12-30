using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdvertisement : MonoBehaviour
{
    public BannerView banner;
    private string ad_unit_id;
    public string admop_unit_id_ios;
    public string admop_unit_id_android;
    
    void Start()
    {
        MobileAds.Initialize((InitializationStatus status) =>
        {
            LoadAd();
        });
        
#if UNITY_ANDROID
        ad_unit_id = admop_unit_id_android;
#elif UNITY_IOS
        ad_unit_id = admop_unit_id_ios;
#endif
    }

    private void LoadAd()
    {
        if (banner == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest.Builder().Build();
        banner.LoadAd(adRequest);
    }

    private void CreateBannerView()
    {
        // if(banner != null)
        banner = new BannerView(ad_unit_id, AdSize.Banner, AdPosition.Bottom);
    }
}