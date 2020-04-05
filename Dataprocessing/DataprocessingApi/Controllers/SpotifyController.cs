// Keeping as a sample.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHelper;
using DatabaseHelper.Models;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace DataprocessingApi.Controllers
{
    /// <summary>
    /// Spotify Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/xml", "application/json")]
    [Produces("application/xml", "application/json")] // Restrict requests to XML and JSON
    public class SpotifyController : ControllerBase
    {
        const string JSON_SCHEMA = "/schemas/json/SpotifyTrendingSong.json";
        const string XML_SCHEMA = "/schemas/xml/SpotifyTrendingSong.xsd";
        const string JSON_ARRAY_SCHEMA = "/schemas/json/ArrayOfSpotifyTrendingSong.json";
        const string XML_ARRAY_SCHEMA = "/schemas/xml/ArrayOfSpotifyTrendingSong.xsd";

        private readonly Database database;

        /// <summary>
        /// Creates a new SpotifyController
        /// </summary>
        /// <param name="database">database to use.</param>
        public SpotifyController(Database database)
        {
            this.database = database.NewConnection();
        }

        /// <summary>
        /// Gets all trending songs for a region on a specific day.
        /// </summary>
        /// <param name="day">Day of month</param>
        /// <param name="month">Month of year</param>
        /// <param name="year">Year</param>
        /// <param name="region">Region</param>
        /// <param name="limit">OPTIONAL Max amounts of songs to return</param>
        /// <returns>A list of songs</returns>
        [HttpGet]
        public ActionResult<IEnumerable<SpotifyTrendingSong>> Get(int day, int month, int year, string region, int limit = 25)
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

            // create datetime object from request values
            var date = new DateTime(year, month, day);

            // net zo mooi als de gemiddelde SQL query.
            return database.SpotifyData
                .Where(x => // get all objects where
                x.Date == date // the date matches
                && x.Region.ToLower() == region.ToLower() // the region matches
                && x.Position <= limit) // Position is within the limit
                ?.ToList(); // And return a list (to fix some compilation errors)
        }

        /// <summary>
        /// Deletes a top song.
        /// </summary>
        /// <param name="position">Position the song is on.</param>
        /// <param name="day">Day the song was trending.</param>
        /// <param name="month">Month the song was trending.</param>
        /// <param name="year">Year the song was trending.</param>
        /// <param name="region">Region the song was trending in.</param>
        /// <returns>The song you deleted.</returns>
        [HttpDelete]
        public ActionResult<SpotifyTrendingSong> Delete(int position, int day, int month, int year, string region)
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

            // make date object from request values
            var date = new DateTime(year, month, day);

            // If there's no such object to delete, return error
            if (!database.SpotifyData.Any(x => 
                x.Position == position 
                && x.Region == region
                && x.Date == date))
            {
                return Conflict("No such top song to delete!");
            }

            // Find the value to delete
            var deletable = database.SpotifyData.First(x =>
                x.Position == position
                && x.Region == region
                && x.Date == date);

            // and delete it
            database.SpotifyData.Remove(deletable);
            database.SaveChanges();

            // give it back to the user too.
            return deletable;
        }

        /// <summary>
        /// Updates a song in the database.
        /// </summary>
        /// <param name="updatedSong">The new trending song.</param>
        /// <returns>Returns the old song object on success.</returns>
        [HttpPut]
        public ActionResult<SpotifyTrendingSong> Put([FromBody]SpotifyTrendingSong updatedSong)
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

            // if no such value to update, return an error.
            if (!database.SpotifyData.Any(x =>
                x.Position == updatedSong.Position
                && x.Date == updatedSong.Date
                && x.Region == updatedSong.Region))
            {
                return Conflict("No such song with this Region/date/postion combo to update!");
            }

            // Get the old value
            var oldSong = database.SpotifyData.First(x =>
                x.Position == updatedSong.Position
                && x.Date == updatedSong.Date
                && x.Region == updatedSong.Region);

            // Update with the new value
            database.SpotifyData.Update(updatedSong);
            database.SaveChanges();

            // Return the old value
            return oldSong;
        }

        /// <summary>
        /// Posts a new song to the database if no song with that position/date/ragion combo exists yet. Else returns 409.
        /// </summary>
        /// <param name="newSong">New song to add</param>
        /// <returns>Returns the new song on success.</returns>
        [HttpPost]
        public ActionResult<SpotifyTrendingSong> Post([FromBody]SpotifyTrendingSong newSong)
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
            if (database.SpotifyData.Any(x => 
                x.Position == newSong.Position 
                && x.Date == newSong.Date
                && x.Region == newSong.Region))
            {
                return Conflict("Song with this Region/date/postion combo already exists!");
            }

            database.SpotifyData.Add(newSong);
            database.SaveChanges();

            return newSong;
        }
    }
}
