class ContentSchema
{
    constructor(content, schema)
    {
        this.content = content;
        this.schema = schema;
    }
}

async function httpGET(url)
{
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("GET", url, false);
    xmlHttp.setRequestHeader("accept", RequestType)
    xmlHttp.send(null);
    return new ContentSchema(xmlHttp.responseText, xmlHttp.getResponseHeader("link"));
}

function getSchema(path)
{
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("GET", path, false);
    xmlHttp.send(null);

    if (RequestType == "application/json")
    {
        console.debug(xmlHttp.responseText)
        return JSON.parse(xmlHttp.responseText);
    }
    else if (RequestType == "application/xml")
    {
        return xmlHttp.responseText;
    }
}

var RequestType = "";
var State;
var Date;

async function start()
{
    // Remove start button
    document.getElementById("startbtn").remove();
    Date = document.getElementById("datepicker").value.split("-");
    console.debug(Date);
    State = document.getElementById("state");
    State.innerHTML = "Working...";

    // Getting request type
    var select = document.getElementById("reqtype");
    var requestType = select.options[select.selectedIndex].value;

    RequestType = requestType;

    // Do downloads
    DownloadTopSongs(1, 1, 2017, "us");
    State.innerHTML = "Done.";
}

async function DownloadTopSongs(day, month, year, region)
{
    var resp = await httpGET("/api/Spotify?day=" + Date[2] + "&month=" + Date[1] + "&year=" + Date[0] + "&region=" + region)
    console.debug(resp.content);
    console.debug(resp.schema)
    var topsongs = await ValidateAndParse(resp);
    console.log(topsongs[0].artist);

    // ugly thing
    document.getElementById("topsong")
        .innerHTML =
        "<iframe src=\"https://open.spotify.com/embed/track/" + topsongs[0].url.split('/')[4] + "\" width=\"400\" height=\"80\" frameborder=\"0\" allowtransparency=\"true\" allow=\"encrypted-media\"></iframe>";
}

async function ValidateAndParse(contentschema)
{
    if (RequestType == "application/json")
    {
        console.debug(contentschema.schema);
        var jobject = JSON.parse(contentschema.content);

        // validate JSON
        var ajv = new Ajv();
        var valid = ajv.validate(getSchema(contentschema.schema), jobject);

        if (!valid)
        {
            console.debug("json not valid!!!");
            console.debug(ajv.errors);
            return null;
        }
        else
        {
            console.debug("json valid.");
        }

        console.debug(jobject);
        return jobject;
    }
    else if (RequestType == "application/xml")
    {
        var sch = getSchema(contentschema.schema);
        console.debug(sch);
        var validation = await xmlschema(sch).validate(contentschema.content);
        if (!validation.valid) {
            console.debug("xml not valid!!!");
            console.debug(validation.message);
            return null;
        }
        else {
            console.debug("xml valid.");
        }
        // options for xml to json thingy
        

        var result = xmljson.toJSON(contentschema.content);
        result = result[Object.keys(result)[0]];
        result = result[Object.keys(result)[1]];// Go down two levels of object bs
        console.debug(result);
        return result;
    }
    else
    {
        console.error("Invalid request type");
        return null;
    }
}