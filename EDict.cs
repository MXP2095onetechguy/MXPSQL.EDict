using System;
using System.IO;
using CsvHelper;
using System.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MXPSQL.EDict{

    // extensible dictionary
    public class ExtDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : ICloneable, IDisposable
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
            var strs = JsonConvert.SerializeObject(this, Formatting.Indented);

            return strs;
        }

        public string ToCsv(){
            string res = "";
            using(var swriter = new StringWriter())
            using (var csv = new CsvWriter(swriter, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(this);
            }

            return res;
        }

        public string ToCvs(){
            return ToCsv();
        }
    }
}