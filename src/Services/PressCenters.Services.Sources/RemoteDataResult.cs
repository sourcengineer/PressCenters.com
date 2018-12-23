﻿namespace PressCenters.Services.Sources
{
    using System.Collections.Generic;

    public class RemoteDataResult
    {
        public RemoteDataResult()
        {
            this.News = new List<RemoteNews>();
        }

        public IEnumerable<RemoteNews> News { get; set; }
    }
}
