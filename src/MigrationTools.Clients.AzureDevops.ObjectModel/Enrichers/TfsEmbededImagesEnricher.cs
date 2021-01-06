﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using MigrationTools._EngineV1.Enrichers;
using MigrationTools.DataContracts;
using MigrationTools.Processors;

namespace MigrationTools.Enrichers
{
    public class TfsEmbededImagesEnricher : EmbededImagesRepairEnricherBase
    {
        public IMigrationEngine Engine { get; private set; }

        public TfsEmbededImagesEnricher(IServiceProvider services, ILogger<TfsEmbededImagesEnricher> logger) : base(services, logger)
        {
            Engine = services.GetRequiredService<IMigrationEngine>();
            //
        }

        [Obsolete]
        public override void Configure(bool save = true, bool filter = true)
        {
            throw new NotImplementedException();
        }

        [Obsolete("v2 Archtecture: use Configure(bool save = true, bool filter = true) instead", true)]
        public override void Configure(IProcessorEnricherOptions options)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override int Enrich(WorkItemData sourceWorkItem, WorkItemData targetWorkItem)
        {
            FixEmbededImages(targetWorkItem, Engine.Source.Config.AsTeamProjectConfig().Collection.ToString(), Engine.Target.Config.AsTeamProjectConfig().Collection.ToString(), Engine.Source.Config.AsTeamProjectConfig().PersonalAccessToken);
            return 0;
        }

        /**
      *  from https://gist.github.com/pietergheysens/792ed505f09557e77ddfc1b83531e4fb
      */

        protected override void FixEmbededImages(WorkItemData wi, string oldTfsurl, string newTfsurl, string sourcePersonalAccessToken = "")
        {
            //Log.LogInformation("EmbededImagesRepairEnricher: Fixing HTML field attachments for work item {Id} from {OldTfsurl} to {NewTfsUrl}", wi.Id, oldTfsurl, GetUrlWithOppositeSchema(oldTfsurl));
            Log.LogInformation("EmbededImagesRepairEnricher: Fixing HTML field attachments for work item {Id} from {oldTfsurl} to {newTfsurl}", wi.Id, oldTfsurl, newTfsurl);

            var oldTfsurlOppositeSchema = GetUrlWithOppositeSchema(oldTfsurl);
            string regExSearchForImageUrl = "(?<=<img.*src=\")[^\"]*";

            foreach (Field field in wi.ToWorkItem().Fields)
            {
                if (field.FieldDefinition.FieldType == FieldType.Html)
                {
                    MatchCollection matches = Regex.Matches((string)field.Value, regExSearchForImageUrl);

                    string regExSearchFileName = "(?<=FileName=)[^=]*";
                    foreach (Match match in matches)
                    {
                        if (match.Value.ToLower().Contains(oldTfsurl.ToLower().Replace(" ","%20")) || match.Value.ToLower().Contains(oldTfsurlOppositeSchema.ToLower().Replace(" ", "%20")))
                        {
                            //save image locally and upload as attachment
                            Match newFileNameMatch = Regex.Match(match.Value, regExSearchFileName, RegexOptions.IgnoreCase);
                            if (newFileNameMatch.Success)
                            {
                                Log.LogDebug("EmbededImagesRepairEnricher: field '{fieldName}' has match: {matchValue}", field.Name, System.Net.WebUtility.HtmlDecode(match.Value));
                                string fullImageFilePath = Path.GetTempPath() + newFileNameMatch.Value;

                                using (var httpClient = new HttpClient(_httpClientHandler, false))
                                {
                                    if (!string.IsNullOrEmpty(sourcePersonalAccessToken))
                                    {
                                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", sourcePersonalAccessToken))));
                                    }
                                    var result = DownloadFile(httpClient, match.Value, fullImageFilePath);
                                    if (!result.IsSuccessStatusCode)
                                    {
                                        if (_ignore404Errors && result.StatusCode == HttpStatusCode.NotFound)
                                        {
                                            Log.LogDebug("EmbededImagesRepairEnricher: Image {MatchValue} could not be found in WorkItem {WorkItemId}, Field {FieldName}", match.Value, wi.Id, field.Name);
                                            continue;
                                        }
                                        else
                                        {
                                            result.EnsureSuccessStatusCode();
                                        }
                                    }
                                }

                                if (GetImageFormat(File.ReadAllBytes(fullImageFilePath)) == ImageFormat.unknown)
                                {
                                    throw new Exception($"Downloaded image [{fullImageFilePath}] from Work Item [{wi.ToWorkItem().Id}] Field: [{field.Name}] could not be identified as an image. Authentication issue?");
                                }

                                int attachmentIndex = wi.ToWorkItem().Attachments.Add(new Attachment(fullImageFilePath));
                                wi.SaveToAzureDevOps();

                                var newImageLink = wi.ToWorkItem().Attachments[attachmentIndex].Uri.ToString();

                                field.Value = field.Value.ToString().Replace(match.Value, newImageLink);
                                wi.ToWorkItem().Attachments.RemoveAt(attachmentIndex);
                                wi.SaveToAzureDevOps();
                                File.Delete(fullImageFilePath);
                            }
                        }
                    }
                }
            }
        }

        protected override void RefreshForProcessorType(IProcessor processor)
        {
            throw new NotImplementedException();
        }

        protected override void EntryForProcessorType(IProcessor processor)
        {
            throw new NotImplementedException();
        }
    }
}