using System;
using System.Collections;

namespace AdColony {
    public class PubServicesPushNotification {
        // Unique notification id
        public string NotificationId { get; private set; }

        // Action that triggered handling of the push notification (iOS8+ only)
        public string Action { get; private set; }

        // String displayed within the push notification
        public string Message { get; private set; }

        // Title displayed within the push notification
        public string Title { get; private set; }

        // Category of the notification (iOS8 only)
        public string Category { get; private set; }

        // Date user interacted with the push or was received if in foreground
        public DateTime DateReceived { get; private set; }

        // Developer-specific data set from the AdColony Developer Portal. This can be used for deep-linking or any special logic the developer would like to specify.
        public string Payload { get; private set; }

        // Whether or not this is a notification sent by AdColony PubServices
        public bool IsPubServicesNotification { get; private set; }

        // Raw data
        public Hashtable Data { get; private set; }

        public PubServicesPushNotification(Hashtable values) {
            NotificationId = "";
            Action = "";
            Message = "";
            Title = "";
            Category = "";
            DateReceived = new DateTime();
            Payload = "";
            IsPubServicesNotification = false;
            Data = new Hashtable();
            if (values != null) {
                Data = (Hashtable)values.Clone();

                if (values.ContainsKey("notification_id")) {
                    NotificationId = values["notification_id"] as string;
                }
                if (values.ContainsKey("action")) {
                    Action = values["action"] as string;
                }
                if (values.ContainsKey("message")) {
                    Message = values["message"] as string;
                }
                if (values.ContainsKey("title")) {
                    Title = values["title"] as string;
                }
                if (values.ContainsKey("category")) {
                    Category = values["category"] as string;
                }
                if (values.ContainsKey("payload")) {
                    Payload = values["payload"] as string;
                }
                if (values.ContainsKey("is_pubservices_notification")) {
                    IsPubServicesNotification = Convert.ToInt32(values["is_pubservices_notification"]) == 1;
                }
                if (values.ContainsKey("date_received")) {
                    DateReceived = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds((double)values["date_received"]);
                }
            }
        }
    }
}
