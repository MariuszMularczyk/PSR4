using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Neo4jPSR
{

    public class Book
    {
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
        [JsonPropertyName("author")]
        public string author { get; set; }
        [JsonPropertyName("year")]
        public int year { get; set; }

    }
}
