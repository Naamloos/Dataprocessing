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
    /// YouTube controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/xml", "application/json")]
    [Produces("application/xml", "application/json")] // Restrict requests to XML and JSON
    public class YoutubeController : ControllerBase
    {
        const string JSON_SCHEMA = "/schemas/json/YoutubeTrendingVideo.json";
        const string XML_SCHEMA = "/schemas/xml/YoutubeTrendingVideo.xsd";
        const string JSON_ARRAY_SCHEMA = "/schemas/json/ArrayOfYoutubeTrendingVideo.json";
        const string XML_ARRAY_SCHEMA = "/schemas/xml/ArrayOfYoutubeTrendingVideo.xsd";

        private readonly Database database;

        /// <summary>
        /// Constructs a new YouTube controller
        /// </summary>
        /// <param name="database">database to use</param>
        public YoutubeController(Database database)
        {
            this.database = database.NewConnection();
        }

        /// <summary>
        /// Returns all top videos for a specific region on a specific date.
        /// </summary>
        /// <param name="day">Day of the month</param>
        /// <param name="month">Month of the year</param>
        /// <param name="year">Year</param>
        /// <param name="region">Region to return top songs for.</param>
        /// <param name="limit">OPTIONAL Max amount of songs to return.</param>
        /// <returns>An array of Spotify Trending Song objects.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<YoutubeTrendingVideo>> Get(int day, int month, int year, string region, int limit = 25)
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

            var date = new DateTime(year, month, day);

            var reg = region.ToUpper();

            return database.Youtube.OrderByDescending(x => x.Likes).Where(x => 
                x.CountryCode == reg
                && x.TrendingDate.Date == date)
                .Take(limit)? // Maximaal aantal returnen
                .ToList();
        }

        /// <summary>
        /// Deletes a trending video from teh Database.
        /// </summary>
        /// <param name="videoid">ID of video to remove.</param>
        /// <param name="day">Day the video was trending.</param>
        /// <param name="month">Month the video was trending.</param>
        /// <param name="year">Year the video was trending.</param>
        /// <param name="region">Region the video was trending in.</param>
        /// <returns>The video you just deleted.</returns>
        [HttpDelete]
        public ActionResult<YoutubeTrendingVideo> Delete(string videoid, int day, int month, int year, string region)
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

            var date = new DateTime(year, month, day);

            if (!database.Youtube.Any(x => 
                x.VideoId == videoid 
                && x.CountryCode.ToLower() == region.ToLower()
                && x.TrendingDate == date))
            {
                return Conflict("No such video to delete!");
            }

            var deletable = database.Youtube.First(x =>
                x.VideoId == videoid
                && x.CountryCode.ToLower() == region.ToLower()
                && x.TrendingDate == date);

            database.Youtube.Remove(deletable);
            database.SaveChanges();

            return deletable;
        }

        /// <summary>
        /// Updates a trending video in the database.
        /// </summary>
        /// <param name="updatedVideo">Video to update.</param>
        /// <returns>The old video you updated.</returns>
        [HttpPut]
        public ActionResult<YoutubeTrendingVideo> Put([FromBody]YoutubeTrendingVideo updatedVideo)
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

            if (!database.Youtube.Any(x =>
                x.VideoId == updatedVideo.VideoId
                && x.CountryCode.ToLower() == updatedVideo.CountryCode.ToLower()
                && x.TrendingDate == updatedVideo.TrendingDate))
            {
                return Conflict("No such video to update!");
            }

            var oldVideo = database.Youtube.First(x =>
                x.VideoId == updatedVideo.VideoId
                && x.CountryCode.ToLower() == updatedVideo.CountryCode.ToLower()
                && x.TrendingDate == updatedVideo.TrendingDate);

            database.Youtube.Update(updatedVideo);
            database.SaveChanges();

            return oldVideo;
        }

        /// <summary>
        /// Adds a new trending video to the database.
        /// </summary>
        /// <param name="newVideo">New video to add.</param>
        /// <returns>The video you added.</returns>
        [HttpPost]
        public ActionResult<YoutubeTrendingVideo> Post([FromBody]YoutubeTrendingVideo newVideo)
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
            if (database.Youtube.Any(x =>
                x.VideoId == newVideo.VideoId
                && x.CountryCode.ToLower() == newVideo.CountryCode.ToLower()
                && x.TrendingDate == newVideo.TrendingDate))
            {
                return Conflict("Video with this date/region/id combo already exists!");
            }

            database.Youtube.Add(newVideo);
            database.SaveChanges();

            return newVideo;
        }
    }
}