using System;
using System.IO;
using CsvHelper;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Globalization;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MXPSQL.EDict{
    public class CvsStuff{
        public CsvConfiguration cvscfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            NewLine = Environment.NewLine,
            PrepareHeaderForMatch = args => args.Header.ToLower(),
            Comment = '#'
        };
    }
    // extensible dictionary
    public class ExtDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : ICloneable, IDisposable, IFormattable
    {
        // original code
        /*
            public CloneableDictionary<TKey, TValue> Clone()
                {
                CloneableDictionary<TKey, TValue> clone = new CloneableDictionary<TKey, TValue>();
                foreach (KeyValuePair<TKey, TValue> kvp in this)
                    {
                    clone.Add(kvp.Key, (TValue) kvp.Value.Clone());
                    }
                return clone;
                }
        */

        public readonly CvsStuff cvss = new CvsStuff();

        // new one
        public ExtDictionary<TKey, TValue> New()
        {
            ExtDictionary<TKey, TValue> clone = new ExtDictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> kvp in this)
            {
                clone.Add(kvp.Key, (TValue) kvp.Value.Clone());
            }
            return clone;
        }

        // extended code
        public ExtDictionary<TKey, TValue> Clone(){
            return New();
        }

        // dispose method
        public void Dispose(){
            this.Clear();
        }

        public void Delete(){
            Dispose();
        }

        // convert from other to this
        public void ConvertFromDictionary(Dictionary<TKey, TValue> dict){
            foreach (KeyValuePair<TKey, TValue> kvp in dict){
                this.Add(kvp.Key, (TValue) kvp.Value.Clone());
            }
        }

        public void ConvertFromKeyValuePair(KeyValuePair<TKey, TValue> kvpx){
            var list = new List<KeyValuePair<TKey, TValue>>{kvpx};
            var dict = list.ToDictionary(x => x.Key, x => x.Value);
            // dict.Add(kvpx.Key, kvpx.Value.Clone());
            /* foreach (KeyValuePair<TKey, TValue> kvp in kvpx){
                this.Add(kvp.Key, (TValue) kvp.Value.Clone());
            } */
            ConvertFromDictionary(dict);
        }

        public void ConvertFromJson(string json){
            Dictionary<TKey, TValue> dict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(json);

            ConvertFromDictionary(dict);
        }

        /* public void ConvertFromCsv(string csvtr){
            string res = "";
            IEnumerable<T> IEBuffer;
            ILookup<string, Order> Lookup;
            using(var sreader = new StringReader(res))
            using (var csv = new CsvReader(sreader, cvss.cvscfg))
            {
                // csv.WriteRecords(this);
                IEBuffer = csv.GetRecords<T>;
                Lookup = IEBuffer.ToLookup(o => o.CustomerName);
            }

            this = Lookup.ToDictionary(g => g.Key);
        } */

        /* public void ConvertFromCvs(string cvstr){
            ConvertFromCsv(cvstr);
        } */

        // convert from this to others
        public Dictionary<TKey, TValue> ToDictionary(){
            Dictionary<TKey, TValue> NewDict = new Dictionary<TKey, TValue>();
            ExtDictionary<TKey, TValue> edict = this.New();
            foreach(KeyValuePair<TKey, TValue> kvp in edict){
                NewDict.Add(kvp.Key, (TValue) kvp.Value.Clone());
            }
            return NewDict;
        }

        public List<KeyValuePair<TKey, TValue>> ToLKVP(){
            var list = this.ToList<KeyValuePair<TKey, TValue>>();

            return list;
        }

        public string ToJson(){
            var strs = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);

            return strs;
        }

        public string ToCsv(){
            string res = "";

            using(var swriter = new StringWriter()){
                using (var csv = new CsvWriter(swriter, cvss.cvscfg))
                {
                    csv.WriteRecords(this);
                }
                res = swriter.ToString();
            }

            return res;
        }

        public string ToCvs(){
            return ToCsv();
        }

        public string ToXML(){
            string json = ToJson();
            XNode node = JsonConvert.DeserializeXNode(json, "Root");
            return node.ToString();
        }

        public string ToXml(){
            return ToXML();
        }

        public override string ToString(){
            return ToCvs();
        }
    }
}