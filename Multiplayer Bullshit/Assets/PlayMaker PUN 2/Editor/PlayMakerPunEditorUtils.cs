// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using HutongGames.PlayMakerEditor;
using UnityEngine;

using UnityEditor;

[InitializeOnLoad]
public class PlayMakerPunEditorUtils
{

    static PlayMakerPunEditorUtils()
    {
        Actions.AddCategoryIcon("Photon",PunCategoryIcon);
    }

    private static Texture sPunCategoryIcon = null;
    internal static Texture PunCategoryIcon
    {
        get
        {
            if (sPunCategoryIcon == null)
                sPunCategoryIcon = Resources.Load<Texture>("Pun_playmaker_category_icon");
            ;
            if (sPunCategoryIcon != null)
                sPunCategoryIcon.hideFlags = HideFlags.DontSaveInEditor;
            return sPunCategoryIcon;
        }
    }


}
