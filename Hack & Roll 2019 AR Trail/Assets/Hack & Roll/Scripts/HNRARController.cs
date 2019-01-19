using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class HNRARController : MonoBehaviour
{
  /// <summary>
  /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
  /// </summary>
  public Camera FirstPersonCamera;

  public GameObject[] ToSpawnPrefabs;

  /// <summary>
  /// A game object parenting UI for displaying the "searching for planes" snackbar.
  /// </summary>
  public GameObject SearchingForPlaneUI;

  /// <summary>
  /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
  /// the application to avoid per-frame allocations.
  /// </summary>
  private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

  /// <summary>
  /// The overlay containing the fit to scan user guide.
  /// </summary>
  public GameObject FitToScanOverlay;

  /// <summary>
  /// Stores all detecected Augmented Images
  /// </summary>
  private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

  private bool m_IsQuitting = false;

  private Anchor existingAnchor;

  private int currTrackingIdx = -1;

  private bool UserResponded = true;

  private AugmentedImage nextAI = null;

  private void Update()
  {
    if (m_AllPlanes.Count <= 0)
    {
      SearchingForPlaneUI.SetActive(true);
    }
    else
    {
      _ShowAndroidToastMessage(m_TempAugmentedImages.Count.ToString());
      if (m_TempAugmentedImages.Count <= 0)
      {
        FitToScanOverlay.SetActive(true);
      }
      else
      {
        FitToScanOverlay.SetActive(false);
      }
      SearchingForPlaneUI.SetActive(false);
    }

    _UpdateApplicationLifecycle();

    TrackAllDetectedPlanes();

    if (m_AllPlanes.Count <= 0)
    {
      SearchingForPlaneUI.SetActive(true);
      return;
    }
    SearchingForPlaneUI.SetActive(false);

    FindAugmentedImages();

    // TODO: Include a way to track which augmented image is active

    if (!UserResponded)
    {
      return;
    }

    foreach (AugmentedImage AI in m_TempAugmentedImages)
    {
      if (currTrackingIdx < 0)
      {
        ReinstantiateRoute(AI);
        break;
      }
      else if (AI.DatabaseIndex != currTrackingIdx)
      {
        // TODO: Prompt User for change
        _ShowAndroidToastMessage("Prompt User");
        nextAI = AI;
        UserResponded = false;
      }
    }
  }

  public void UserResponse(bool yo)
  {
    if (yo)
    {
      Destroy(existingAnchor.gameObject);
      ReinstantiateRoute(nextAI);
      nextAI = null;
    }
    // TODO: Close Pop up
    UserResponded = true;
  }

  private void ReinstantiateRoute(AugmentedImage AI)
  {
    existingAnchor = m_AllPlanes[AI.DatabaseIndex].CreateAnchor(AI.CenterPose);
    Instantiate(ToSpawnPrefabs[AI.DatabaseIndex], AI.CenterPose.position, AI.CenterPose.rotation, existingAnchor.transform);
    currTrackingIdx = AI.DatabaseIndex;
  }

  private void FindAugmentedImages()
  {
    Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);
  }

  private void TrackAllDetectedPlanes()
  {
    Session.GetTrackables<DetectedPlane>(m_AllPlanes);
  }

  #region MISC
  /// <summary>
  /// Check and update the application lifecycle.
  /// </summary>
  private void _UpdateApplicationLifecycle()
  {
    // Exit the app when the 'back' button is pressed.
    if (Input.GetKey(KeyCode.Escape))
    {
      Application.Quit();
    }

    // Only allow the screen to sleep when not tracking.
    if (Session.Status != SessionStatus.Tracking)
    {
      const int lostTrackingSleepTimeout = 15;
      Screen.sleepTimeout = lostTrackingSleepTimeout;
    }
    else
    {
      Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    if (m_IsQuitting)
    {
      return;
    }

    // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
    if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
    {
      _ShowAndroidToastMessage("Camera permission is needed to run this application.");
      m_IsQuitting = true;
      Invoke("_DoQuit", 0.5f);
    }
    else if (Session.Status.IsError())
    {
      _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
      m_IsQuitting = true;
      Invoke("_DoQuit", 0.5f);
    }
  }

  /// <summary>
  /// Show an Android toast message.
  /// </summary>
  /// <param name="message">Message string to show in the toast.</param>
  private void _ShowAndroidToastMessage(string message)
  {
    AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

    if (unityActivity != null)
    {
      AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
      unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
      {
        AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
            message, 0);
        toastObject.Call("show");
      }));
    }
  }
  #endregion
}
