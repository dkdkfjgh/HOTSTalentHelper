using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using agi = HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace HOTSTalentHelper
{
    class HeroAnalyzer
    {
        public Hero SelectedHero = new Hero();
        public bool Analyze(string HeroKoreanName)
        {
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string getString = client.DownloadString("https://api.hotslogs.com/Public/Data/Heroes");


            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            dynamic obj = serializer.Deserialize(getString, typeof(object));

            List<Hero> HeroesList = new List<Hero>();
            Hero hero = new Hero();
            foreach (var items in obj)
            {
                hero.PrimaryName = items.PrimaryName;
                hero.ImageURL = items.ImageURL;
                hero.AttributeName = items.AttributeName;
                hero.Group = items.Group;
                hero.SubGroup = items.SubGroup;
                hero.Translations = items.Translations;
                HeroesList.Add(hero.Clone() as Hero);
            }

            string HeroinKorean;
            string PrimaryName = "없음";


            HeroinKorean = HeroKoreanName;





            foreach (Hero item in HeroesList)
            {

                if (item.Translations.Contains(HeroinKorean) == true)
                {
                    PrimaryName = item.PrimaryName;
                    SelectedHero = item.Clone() as Hero;
                    break;
                }
            }

            if (PrimaryName == "없음")
            {

                return false;
            }

            string URL = "https://www.hotslogs.com/Sitewide/HeroDetails?Hero=" + PrimaryName;

            getString = client.DownloadString(URL);
            agi.HtmlDocument doc = new agi.HtmlDocument();
            doc.LoadHtml(getString);

            HtmlNode nodes = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_MainContent_RadGridHeroTalentStatistics_ctl00\"]/tbody");
            Talent talent = new Talent();

            foreach (var node in nodes.ChildNodes)
            {
                if (node.ChildNodes.Count == 0) continue;
                foreach (var item in node.ChildNodes)
                {
                    if (item.InnerText.Contains("Level") == true)
                    {
                        Console.WriteLine("{0}", item.InnerText);
                        talent.Level = item.InnerText;
                    }
                    if (item.InnerHtml.Contains("png") == true)
                    {

                        //Console.WriteLine("Talent Image Link : {0}", item.OuterHtml);



                        Regex regex = new Regex(@"(?:\/\/.*\.(?:png|jpg))");
                        Match match = regex.Match(item.OuterHtml);
                        talent.TalentImageUrl = "http:" + match.Value;

                        talent.Name = item.NextSibling.InnerText;
                        if(talent.Name.Length > 13)
                        {
                            if(talent.Name.Contains(" "))
                            {
                                talent.Name = ReplaceFirstOccurrence(talent.Name, " ", "\n");

                            }

                        }


                        talent.Desc = item.NextSibling.NextSibling.InnerText;
                        talent.Desc = SpliceText(talent.Desc, 20);
                        talent.Popularity = item.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                        if (talent.Popularity.Contains("nbsp") == true) talent.Popularity = "0.0 %";
                        talent.Winrate = item.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                        if (talent.Winrate.Contains("nbsp") == true) talent.Winrate = "? %";
                        SelectedHero.Tal.Add(talent.Clone() as Talent);
                        //talent = null;
                    }
                }
            }

            return true;
        }

        public int TalentCountbyNum(int level)
        {
            int cnt = 0;

            foreach (var tal in SelectedHero.Tal)
            {
                if (tal.Level == "Level: " + level.ToString())
                {
                    cnt++;
                }
            }

            return cnt;
        }
        private static string SpliceText(string text, int lineLength)
        {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
        }
        private static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.IndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }
    }



    public sealed class DynamicJsonConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return type == typeof(object) ? new DynamicJsonObject(dictionary) : null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(object) })); }
        }

        #region Nested type: DynamicJsonObject

        private sealed class DynamicJsonObject : DynamicObject
        {
            private readonly IDictionary<string, object> _dictionary;

            public DynamicJsonObject(IDictionary<string, object> dictionary)
            {
                if (dictionary == null)
                    throw new ArgumentNullException("dictionary");
                _dictionary = dictionary;
            }

            public override string ToString()
            {
                var sb = new StringBuilder("{");
                ToString(sb);
                return sb.ToString();
            }

            private void ToString(StringBuilder sb)
            {
                var firstInDictionary = true;
                foreach (var pair in _dictionary)
                {
                    if (!firstInDictionary)
                        sb.Append(",");
                    firstInDictionary = false;
                    var value = pair.Value;
                    var name = pair.Key;
                    if (value is string)
                    {
                        sb.AppendFormat("{0}:\"{1}\"", name, value);
                    }
                    else if (value is IDictionary<string, object>)
                    {
                        new DynamicJsonObject((IDictionary<string, object>)value).ToString(sb);
                    }
                    else if (value is ArrayList)
                    {
                        sb.Append(name + ":[");
                        var firstInArray = true;
                        foreach (var arrayValue in (ArrayList)value)
                        {
                            if (!firstInArray)
                                sb.Append(",");
                            firstInArray = false;
                            if (arrayValue is IDictionary<string, object>)
                                new DynamicJsonObject((IDictionary<string, object>)arrayValue).ToString(sb);
                            else if (arrayValue is string)
                                sb.AppendFormat("\"{0}\"", arrayValue);
                            else
                                sb.AppendFormat("{0}", arrayValue);

                        }
                        sb.Append("]");
                    }
                    else
                    {
                        sb.AppendFormat("{0}:{1}", name, value);
                    }
                }
                sb.Append("}");
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (!_dictionary.TryGetValue(binder.Name, out result))
                {
                    // return null to avoid exception.  caller can check for null this way...
                    result = null;
                    return true;
                }

                result = WrapResultObject(result);
                return true;
            }

            public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
            {
                if (indexes.Length == 1 && indexes[0] != null)
                {
                    if (!_dictionary.TryGetValue(indexes[0].ToString(), out result))
                    {
                        // return null to avoid exception.  caller can check for null this way...
                        result = null;
                        return true;
                    }

                    result = WrapResultObject(result);
                    return true;
                }

                return base.TryGetIndex(binder, indexes, out result);
            }

            private static object WrapResultObject(object result)
            {
                var dictionary = result as IDictionary<string, object>;
                if (dictionary != null)
                    return new DynamicJsonObject(dictionary);

                var arrayList = result as ArrayList;
                if (arrayList != null && arrayList.Count > 0)
                {
                    return arrayList[0] is IDictionary<string, object>
                        ? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)))
                        : new List<object>(arrayList.Cast<object>());
                }

                return result;
            }
        }

        #endregion
    }
    public sealed class Hero : ICloneable
    {
        public string PrimaryName;
        public string ImageURL;
        public string AttributeName;
        public string Group;
        public string SubGroup;
        public string Translations;

        public object Clone()
        {
            return new Hero()
            {
                PrimaryName = this.PrimaryName,
                ImageURL = this.ImageURL,
                AttributeName = this.AttributeName,
                Group = this.Group,
                SubGroup = this.SubGroup,
                Translations = this.Translations,

            };

        }
        public List<Talent> Tal = new List<Talent>();
    }
    public sealed class Talent : ICloneable
    {
        public string Level = ""; //n특
        public string Name = ""; //이름
        public string Desc = ""; //설명
        public string Popularity = ""; //선호도
        public string Winrate = ""; //승률
        public string TalentImageUrl = "";
        public object Clone()
        {
            return new Talent()
            {
                Level = this.Level,
                Name = this.Name,
                Desc = this.Desc,
                Popularity = this.Popularity,
                Winrate = this.Winrate,
                TalentImageUrl = this.TalentImageUrl
            };
        }
    }
}
