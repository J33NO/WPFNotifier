using System;
using System.Collections.Generic;
using System.Text;
using Twilio.AspNet.Core;
using Twilio.Rest.Api.V2010.Account;

namespace WPFNotifier
{
    public class SMS : TwilioController
    {
        public static void CreateMessage(string to, string from, string sid, string body)
        {
            var message = MessageResource.Create(
                to: to,
                from: from,
                body: body);
        }
    }
}
