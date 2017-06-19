﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    public abstract class SpecialAbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float enegyCost = 10f;

        abstract public ISpecialAbility AddComponent(GameObject gameObjectToAttachTo);
    }
}
