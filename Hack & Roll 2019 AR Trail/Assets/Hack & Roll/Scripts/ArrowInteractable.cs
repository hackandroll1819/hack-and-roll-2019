using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowInteractable : MonoBehaviour
{
  public ParticleSystem ParticleSystem;

  private void Start()
  {
    ParticleSystem.gameObject.SetActive(false);
  }

  private void OnMouseUp()
  {
    if (!ParticleSystem.gameObject.activeSelf)
    {
      ParticleSystem.gameObject.SetActive(true);
      StartCoroutine(OffPS());
    }
  }

  private IEnumerator OffPS()
  {
    yield return new WaitForSeconds(ParticleSystem.main.duration);
    ParticleSystem.gameObject.SetActive(false);
  }
}
