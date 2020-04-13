using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.businesscentral
{
    public partial class Translations
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("value")]
        public List<Value> Value { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("translationCode")]
        public long TranslationCode { get; set; }

        [JsonProperty("originalText")]
        public string OriginalText { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("translatedText")]
        public string TranslatedText { get; set; }
    }

}
