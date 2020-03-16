﻿using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.TraceExtensions;
using Newtonsoft.Json;

namespace ExtensionsLib.Actions.MSGraph
{
    public class CreateOnlineMeeting : Dialog
    {
        [JsonProperty("$kind")]
        public const string DeclarativeType = "Microsoft.Graph.Calendar.CreateOnlineMeeting";

        [JsonConstructor]
        public CreateOnlineMeeting([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
            : base()
        {
            this.RegisterSourceLocation(callerPath, callerLine);
        }

        [JsonProperty("resultProperty")]
        public ValueExpression ResultProperty { get; set; }

        [JsonProperty("token")]
        public StringExpression Token { get; set; }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            var dcState = dc.GetState();
            var token = this.Token.GetValue(dcState);

            var graphClient = GraphClient.GetAuthenticatedClient(token);

            object result = null;

            // Write Trace Activity for the http request and response values
            await dc.Context.TraceActivityAsync(nameof(CreateOnlineMeeting), result, valueType: DeclarativeType, label: this.Id).ConfigureAwait(false);

            if (this.ResultProperty != null)
            {
                this.ResultProperty.SetValue(result);
            }

            // return the actionResult as the result of this operation
            return await dc.EndDialogAsync(result: result, cancellationToken: cancellationToken);
        }
    }
}
