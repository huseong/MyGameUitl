using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Script was written by huseong Lee.
/// with MIT License
/// You Need Singleton<T> and CSVParser
/// </summary>
/// This is enum of Pooling Object

namespace GameUtilSD {
    public enum Language {
        English, Korean // you can add new language
    }
    public class LanguageManager : Singleton<LanguageManager> {
        private Language _language;
        private Language language {
            get {
                return _language;
            }
            set {
                _language = value;
                _currentFont = _fontDic[_language];
            }
        }
        private Font _currentFont;
        private Dictionary<Language, Dictionary<string, string>> _languageDic;
        private Dictionary<Language, Font> _fontDic;
        private Dictionary<Language, string> _languagePath;
        private Dictionary<Language, string> _fontPath;
        public delegate void onChangeLanguage(LanguageManager languageManager);
        public onChangeLanguage onchangeLanguage;

        protected void Awake() {
            if (!isSingleton()) {
                return;
            }
            _languageDic = new Dictionary<Language, Dictionary<string, string>>();
            _fontDic = new Dictionary<Language, Font>();
        }

        private void Start() {
            onchangeLanguage(instance);
        }

        private void setLanguagePath() {

        }
        /// <summary>
        /// You have to add the Language's Path
        /// </summary>
        private void setFontPath() {

        }

        public void setLanguage(Language lauguage) {
            this.language = lauguage;
            onchangeLanguage(instance);
        }

        public string getText(string key) {
            string temp;
            if (_languageDic[_language].TryGetValue(key, out temp)) {
                return temp;
            }
            Debug.LogWarning("No value linked this Key : " + key);
            return "";
        }
    }
}

