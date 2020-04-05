using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHelper;
using DatabaseHelper.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataprocessingApi.Controllers
{
    /// <summary>
    /// Terrorism controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/xml", "application/json")]
    [Produces("application/xml", "application/json")] // Restrict requests to XML and JSON
    public class TerrorismController : Controller
    {
        const string JSON_SCHEMA = "/schemas/json/TerrorismEvent.json";
        const string XML_SCHEMA = "/schemas/xml/TerrorismEvent.xsd";
        const string JSON_ARRAY_SCHEMA = "/schemas/json/ArrayOfTerrorismEvent.json";
        const string XML_ARRAY_SCHEMA = "/schemas/xml/ArrayOfTerrorismEvent.xsd";

        private readonly Database database;
        private readonly IsoCountries iso; // This one is for translating ISO countries.

        /// <summary>
        /// Constructs a new TerrorismController.
        /// </summary>
        /// <param name="database">Database to use</param>
        /// <param name="iso">Iso country converter</param>
        public TerrorismController(Database database, IsoCountries iso)
        {
            this.database = database.NewConnection();
            this.iso = iso;
        }

        /// <summary>
        /// Gets all terrorism events in a specific region in a specific year.
        /// </summary>
        /// <param name="region">Region the event happened in.</param>
        /// <param name="year">Year it happened.</param>
        /// <returns>A terrorism event.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<TerrorismEvent>> Get(string region, int year)
        {
            // Add schema header related to accept data type
            this.HttpContext.Request.Headers.TryGetValue("Accept", out var accept);

            switch (accept)
            {
                case "application/json":
                    Response.Headers.Add("link", JSON_ARRAY_SCHEMA);
                    break;
                case "application/xml":
                    Response.Headers.Add("link", XML_ARRAY_SCHEMA);
                    break;
                default:
                    return BadRequest("Invalid accept header! (application/xml OR application/json)");
            }

            // return all terrorism events in the given region/year combo
            var country = iso.Countries[region.ToUpper()];
            return database.Gtd.Where(x => 
                x.country_txt.ToLower() == country.ToLower()
                && x.iyear == year
                )?.ToList();
        }

        /// <summary>
        /// Deletes an event from the Database.
        /// </summary>
        /// <param name="eventid">ID of the event.</param>
        /// <returns>The vent you deleted.</returns>
        [HttpDelete]
        public ActionResult<TerrorismEvent> Delete(int eventid)
        {
            // Add schema header related to accept data type
            this.HttpContext.Request.Headers.TryGetValue("Accept", out var accept);

            switch (accept)
            {
                case "application/json":
                    Response.Headers.Add("link", JSON_SCHEMA);
                    break;
                case "application/xml":
                    Response.Headers.Add("link", XML_SCHEMA);
                    break;
                default:
                    return BadRequest("Invalid accept header! (application/xml OR application/json)");
            }


            if (!database.Gtd.Any(x => x.eventid == eventid))
            {
                return Conflict("No such event to delete!");
            }

            // deleting by eventid
            var deletable = database.Gtd.First(x => x.eventid == eventid);

            database.Gtd.Remove(deletable);
            database.SaveChanges();

            return deletable;
        }

        /// <summary>
        /// Updates an event in the database.
        /// </summary>
        /// <param name="updatedEvent">Your updated event.</param>
        /// <returns>The old version of your updated event.</returns>
        [HttpPut]
        public ActionResult<TerrorismEvent> Put([FromBody]TerrorismEvent updatedEvent)
        {
            // Add schema header related to accept data type
            this.HttpContext.Request.Headers.TryGetValue("Accept", out var accept);

            switch (accept)
            {
                case "application/json":
                    Response.Headers.Add("link", JSON_SCHEMA);
                    break;
                case "application/xml":
                    Response.Headers.Add("link", XML_SCHEMA);
                    break;
                default:
                    return BadRequest("Invalid accept header! (application/xml OR application/json)");
            }

            if (!database.Gtd.Any(x => x.eventid == updatedEvent.eventid))
            {
                return Conflict("No such event with this eventid to update!");
            }

            //update an event and return the old value
            var oldEvent = database.Gtd.First(x => x.eventid == updatedEvent.eventid);

            database.Gtd.Update(updatedEvent);
            database.SaveChanges();

            return oldEvent;
        }

        /// <summary>
        /// Adds a new event to the database.
        /// </summary>
        /// <param name="newEvent">The new event you want to add.</param>
        /// <returns>Your new event.</returns>
        [HttpPost]
        public ActionResult<TerrorismEvent> Post([FromBody]TerrorismEvent newEvent)
        {
            // Add schema header related to accept data type
            this.HttpContext.Request.Headers.TryGetValue("Accept", out var accept);

            switch (accept)
            {
                case "application/json":
                    Response.Headers.Add("link", JSON_SCHEMA);
                    break;
                case "application/xml":
                    Response.Headers.Add("link", XML_SCHEMA);
                    break;
                default:
                    return BadRequest("Invalid accept header! (application/xml OR application/json)");
            }

            // check ID
            // error on exist
            // return new object on success.
            if (database.Gtd.Any(x => x.eventid == newEvent.eventid))
            {
                return Conflict("Event with this eventid already exists!");
            }

            database.Gtd.Add(newEvent);
            database.SaveChanges();

            return newEvent;
        }
    }
}
