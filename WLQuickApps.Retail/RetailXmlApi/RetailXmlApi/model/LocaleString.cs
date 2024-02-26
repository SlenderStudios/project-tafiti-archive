using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace RetailXmlApi.model
{
    public class LocaleString
    {
        protected Language _Dialect = Language.AMERICAN_ENGLISH;
        Dictionary<Language, string> _LanguageList = new Dictionary<Language, string>();

        public LocaleString()
        {
        }
        public LocaleString(string str, Language lang)
        {
            AddTranslation(str, lang);
        }
        public void AddTranslation(string str, Language lang)
        {
            if (_LanguageList.ContainsKey(lang))
            {
                throw new Exception("key already exists in dictionary");
            }
            else
            {
                _LanguageList.Add(lang, str);
            }
        }
        public string GetTranslation(Language lang)
        {
            return _LanguageList[_Dialect];
        }
        public string Value
        {
            get
            {
                return _LanguageList[_Dialect];
            }
        }
        public Language Dialect
        {
            set
            {
                _Dialect = value;
            }
            get
            {
                return _Dialect;
            }
        }
        public override string ToString()
        {
            return this.Value;
        }
    }
}