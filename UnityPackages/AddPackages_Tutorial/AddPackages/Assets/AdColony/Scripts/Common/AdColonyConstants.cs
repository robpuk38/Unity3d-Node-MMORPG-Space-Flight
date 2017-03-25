using System;

namespace AdColony {
    public class Constants {
        // These keys need to exactly match the keys in the plugin.

        // JSON keys for general Options
        public static string OptionsMetadataKey = "metadata";

        // JSON keys for AppOptions
        public static string AppOptionsDisableLoggingKey = "logging";
        public static string AppOptionsUserIdKey = "user_id";
        public static string AppOptionsOrientationKey = "orientation";
        // Android only keys:
        public static string AppOptionsMultiWindowEnabledKey = "multi_window_enabled";
        public static string AppOptionsOriginStoreKey = "origin_store";

        // JSON keys for AdOptions
        public static string AdOptionsPrePopupKey = "pre_popup";
        public static string AdOptionsPostPopupKey = "post_popup";

        // JSON keys for UserMetadata
        public static string UserMetadataAgeKey = "age";
        public static string UserMetadataInterestsKey = "interests";
        public static string UserMetadataGenderKey = "gender";
        public static string UserMetadataLatitudeKey = "latitude";
        public static string UserMetadataLongitudeKey = "longitude";
        public static string UserMetadataZipCodeKey = "zipcode";
        public static string UserMetadataHouseholdIncomeKey = "income";
        public static string UserMetadataMaritalStatusKey = "marital_status";
        public static string UserMetadataEducationLevelKey = "edu_level";

        // JSON keys for Zone
        public static string ZoneIdentifierKey = "zone_id";
        public static string ZoneTypeKey = "type";
        public static string ZoneEnabledKey = "enabled";
        public static string ZoneRewardedKey = "rewarded";
        public static string ZoneViewsPerRewardKey = "views_per_reward";
        public static string ZoneViewsUntilRewardKey = "views_until_reward";
        public static string ZoneRewardAmountKey = "reward_amount";
        public static string ZoneRewardNameKey = "reward_name";

        // JSON keys for IAP Opportunity
        public static string OnIAPOpportunityAdKey = "ad";
        public static string OnIAPOpportunityEngagementKey = "engagement";
        public static string OnIAPOpportunityIapProductIdKey = "iap_product_id";

        // JSON keys for currency reward
        public static string OnRewardGrantedZoneIdKey = "zone_id";
        public static string OnRewardGrantedSuccessKey = "success";
        public static string OnRewardGrantedNameKey = "name";
        public static string OnRewardGrantedAmountKey = "amount";

        // JSON keys for custom message
        public static string OnCustomMessageReceivedTypeKey = "type";
        public static string OnCustomMessageReceivedMessageKey = "message";

        public static string AdsManagerName = "AdColony";
        public static string AdsMessageNotInitialized = "AdColony SDK not initialized, use Configure()";
        public static string AdsMessageAlreadyInitialized = "AdColony SDK already initialized";
        public static string AdsMessageSDKUnavailable = "AdColony SDK unavailable on current platform";

        public static string PubServicesManagerName = "AdColonyPubServices";
        public static string PubServicesMessageNotInitialized = "AdColonyPubServices SDK not initialized, use PubServices.Configure()";
        public static string PubServicesMessageAlreadyInitialized = "AdColonyPubServices SDK already initialized";
        public static string PubServicesMessageSdkUnavailable = "AdColonyPubServices SDK unavailable on current platform";
    }
}
