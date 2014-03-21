﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.UnitOfWork;
using Umbraco.Core.Services;

namespace Chauffeur.Deliverables
{
    [DeliverableName("content-type")]
    public sealed class ContentTypeDeliverable : Deliverable, IProvideDirections
    {
        public ContentTypeDeliverable(TextReader reader, TextWriter writer)
            : base(reader, writer)
        {
        }

        public override async Task<DeliverableResponse> Run(string[] args)
        {
            var operation = args[0];

            switch (operation.ToLower())
            {
                case "get-all":
                    await GetAll();
                    break;

                default:
                    await Out.WriteLineAsync(string.Format("The operation `{0}` is not supported", operation));
                    break;
            }

            return await base.Run(args);
        }

        private async Task GetAll()
        {
            var rf = new RepositoryFactory(true);
            var cs = new ContentService(rf);
            var ms = new MediaService(rf);
            var cts = new ContentTypeService(rf, cs, ms);

            var types = cts.GetAllContentTypes();

            await Out.WriteLineAsync("\tId\tAlias\tName");
            foreach (var type in types)
                await Out.WriteLineAsync(string.Format("\t{0}\t{1}\t{2}", type.Id, type.Alias, type.Name));
        }

        public async Task Directions()
        {
            await Out.WriteLineAsync("content-type");
            await Out.WriteLineAsync("\tPerform operations against Umbraco Content Types");
            await Out.WriteLineAsync("");
        }
    }
}