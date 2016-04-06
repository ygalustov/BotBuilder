﻿using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.Bot.Sample.EchoBot
{
    [Serializable]
    public class EchoCommandDialog
    {
        public static readonly CommandDialog<object> echoCommandDialog = new CommandDialog<object>().On(new Regex("^reset"), async (context, msg) =>
        {
            PromptDialog.Confirm(context, echoCommandDialog.ResultHandler,
            "Are you sure you want to reset the count?",
            "Didn't get that!");
        }).OnResult<bool>(async (context, result) =>
        {
            var confirm = await result;
            if (confirm)
            {
                context.UserData.SetValue("count", 0);
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
        })
          .OnDefault(async (context, msg) =>
            {
                int count;
                var message = await msg;
                context.UserData.TryGetValue("count", out count);
                context.UserData.SetValue("count", ++count);
                await context.PostAsync(string.Format("{0}: You said {1}", count, message.Text));
                context.Wait(echoCommandDialog.MessageReceived);
            });

    }
}