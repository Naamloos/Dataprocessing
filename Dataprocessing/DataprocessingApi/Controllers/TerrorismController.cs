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
        const string XML_SCHEMA = "/schemas/xml/TerrorismEvent.xml";
        const string JSON_ARRAY_SCHEMA = "/schemas/json/ArrayOfTerrorismEvent.json";
        const string XML_ARRAY_SCHEMA = "/schemas/xml/ArrayOfTerrorismEvent.xml";

        private Database database;
        private IsoCountries iso; // This one is for translating ISO countries.

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
                return Conflict("No such top song to delete!");
            }

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
                return Conflict("No such song with this Region/date/postion combo to update!");
            }

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
                return Conflict("Song with this Region/date/postion combo already exists!");
            }

            database.Gtd.Add(newEvent);
            database.SaveChanges();

            return newEvent;
        }
    }
}
