using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AdColony {
    public enum AdsIAPEngagementType {
        AdColonyIAPEngagementEndCard = 0,
        AdColonyIAPEngagementOverlay
    }

    public enum AdZoneType {
        AdColonyZoneTypeInterstitial = 0,
        AdColonyZoneTypeBanner,
        AdColonyZoneTypeNative
    }

    public enum AdOrientationType {
        AdColonyOrientationPortrait = 0,
        AdColonyOrientationLandscape,
        AdColonyOrientationAll
    }

    public enum AdMetadataGenderType {
        Unknown = 0,
        Male,
        Female
    }

    public enum AdMetadataMaritalStatusType {
        Unknown = 0,
        Single,
        Married
    }

    public enum AdMetadataEducationLevelType {
        Unknown = 0,
        GradeSchool,
        SomeHighSchool,
        HighSchool,
        SomeCollege,
        AssociateDegree,
        BachelorDegree,
        GraduateDegree
    }

    public enum PubServicesStatusType {
        Unknown,
        Unavailable,
        Connecting,
        Available,
        Invisible,
        Maintenance,
        Disabled,
        Banned,
    }

    [Flags]
    public enum PubServicesNotificationType {
        None = 0,
        Toast = 1,
        Modal = 2
    }

    [Flags]
    public enum PubServicesPushNotificationType {
        None = 0,
        Badge = 1,
        Sound = 2,
        Alert = 3
    }
}
