using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public static class OptionsPP {
  public static Volume volume;

  public static float bloomValue = 1;

  public static float brightnessValue = 1; // [0, 0.95]

  public static float shadowsValue = 1;

  public static float fovValue = 40;

  public static int qualityValue = QualitySettings.GetQualityLevel();

  public static bool fullScreen = false;

  public static int textureValue = 1;

  public static int aaValue = 0;

  public static bool xValue = false;

  public static bool yValue = false;

  public static bool vsyncVal = false;

}
