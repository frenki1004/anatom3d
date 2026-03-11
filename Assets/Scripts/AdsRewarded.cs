using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements; //Assuming you imported the Advertisements from the "Package Manager"
using UnityEngine.UI;
public class AdsRewarded : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string gameId = "Your-Google-ID";
    public string mySurfacingId = "Rewarded_Android";
    public bool testMode = true; //Leave this as True UNTIL you release your game!!!

    public Button[] buttons;
    public Animation[] loadingAnimations;
    private bool adLoaded;

    void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.interactable = false;
        }
        foreach (Animation animation in loadingAnimations)
        {
            animation.Play();
        }
        // Initialize Ads SDK. Load is triggered after successful initialization callback.
        Advertisement.Initialize(gameId, testMode, this);
    }

    public void ShowRewardedVideo() //Shows The add when this method is called - 
    {   // Check if UnityAds ready before calling Show method:
        if (adLoaded)
        {
            Advertisement.Show(mySurfacingId, this);
        }
        else
        {
            PopUpManagement.instance.Show("The video is not ready at the moment. Please try again later.", 3);
        }
    }

    public void OnInitializationComplete()
    {
        Advertisement.Load(mySurfacingId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        PopUpManagement.instance.Show("Something went wrong. Please try again later.", 3);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == mySurfacingId)
        {
            adLoaded = true;
            foreach (Button btn in buttons)
            {
                btn.interactable = true;
            }
            foreach (Animation animation in loadingAnimations)
            {
                animation.gameObject.SetActive(false);
            }
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        adLoaded = false;
        PopUpManagement.instance.Show("The video is not ready at the moment. Please try again later.", 3);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        adLoaded = false;
        PopUpManagement.instance.Show("Something went wrong. Please try again later.", 3);
        Advertisement.Load(mySurfacingId, this);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        adLoaded = false;

        if (placementId == mySurfacingId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            PopUpManagement.instance.Show("Thank you!", 3);
        }

        Advertisement.Load(mySurfacingId, this);
    }
}