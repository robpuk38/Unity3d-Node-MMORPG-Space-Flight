using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AdColony {
    public class Zone {
        public string Identifier;
        public AdZoneType Type;
        public bool Enabled;
        public bool Rewarded;
        public int ViewsPerReward;
        public int ViewsUntilReward;
        public int RewardAmount;
        public string RewardName;

        public Zone() {
        }

        public Zone(Hashtable values) {
            if (values != null) {
                if (values.ContainsKey(Constants.ZoneIdentifierKey)) {
                    Identifier = values[Constants.ZoneIdentifierKey] as string;
                }
                if (values.ContainsKey(Constants.ZoneTypeKey)) {
                    Type = (AdZoneType)Convert.ToInt32(values[Constants.ZoneTypeKey]);
                }
                if (values.ContainsKey(Constants.ZoneEnabledKey)) {
                    Enabled = Convert.ToBoolean(Convert.ToInt32 (values[Constants.ZoneEnabledKey]));
                }
                if (values.ContainsKey(Constants.ZoneRewardedKey)) {
                    Rewarded = Convert.ToBoolean(Convert.ToInt32(values[Constants.ZoneRewardedKey]));
                }
                if (values.ContainsKey(Constants.ZoneViewsPerRewardKey)) {
                    ViewsPerReward = Convert.ToInt32(values[Constants.ZoneViewsPerRewardKey]);
                }
                if (values.ContainsKey(Constants.ZoneViewsUntilRewardKey)) {
                    ViewsUntilReward = Convert.ToInt32(values[Constants.ZoneViewsUntilRewardKey]);
                }
                if (values.ContainsKey(Constants.ZoneRewardAmountKey)) {
                    RewardAmount = Convert.ToInt32(values[Constants.ZoneRewardAmountKey]);
                }
                if (values.ContainsKey(Constants.ZoneRewardNameKey)) {
                    RewardName = values[Constants.ZoneRewardNameKey] as string;
                }
            }
        }

        public string toJsonString() {
            Hashtable data = new Hashtable();
            data.Add(Constants.ZoneIdentifierKey, Identifier);
            data.Add(Constants.ZoneTypeKey, Convert.ToInt32(Type).ToString());
            data.Add(Constants.ZoneEnabledKey, Enabled ? "1" : "0");
            data.Add(Constants.ZoneRewardedKey, Rewarded ? "1" : "0");
            data.Add(Constants.ZoneViewsPerRewardKey, ViewsPerReward.ToString());
            data.Add(Constants.ZoneViewsUntilRewardKey, ViewsUntilReward.ToString());
            data.Add(Constants.ZoneRewardAmountKey, RewardAmount.ToString());
            data.Add(Constants.ZoneRewardNameKey, RewardName);

            return AdColonyJson.Encode(data);
        }
    }
}
