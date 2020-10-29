﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool wasTriggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && wasTriggered == false)
            {
                wasTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }

}
