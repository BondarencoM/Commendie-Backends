﻿using RecommendationService.Extensions;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.Personas;
using RecommendationService.Models.Wikibase;
using RecommendationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WikiClientLibrary;
using WikiClientLibrary.Sites;
using WikiClientLibrary.Wikibase;

namespace RecommendationService.Services
{
    public class WikiPersonaScrappingService : IPersonaScrappingService
    {
        private readonly WikiSite wiki;

        public WikiPersonaScrappingService(WikiSite wiki)
        {
            this.wiki = wiki;
        }

        public async Task<Persona> ScrapePersonaDetails(string wikiId)
        {
            await wiki.Initialization;

            var entity = new Entity(wiki, wikiId);
            try
            {
                await entity.RefreshAsync(EntityQueryOptions.FetchAllProperties);
            }
            catch(OperationFailedException e)
            {
                if (e.Message.Contains("no-such-entity"))
                    throw new EntityNotFoundException(e.Message);
            }

            if (entity.IsHuman() == false) 
                throw new AddedEntityIsNotHuman(entity);

            var model = new Persona()
            {
                Name = entity.Labels["en"],
                Description = entity.Descriptions["en"],
                WikiId = entity.Id,
                ImageUri = entity.ImageUris().FirstOrDefault(),
                WikipediaUri = entity.WikipediaLink(),
            };

            return model;
        }
    }
}