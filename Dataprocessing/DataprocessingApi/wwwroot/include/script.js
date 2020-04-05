Chart.defaults.global.legend.display = false;

// Class used for returning http body / schema header combo.
class ContentSchema
{
    constructor(content, schema)
    {
        this.content = content;
        this.schema = schema;
    }
}

// Input data vars
var RequestType = "";
var Region = "US";
var State;
var InputDate;

// Output data vars
var trendingvideos = null;
var trendingsongs = null;
var terrorismevents = null;

// Function to start crunching data.
function start() {

    // Remove start button
    document.getElementById("startbtn").remove();

    // Disable date picker, region picker and reqtype picker
    document.getElementById("datepicker").disabled = true;
    document.getElementById("region").disabled = true;
    document.getElementById("reqtype").disabled = true;

    // Get the date input and "parse" it
    InputDate = document.getElementById("datepicker").value.split("-");

    // Get the region input
    Region = document.getElementById("region").value;

    // Get the state text element
    State = document.getElementById("state");

    // Indicate the page is working (crunching the numbers)
    State.innerHTML = "Working...";
    State.style.color = "Cyan";

    // Getting request type from selector.
    var select = document.getElementById("reqtype");
    var requestType = select.options[select.selectedIndex].value;

    // Store request type.
    RequestType = requestType;

    // Start all download methods.
    // These start asynchronous downloads so they do not block the UI.
    DownloadTopSongs();
    DownloadTopVideos();
    DownloadTerrorismEvents();
}

// Returns a promise with the results of a request. The results contain content and a schema path.
function httpGET(url)
{
    // Return a newly created Promise
    return new Promise((success, fail) =>
    {
        // Do the request
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open("GET", url, true);
        xmlHttp.setRequestHeader("accept", RequestType)
        xmlHttp.send(null);

        var res = null;
        // Build event handler for onload
        xmlHttp.onload = function (e)
        {
            // Return results
            success(new ContentSchema(xmlHttp.responseText, xmlHttp.getResponseHeader("link")));
        };
    });
}

// Gets a parsed schema from a path.
function getSchema(path)
{
    return new Promise((success, fail) =>
    {
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open("GET", path, false);
        xmlHttp.send(null);

        if (RequestType == "application/json")
        {
            // We can jsut parse the json schema no issues there
            console.debug(xmlHttp.responseText)
            success(JSON.parse(xmlHttp.responseText));
        }
        else if (RequestType == "application/xml")
        {
            // The XML schema gets parsed later.
            success(xmlHttp.responseText);
        }
        else
        {
            // uh wat
            fail();
        }
    })
}

// Downloads the terrorism events from the API
function DownloadTerrorismEvents()
{
    // async request
    httpGET("/api/Terrorism?day=&year=" + InputDate[0] + "&region=" + Region)
        .then(function (resp) // success event handler
        {
            // validate and parse
            ValidateAndParse(resp).then((result) => // success event handler
            {
                // If not undefined, store result and fill graph
                if (typeof result !== "undefined")
                {
                    terrorismevents = result;
                    FillTerrorismGraph();
                }
                else
                {
                    // Give storage var a value to indicate it's done.
                    terrorismevents = 1;
                    // Tell user query yielded no results.
                    document.getElementById("noresults_terr").innerHTML = "Query unfortunately yielded no results.";
                }

                // Check whether all requests are done.
                CheckDone();
            });
        });
}

// Downloads the top videos
function DownloadTopVideos(region)
{
    // async get request
    httpGET("/api/Youtube?day=" + InputDate[2] + "&month=" + InputDate[1] + "&year=" + InputDate[0] + "&region=" + Region)
        .then(function (resp) // success event handler.
        {
            ValidateAndParse(resp).then((result) => // validate and parse, run following on success.
            {
                // if not undefined
                if (typeof result !== "undefined")
                {
                    // store result and fill graph
                    trendingvideos = result;
                    FillYoutubeGraph();
                }
                else
                {
                    // Set data on storage var to indicate we're done
                    trendingvideos = 1;
                    // Tell user: no results.
                    document.getElementById("topvideo").innerHTML = "Query unfortunately yielded no results.";
                }
                // Check whether we're done.
                CheckDone();
            });
        });
}

// Downloads spotify top songs
function DownloadTopSongs(region)
{
    // async request
    httpGET("/api/Spotify?day=" + InputDate[2] + "&month=" + InputDate[1] + "&year=" + InputDate[0] + "&region=" + Region)
        .then(function (resp)
        {
            // validate and parse
            ValidateAndParse(resp).then((result) =>
            {
                // if not undefined
                if (typeof result !== "undefined")
                {
                    // store result and fill graph
                    trendingsongs = result;
                    FillSpotifyGraph();
                }
                else
                {
                    // set a dummy value and tell user there's no results.
                    trendingsongs = 1;
                    document.getElementById("topsong").innerHTML = "Query unfortunately yielded no results.";
                }
                // Check whether we're done.
                CheckDone();
            });
        });
}

