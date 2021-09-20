MXPSQL.EDict


Extended Dictionary, basically You can clone it, convert it to normal dictionary or key value pair, convert from dictionary(No key value pair sorry).





void New(), Clone() for cloning

void ConvertFromDictionary(Dictionary<TKey, TValue> dict), void ConvertFromKeyValuePair(KeyValuePair<TKey, TValue> kvpx) the conversion to this type

Dictionary<TKey, TValue> ToDictionary() convert to normal dictionary