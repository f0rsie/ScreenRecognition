using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.Model
{
    public class MyMemoryJsonModel
    {
        public class Rootobject
        {
            public Responsedata responseData { get; set; }
            public bool quotaFinished { get; set; }
            public object mtLangSupported { get; set; }
            public string responseDetails { get; set; }
            public int responseStatus { get; set; }
            public string responderId { get; set; }
            public object exception_code { get; set; }
            public Match[] matches { get; set; }
        }

        public class Responsedata
        {
            public string translatedText { get; set; }
            public int match { get; set; }
        }

        public class Match
        {
            public string id { get; set; }
            public string segment { get; set; }
            public string translation { get; set; }
            public string source { get; set; }
            public string target { get; set; }
            public string quality { get; set; }
            public object reference { get; set; }
            public int usagecount { get; set; }
            public string subject { get; set; }
            public string createdby { get; set; }
            public string lastupdatedby { get; set; }
            public string createdate { get; set; }
            public string lastupdatedate { get; set; }
            public float match { get; set; }
        }
    }

    public class MyMemoryJsonModel1
    {
    //    {
    //    "responseData":
    //    {
    //    "translatedText":"\u0435\u0431\u0430\u0442\u044c","match":1},
    //    "quotaFinished":false,
    //    "mtLangSupported":null,
    //    "responseDetails":"",
    //    "responseStatus":200,
    //    "responderId":"7",
    //    "exception_code":null,
    //    "matches":[
    //        {
    //            "id":"438715030",
    //            "segment":"fuck",
    //            "translation":"\u0435\u0431\u0430\u0442\u044c",
    //            "source":"en-GB",
    //            "target":"ru-RU",
    //            "quality":"74",
    //            "reference":null,
    //            "usage-count":3,
    //            "subject":"All",
    //            "created-by":"MateCat",
    //            "last-updated-by":"MateCat",
    //            "create-date":"2016-03-18 10:51:16",
    //            "last-update-date":"2016-03-18 10:51:16",
    //            "match":1
    //        },
    //        {
    //            "id":"598269104",
    //            "segment":"fuck",
    //            "translation":"\u0431\u043b\u044f\u0434\u044c",
    //            "source":"en-US",
    //            "target":"ru-RU",
    //            "quality":"74",
    //            "reference":null,
    //            "usage-count":5,
    //            "subject":"All",
    //            "created-by":"MateCat",
    //            "last-updated-by":"MateCat",
    //            "create-date":"2022-02-16 00:32:42",
    //            "last-update-date":"2022-02-16 00:32:42",
    //            "match":0.99
    //        },
    //        {
    //            "id":"560659553",
    //            "segment":"Fuck",
    //            "translation":"\u00ab\u0411***\u044c\u00bb.",
    //            "source":"en-US",
    //            "target":"ru-RU",
    //            "quality":"74",
    //            "reference":null,
    //            "usage-count":2,
    //            "subject":"All",
    //            "created-by":"MateCat",
    //            "last-updated-by":"MateCat",
    //            "create-date":"2020-04-25 13:56:41",
    //            "last-update-date":"2020-04-25 13:56:41",
    //            "match":0.97
    //        }
    //    ]
    //}
    }
}
