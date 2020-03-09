﻿using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Microsoft.Bot.Builder.TraceExtensions;
using Newtonsoft.Json;

namespace BotProject.Actions.MSGraph
{
    public class GetEvents : Dialog
    {
        [JsonProperty("$kind")]
        public const string DeclarativeType = "Microsoft.Graph.Calendar.GetEvents";

        [JsonConstructor]
        public GetEvents([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
            : base()
        {
            this.RegisterSourceLocation(callerPath, callerLine);
        }

        [JsonProperty("resultProperty")]
        public string ResultProperty { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            var token = await new TextTemplate(Token).BindToData(dc.Context, dc.GetState());

            var graphClient = GraphClient.GetAuthenticatedClient(token);

            //var items = new List<Event>();

            //// Define the time span for the calendar view.
            //var options = new List<QueryOption>
            //{
            //    new QueryOption("startDateTime", startTime.ToString("o")),
            //    new QueryOption("endDateTime", endTime.ToString("o")),
            //    new QueryOption("$orderBy", "start/dateTime"),
            //};

            //IUserCalendarViewCollectionPage events = null;

            //try
            //{
            //    events = await _graphClient.Me.CalendarView.Request(options).GetAsync();
            //}
            //catch (ServiceException ex)
            //{
            //    throw GraphClient.HandleGraphAPIException(ex);
            //}

            //if (events?.Count > 0)
            //{
            //    items.AddRange(events);
            //}

            //while (events.NextPageRequest != null)
            //{
            //    events = await events.NextPageRequest.GetAsync();
            //    if (events?.Count > 0)
            //    {
            //        items.AddRange(events);
            //    }
            //}

            object result = null;
            var jsonResult = JsonConvert.SerializeObject(result);

            // Write Trace Activity for the http request and response values
            await dc.Context.TraceActivityAsync(nameof(GetContacts), jsonResult, valueType: DeclarativeType, label: this.Id).ConfigureAwait(false);

            if (this.ResultProperty != null)
            {
                dc.GetState().SetValue(this.ResultProperty, jsonResult);
            }

            // return the actionResult as the result of this operation
            return await dc.EndDialogAsync(result: jsonResult, cancellationToken: cancellationToken);
        }
    }
}
