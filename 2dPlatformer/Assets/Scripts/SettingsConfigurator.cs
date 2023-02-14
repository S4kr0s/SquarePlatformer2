using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsConfigurator : MonoBehaviour
{
    [SerializeField] private Michsky.UI.ModernUIPack.CustomDropdown dropdownResolution;
    [SerializeField] private Sprite dropdownSprite;

    List<Resolution> resolutions = new List<Resolution>();

    private void Awake()
    {
        resolutions.AddRange(Screen.resolutions);

        if (dropdownResolution != null && dropdownResolution.dropdownItems.Count <= 1)
        {
            foreach (var resolution in resolutions)
            {
                dropdownResolution.CreateNewItem($"{resolution.width}x{resolution.height} ({resolution.refreshRate})", dropdownSprite);
            }
        }

        resolutions.Clear();
        resolutions.Add(Screen.currentResolution);
        resolutions.AddRange(Screen.resolutions);
    }

    public void ChangeGraphicsQuality()
    {
        // Not implemented.
        return;
    }

    /*
     * i equals to the FullscreenMode to use.
     * 0 = Exclusive Fullscreen
     * 1 = Borderless Window
     * 2 = Resizable Window
     */
    public void ChangeFullscreenMode(int i)
    {
        switch (i)
        {
            case 0:
                QualitySettings.vSyncCount = 0;
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Screen.SetResolution(resolutions[0].width, resolutions[0].height, true);
                break;

            case 1:
                QualitySettings.vSyncCount = 0;
                Screen.SetResolution(resolutions[0].width, resolutions[0].height, true);
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 2:
                QualitySettings.vSyncCount = 0;
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;

            default:
                break;
        }
    }

    // Resolution index
    public void SetResolution()
    {
        Screen.SetResolution(resolutions[dropdownResolution.selectedItemIndex].width, resolutions[dropdownResolution.selectedItemIndex].height, 
            Screen.fullScreenMode, resolutions[dropdownResolution.selectedItemIndex].refreshRate);
    }
}
