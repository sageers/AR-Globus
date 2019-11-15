using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vuforia.Scripts
{
    public class CustomTrackableEventHandler : DefaultTrackableEventHandler
    {

        public Button btn_movbeobjects;
        protected override void Start()
        {
            base.Start();
        }

        protected override void OnTrackingFound()
        {
            base.OnTrackingFound();
            btn_movbeobjects.interactable = true;
        }

        protected override void OnTrackingLost()
        {
            base.OnTrackingLost();
            btn_movbeobjects.interactable = false;
        }
    }

}