// validates a response with it's given headers.
function ValidateAndParse(contentschema)
{
    // build a new validation promise
    return new Promise((success, fail) =>
    {
        // JSON validation
        if (RequestType == "application/json")
        {
            // parse content to json
            var jobject = JSON.parse(contentschema.content);

            // get schema 
            getSchema(contentschema.schema).then((result) =>
            {
                // validate JSON with Ajv
                var ajv = new Ajv();
                var valid = ajv.validate(result, jobject);

                // if not valid, fail.
                if (!valid)
                {
                    console.debug("json not valid!!!");
                    console.debug(ajv.errors);
                    fail("invalid json");
                    return;
                }
                else
                {
                    console.debug("json valid.");
                }

                // return success.
                success(jobject);
            });
        }
        // xml validation and parsing
        else if (RequestType == "application/xml")
        {
            // Get schemas
            getSchema(contentschema.schema).then((result) =>
            {
                // validate xml
                xmlschema(result).validate(contentschema.content).then((validation) =>
                {
                    // if not valid
                    if (!validation.valid)
                    {
                        // fail with invalid xml
                        console.debug("xml not valid!!!");
                        console.debug(validation.message);
                        fail("invalid xml");
                        return;
                    }
                    else
                    {
                        console.debug("xml valid.");
                    }

                    // Parse xml to json object to have the exact same objects
                    var result = xmljson.toJSON(contentschema.content);
                    result = result[Object.keys(result)[0]];
                    result = result[Object.keys(result)[1]];// Go down two levels because weird
                    success(result);
                });
            });
        }
        else
        {
            fail("Invalid request type");
        }
    });
}

// Checks whether all queries are done and notifies the user.
function CheckDone()
{
    if (trendingsongs != null // trending songs store not null
        && trendingvideos != null // trending videos store not null
        && terrorismevents != null) // terrorism events store not null
    {
        // Change state to done
        State.innerHTML = "Done! :)";
        State.style.color = "Green";
    }
}

// Fills the spotify graph
function FillSpotifyGraph() {
    // Get canvas context
    var ctx = document.getElementById('spotifychart').getContext('2d');

    // Build chart
    var spotifychart = new Chart(ctx, {
        type: 'bar',
        data:
        {
            labels: [],
            datasets:
            [{
                label: 'Top songs by streams',
                data: [],
                borderWidth: 1
            }]
        },
        options:
        {
            tooltips:
            {
                enabled: true
            },


            // https://stackoverflow.com/questions/37122484/chart-js-bar-chart-click-events


            onClick: (c, i) =>
            {
                // onclick for song preview :)
                var e = i[0];
                console.log(e._index);
                var label = spotifychart.data.labels[e._index];
                console.log(label);
                // Display associated video as embed
                // find video with matching title
                var song = trendingsongs.find(v => v.artist + " - " + v.trackname == label);

                document.getElementById("topsong")
                    .innerHTML =
                    "<iframe src=\"https://open.spotify.com/embed/track/"
                    + song.url.split('/')[4]
                    + "\" width=\"400\" height=\"80\" frameborder=\"0\" allowtransparency=\"true\" allow=\"encrypted-media\"></iframe>";
            }
        }
    });

    // add all songs with stream count to graph
    trendingsongs.forEach(song => {
        spotifychart.data.labels.push(song.artist + " - " + song.trackname);
        spotifychart.data.datasets.forEach((dataset) => {
            dataset.data.push(song.streams);
        });
    });

    // force update
    spotifychart.update(1000, false);
}

function FillYoutubeGraph() {
    // YouTube chart, same as spotify
    ctx = document.getElementById('youtubechart').getContext('2d');

    var youtubechart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [],
            datasets: [{
                label: 'Top videos by likes',
                data: [],
                borderWidth: 1
            }]
        },
        options: {
            legend: {
                display: false
            },
            onClick: (c, i) => {
                var e = i[0];
                console.log(e._index);
                var label = youtubechart.data.labels[e._index];
                console.log(label);
                // Display associated video as embed
                // find video with matching title
                var video = trendingvideos.find(v => v.title == label);
                document.getElementById("topvideo")
                    .innerHTML =
                    "<iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/" + video.videoid +
                    "\" frameborder =\"0\" allow=\"accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>";
            }
        }
    });

    trendingvideos.forEach(video => {
        youtubechart.data.labels.push(video.title);
        youtubechart.data.datasets.forEach((dataset) => {
            dataset.data.push(video.likes);
        });
    });

    youtubechart.update(1000, false);
}

function FillTerrorismGraph() {
    // Terrorism graph, almost same as other two.
    ctx = document.getElementById('terroristchart').getContext('2d');

    var terr_graph = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [],
            datasets: [{
                label: 'Amount of times this weapon was used this year',
                data: [],
                borderWidth: 1
            }]
        },
        options: {
            legend: {
                display: false
            }
        }
    });

    var types = [];

    terrorismevents.forEach(ev => {
        // db contains three weaptypes so we'll check it thrice
        if (types[ev.weaptype1_txt] == null) {

            types[ev.weaptype1_txt] = 1;
        } else {
            types[ev.weaptype1_txt] += 1;
        }


        if (types[ev.weaptype2_txt] == null) {

            types[ev.weaptype2_txt] = 1;
        } else {
            types[ev.weaptype2_txt] += 1;
        }


        if (types[ev.weaptype3_txt] == null) {

            types[ev.weaptype3_txt] = 1;
        } else {
            types[ev.weaptype3_txt] += 1;
        }

    })

    for (var key in types) {
        if (key != "null" && key != "") { // discard null values.
            console.log(types[key]);
            terr_graph.data.labels.push(key);
            terr_graph.data.datasets.forEach((dataset) => {
                dataset.data.push(types[key]);
            });
        }
    }

    terr_graph.update(1000, false);
}