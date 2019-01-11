﻿namespace PressCenters.Services.Sources.BgInstitutions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using AngleSharp.Dom;

    public class GovernmentBgSource : BaseSource
    {
        public override string BaseUrl { get; } = "http://www.government.bg/";

        public override IEnumerable<RemoteNews> GetLatestPublications() =>
            this.GetPublications("bg/prestsentar/novini", ".articles .item a");

        public override IEnumerable<RemoteNews> GetAllPublications()
        {
            for (var i = 1; i <= 60; i++)
            {
                var news = this.GetPublications($"bg/prestsentar/novini?page={i}", ".articles .item a");
                Console.WriteLine($"Page {i} => {news.Count} news");
                foreach (var remoteNews in news)
                {
                    yield return remoteNews;
                }
            }
        }

        protected override RemoteNews ParseDocument(IDocument document)
        {
            var titleElement = document.QuerySelector(".view h1");
            var title = titleElement.TextContent.Trim();

            var timeElement = document.QuerySelector(".view p");
            var timeAsString = timeElement.TextContent;
            var time = DateTime.ParseExact(timeAsString, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            var imageElement = document.QuerySelector(".view .gallery img");
            var imageUrl = imageElement?.GetAttribute("src");

            var contentElement = document.QuerySelector(".view");
            contentElement.RemoveRecursively(titleElement);
            contentElement.RemoveRecursively(timeElement);
            contentElement.RemoveRecursively(document.QuerySelector(".view .gallery"));
            this.NormalizeUrlsRecursively(contentElement, this.BaseUrl);
            var content = contentElement.InnerHtml.Trim();

            return new RemoteNews(title, content, time, imageUrl);
        }
    }
}
