﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MXPSQL.EDict{
    // extensible dictionary
    public class ExtDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : ICloneable
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
    }
}