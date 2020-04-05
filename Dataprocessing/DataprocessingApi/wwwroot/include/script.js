Chart.defaults.global.legend.display = false;

class ContentSchema {
    constructor(content, schema) {
        this.content = content;
        this.schema = schema;
    }
}

// Returns a promise with the results of a request.
function httpGET(url) {
    return new Promise((success, fail) => {
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open("GET", url, true);
        xmlHttp.setRequestHeader("accept", RequestType)
        xmlHttp.send(null);
        var res = null;
        xmlHttp.onload = function (e) {
            success(new ContentSchema(xmlHttp.responseText, xmlHttp.getResponseHeader("link")));
        };
    });
}

function getSchema(path) {
    return new Promise((success, fail) => {
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open("GET", path, false);
        xmlHttp.send(null);

        if (RequestType == "application/json") {
            console.debug(xmlHttp.responseText)
            success(JSON.parse(xmlHttp.responseText));
        }
        else if (RequestType == "application/xml") {
            success(xmlHttp.responseText);
        } else {
            fail();
        }
    })
}

var RequestType = "";
var Region = "US";
var State;
var InputDate;

var trendingvideos = null;
var trendingsongs = null;
var terrorismevents = null;

function FillSpotifyGraph() {
    // Spotify Chart.
    var ctx = document.getElementById('spotifychart').getContext('2d');

    var spotifychart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [],
            datasets: [{
                label: 'Top songs by streams',
                data: [],
                borderWidth: 1
            }]
        },
        options: {
            tooltips: {
                enabled: true
            },
            onClick: (c, i) => {
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

    trendingsongs.forEach(song => {
        spotifychart.data.labels.push(song.artist + " - " + song.trackname);
        spotifychart.data.datasets.forEach((dataset) => {
            dataset.data.push(song.streams);
        });
    });

    spotifychart.update(1000, false);
}

function FillYoutubeGraph() {
    // YouTube chart
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
    // YouTube chart
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
        if (key != "null") { // discard null values.
            console.log(types[key]);
            terr_graph.data.labels.push(key);
            terr_graph.data.datasets.forEach((dataset) => {
                dataset.data.push(types[key]);
            });
        }
    }

    terr_graph.update(1000, false);
}

function CheckDone() {
    if (trendingsongs != null && trendingvideos != null && terrorismevents != null) {
        State.innerHTML = "Done! :)";
        State.style.color = "Green";
    }
}

function start() {
    // Remove start button
    document.getElementById("startbtn").remove();
    document.getElementById("datepicker").disabled = true;
    document.getElementById("region").disabled = true;
    document.getElementById("reqtype").disabled = true;
    InputDate = document.getElementById("datepicker").value.split("-");
    Region = document.getElementById("region").value;
    console.debug(InputDate);
    State = document.getElementById("state");
    State.innerHTML = "Working...";
    State.style.color = "Cyan";

    // Getting request type
    var select = document.getElementById("reqtype");
    var requestType = select.options[select.selectedIndex].value;

    RequestType = requestType;

    // Do downloads
    DownloadTopSongs("us");
    DownloadTopVideos("us");
    DownloadTerrorismEvents("us");
}

function DownloadTerrorismEvents(region) {
    httpGET("/api/Terrorism?day=&year=" + InputDate[0] + "&region=" + Region)
        .then(function (resp) {
            console.debug(resp.content);
            console.debug(resp.schema)
            ValidateAndParse(resp).then((result) => {
                console.log(result[0]);

                if (typeof result !== "undefined") {
                    terrorismevents = result;
                    FillTerrorismGraph();
                } else {
                    terrorismevents = 1;
                    document.getElementById("noresults_terr").innerHTML = "Query unfortunately yielded no results.";
                }

                CheckDone();
            });
        });
}

function DownloadTopVideos(region) {
    httpGET("/api/Youtube?day=" + InputDate[2] + "&month=" + InputDate[1] + "&year=" + InputDate[0] + "&region=" + Region)
        .then(function (resp) {
            console.debug(resp.content);
            console.debug(resp.schema)
            ValidateAndParse(resp).then((result) => {

                if (typeof result !== "undefined") {
                    trendingvideos = result;
                    FillYoutubeGraph();
                } else {
                    trendingvideos = 1;
                    document.getElementById("topvideo").innerHTML = "Query unfortunately yielded no results.";
                }
                CheckDone();
            });
        });
}

function DownloadTopSongs(region) {
    httpGET("/api/Spotify?day=" + InputDate[2] + "&month=" + InputDate[1] + "&year=" + InputDate[0] + "&region=" + Region)
        .then(function (resp) {
            console.debug(resp.content);
            console.debug(resp.schema)

            ValidateAndParse(resp).then((result) => {

                if (typeof result !== "undefined") {
                    trendingsongs = result;
                    FillSpotifyGraph();
                } else {
                    trendingsongs = 1;
                    document.getElementById("topsong").innerHTML = "Query unfortunately yielded no results.";
                }
                CheckDone();
            });
        });
}

function ValidateAndParse(contentschema) {
    return new Promise((success, fail) => {
        if (RequestType == "application/json") {
            console.debug(contentschema.schema);
            var jobject = JSON.parse(contentschema.content);

            getSchema(contentschema.schema).then((result) => {
                // validate JSON
                var ajv = new Ajv();
                var valid = ajv.validate(result, jobject);

                if (!valid) {
                    console.debug("json not valid!!!");
                    console.debug(ajv.errors);
                    fail("invalid json");
                }
                else {
                    console.debug("json valid.");
                }

                console.debug(jobject);
                success(jobject);
            });
        }
        else if (RequestType == "application/xml") {
            getSchema(contentschema.schema).then((result) => {
                xmlschema(result).validate(contentschema.content).then((validation) => {
                    if (!validation.valid) {
                        console.debug("xml not valid!!!");
                        console.debug(validation.message);
                        fail("invalid xml");
                    }
                    else {
                        console.debug("xml valid.");
                    }

                    var result = xmljson.toJSON(contentschema.content);
                    result = result[Object.keys(result)[0]];
                    result = result[Object.keys(result)[1]];// Go down two levels of object bs
                    console.debug(result);
                    success(result);
                });
            });
        }
        else {
            fail("Invalid request type");
        }
    });
}