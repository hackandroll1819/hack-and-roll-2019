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
  
  public GameObject ToSpawnPrefab;

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
}
