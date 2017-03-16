using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AdColony {

    public class InterstitialAd {
        public string ZoneId;
        public bool Expired;   // this could be dynamic later, might want to change
        public string Id;   // Not really public, this is for internal use only

        // ---------------------------------------------------------------------------

#region Internal

        public InterstitialAd(Hashtable values) {
            if (values != null) {
                if (values.ContainsKey("zone_id")) {
                    ZoneId = values["zone_id"] as string;
                }
                if (values.ContainsKey("expired")) {
                    Expired = Convert.ToBoolean(values["expired"]);
                }
                if (values.ContainsKey("id")) {
                    Id = values["id"] as string;
                }
            }

            Debug.Log("InterstitialAd constructor: id: " + Id);
        }

        ~InterstitialAd() {
            if (IsValid()) {
                Ads.SharedInstance.EnqueueAction(() => { Ads.DestroyAd(Id); });
            }
        }

        private bool IsValid() {
            return !System.String.IsNullOrEmpty(Id);
        }

#endregion

    }
}
