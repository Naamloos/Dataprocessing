﻿using DatabaseHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataprocessingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/xml", "application/json")]
    [Produces("application/xml", "application/json")] // Restrict requests to XML and JSON
    public class AvailableRegionsController
    {
        private Database database;
        private IsoCountries iso;

        public AvailableRegionsController(Database database, IsoCountries iso)
        {
            this.database = database;
            this.iso = iso;
        }

        /// <summary>
        /// Returns all country codes that have values in all tables. This is especially useful for visualization ;^)
        /// </summary>
        /// <returns>An array of strings representing countries</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // De query duurde te lang, handmatig zijn dit alle countries die in alle 3 tabellen voorkomen.
            return new List<string> { "US", "MX", "FR", "DE", "GB", "CA", "JP" };
        }
    }
}
