﻿using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DS1Catacombs
{
    public class PlaySound : MonoBehaviour
    {
        public string EnableSound;

        public string DisableSound;

        private void Start()
        {
            Util.PlaySound(EnableSound, gameObject);
        }

        private void OnDisable()
        {
            Util.PlaySound(DisableSound, gameObject);
        }
    }
}
