using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace DriverService.Tests
{
    public class BsonAssert
    {
        public static void Equal(object expected, BsonDocument actual)
        {
            // Assert => EventData matches
            var settings = new JsonWriterSettings
            {
                OutputMode = JsonOutputMode.Shell,
                Indent = false
            };

            var docAsJson = actual.ToJson(settings);

            var actualCleaned = Regex.Replace(docAsJson, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

            Assert.Equal(Newtonsoft.Json.JsonConvert.SerializeObject(expected), actualCleaned);
        }
    }
}
