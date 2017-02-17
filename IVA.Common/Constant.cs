using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.Common
{
    public class Constant
    {
        public const string DEFAULT_DB_CONNECTION = "DefaultConnection";
        public const string OWIN_DB_CONNECTION = "DefaultConnection";       

        public struct DateFormatType
        {
            public const string YYYYMMDD = "yyyy-MM-dd";
        }

        public struct UserType
        {
            public const int BUYER = 1;
            public const int SELLER = 2;
        }

        public struct ErrorCodes
        {
            public const string AUTH_ERROR = "AUTH_ERROR";            
        }

        public struct ConfigurationKeys
        {
            public const string SMS_GatewayURL = "SMS_GatewayURL";
            public const string SMS_GatewayAuthCode = "SMS_Gateway_AuthCode";
            public const string Token_Url = "Token_Url";
            public const string UTC_Offset = "UTC_Offset";
            public const string DAYS_TO_EXPIRE_REQUEST = "REQUEST_EXPIRE_WINDOW";
            public const string HOURS_TO_FOLLOW_UP = "HOURS_TO_FOLLOW_UP";
            public const string NOTIFICATION_TITLE = "NOTIFICATION_TITLE";
        }

        public enum ServiceRequestStatus
        {
            Initial = 1,
            PendingResponse = 2,
            SellerResponded = 3,
            Expired = 4,
            Closed = 5
        }

        public enum MessageStatus
        {
            Initial = 1,
            Sent = 2,
            Delivered = 3,
            Read = 4,
            Deleted = 5
        }

        public enum QuotationStatus
        {
            Initial = 1,
            Accepted = 2,
            Closed = 3
        }
        
        public struct ChatGroup
        {
            public const string BUYER = "BUYER";
            public const string SELLER = "SELLER";
        }

        public struct PromotionStatus
        {
            public const int ACTIVE = 1;
            public const int INACTIVE = 2;
            public const int EXPIRED = 3;
        }

        public struct PromotionType
        {            
            public const int OFFER = 1;
            public const int PROMOTION = 2;
        }

        public enum ContactMethod
        {
            Phone = 1,
            Message = 2,            
            Both = 0
        }

        public struct Notification
        {
            //public const string TITLE = "FindExpert";            
            public const string NEW_REQUEST_TEXT = "You have received a new request";            
            public const string NEW_MESSAGE_TEXT = "You have recieved a new message";            
            public const string NEW_QUOTATION_TEXT = "You have recieved a new quotation";            
            public const string ACCCEPTED_TEXT = "Your quotation was accepted";
        }

        public enum NotificationType
        {
            Request = 1,
            Message = 2,
            Quotation = 3,
            Accept = 4,
            FollowUpBuyer = 5,
            FollowUpAgent = 6
        }

        public struct Paging
        {
            public const int BUYER_REQUESTS_PER_PAGE = 20;
            public const int AGENT_REQUESTS_PER_PAGE = 20;
            public const int MESSAGE_THREADS_PER_PAGE = 20;
        }

    }
}
