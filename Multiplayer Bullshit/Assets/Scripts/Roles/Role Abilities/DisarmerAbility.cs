using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisarmerAbility : RoleAbility {

  // Start is called before the first frame update
  void Start() {

  }

  public override void SetAbilityText() {
    abilityText = GameObject.Find("Main Camera/Ability Text Canvas/AbilityText").GetComponent<TextMeshProUGUI>();
    abilityText.text = "Disarmer";
  }

  public override void UseAbility() {
    throw new System.NotImplementedException();
  }
}
