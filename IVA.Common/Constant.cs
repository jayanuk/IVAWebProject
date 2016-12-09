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
    }
}
