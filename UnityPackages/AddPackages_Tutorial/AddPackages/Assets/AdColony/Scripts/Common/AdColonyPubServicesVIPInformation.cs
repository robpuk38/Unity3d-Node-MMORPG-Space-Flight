using System;
using System.Collections;

namespace AdColony {
    public class PubServicesVIPInformation {
        // VIP rank name
        public string RankName { get; private set; }

        // Next VIP rank name
        public string NextRankName { get; private set; }

        // VIP rank level
        public int RankLevel { get; private set; }

        // Next VIP rank level
        public int NextRankLevel { get; private set; }

        // Percent progress to next rank. Max rank, 1.0.
        public float RankPercentProgress { get; private set; }

        // Total amount of in-game currency to grant per currency locale unit. This value should only be used for non-in-game currency IAP items, i.e. non-consumables or consumable objects.
        //
        // The mapping of in-game currency to currency unit is set from the AdColony Developer Portal
        //
        // For example:
        //
        // IAP Item:
        // Level Unlock A  - $0.99 USD
        // bonusPerCurrencyLocaleUnit = 0.15
        //
        // If the developer sets a ratio of 1000 coins per USD, when a user
        // purchases this product, they would receive a currency bonus of:
        //
        // 1000 * 0.99USD * 0.15 = 149 (148.5 rounded up)
        //
        // Bonus percentages may vary for each VIP rank.
        //
        // This is how the inGameCurrencyBonus is calculated for the OnInAppPurchaseReward event.
        public float BonusPerCurrencyLocaleUnit { get; private set; }

        // Next VIP rank BonusPerCurrencyLocaleUnit
        public float NextBonusPerCurrencyLocaleUnit { get; private set; }

        // Percent of the in-game currency to grant per IAP item.
        //
        // This bonus is above and beyond the base quantity of the IAP item.
        //
        // For example:
        //
        // IAP Item:
        // Gold Package A (1000 Coins) - $0.99 USD
        // bonusPerProductUnit = 0.15
        //
        // In this case, since the IAP product is in-game currency based, the user will receive an in-game currency bonus of:
        //
        // 1000 * 0.15 = 150 Coins
        //
        // Bonus percentages may vary for each VIP rank.
        //
        // This is how the inGameCurrencyBonus is calculated for the OnInAppPurchaseReward event.
        public float BonusPerProductUnit { get; private set; }

        // Next VIP rank BonusPerProductUnit
        public float NextBonusPerProductUnit { get; private set; }

        // Number of achievements visible within the overlay, completed and uncompleted
        public int TotalAchievementCount { get; private set; }

        // Number of pending redemptions available from achievements
        public int PendingAchievementRedemptionCount { get; private set; }

        // Raw data for VIP information
        public Hashtable Data { get; private set; }

        public PubServicesVIPInformation(Hashtable values) {
            RankName = "";
            NextRankName = "";
            RankLevel = 0;
            NextRankLevel = 0;
            RankPercentProgress = 0.0f;
            BonusPerCurrencyLocaleUnit = 0.0f;
            NextBonusPerCurrencyLocaleUnit = 0.0f;
            BonusPerProductUnit = 0.0f;
            NextBonusPerProductUnit = 0.0f;
            TotalAchievementCount = 0;
            PendingAchievementRedemptionCount = 0;
            Data = new Hashtable();
            if (values != null) {
                Data = (Hashtable)values.Clone();

                if (values.ContainsKey("rank_name")) {
                    RankName = values["rank_name"] as string;
                }
                if (values.ContainsKey("next_rank_name")) {
                    NextRankName = values["next_rank_name"] as string;
                }
                if (values.ContainsKey("rank_level")) {
                    RankLevel = Convert.ToInt32(values["rank_level"]);
                }
                if (values.ContainsKey("next_rank_level")) {
                    NextRankLevel = Convert.ToInt32(values["next_rank_level"]);
                }
                if (values.ContainsKey("progress_to_next_rank_micro")) {
                    RankPercentProgress = Convert.ToSingle(values["progress_to_next_rank_micro"]) / 1000000.0f;
                }
                if (values.ContainsKey("bonus_per_currency_locale_unit_micro")) {
                    BonusPerCurrencyLocaleUnit = Convert.ToSingle(values["bonus_per_currency_locale_unit_micro"]) / 1000000.0f;
                }
                if (values.ContainsKey("next_bonus_per_currency_locale_unit_micro")) {
                    NextBonusPerCurrencyLocaleUnit = Convert.ToSingle(values["next_bonus_per_currency_locale_unit_micro"]) / 1000000.0f;
                }
                if (values.ContainsKey("bonus_per_product_unit_micro")) {
                    BonusPerProductUnit = Convert.ToSingle(values["bonus_per_product_unit_micro"]) / 1000000.0f;
                }
                if (values.ContainsKey("next_bonus_per_product_unit_micro")) {
                    NextBonusPerProductUnit = Convert.ToSingle(values["next_bonus_per_product_unit_micro"]) / 1000000.0f;
                }
                if (values.ContainsKey("total_achievement_count")) {
                    TotalAchievementCount = Convert.ToInt32(values["total_achievement_count"]);
                }
                if (values.ContainsKey("pending_achievement_redemption_count")) {
                    PendingAchievementRedemptionCount = Convert.ToInt32(values["pending_achievement_redemption_count"]);
                }
            }
        }
    }
}
