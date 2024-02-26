using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Calendar
{
    public enum ContentLineType
    {
        None,
        Component,
        EndComponent,
        Property,
        Parameter
    }

    public class CalendarReader
    {

        #region Member Variables

        // internal reader
        private TextReader _reader;

        // current state
        private ContentLineType _contentLineType;
        private string _name;
        private string _value;

        #endregion

        #region property/parameters
        private KeyValuePair<string, string> _property;
        private int _curParmIdx;
        private List<KeyValuePair<string, string>> _parameters;

        #endregion

        #region Constructors

        public CalendarReader(string url)
        {
            try
            {
                Uri uri = new Uri(url, UriKind.RelativeOrAbsolute);

                // clean the URI (is it absolute and is it set)
                if (!uri.IsAbsoluteUri && uri.OriginalString.Length > 0)
                {
                    uri = new Uri(Path.GetFullPath(url));
                }

                if (uri.IsFile)
                {
                    InitCalendarReader(File.OpenText(uri.LocalPath));
                }
                else
                {
                    WebRequest request = HttpWebRequest.Create(uri);

                    WebResponse response = request.GetResponse();

                    InitCalendarReader(new StreamReader(response.GetResponseStream()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to read calendar.", ex);
            }
        }

        public CalendarReader(TextReader input)
        {
            InitCalendarReader(input);
        }

        #endregion

        #region Properties

        public ContentLineType ContentLineType
        {
            get { return _contentLineType; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Value
        {
            get { return _value; }
        }

        #endregion


        #region Methods

        #region Public Methods

        public bool Read()
        {
            string line;
            if ((line = _reader.ReadLine()) != null)
            {
                // unfold multiple line representations
                while (IsWhitespace((char)_reader.Peek())) // space or tab
                {
                    _reader.Read();
                    line += _reader.ReadLine();
                }

                ParseLine(line);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetParameter(int index)
        {
            if (_parameters != null &&
                _parameters.Count > 0 && index < _parameters.Count)
            {
                return _parameters[_curParmIdx].Value;
            }
            else
            {
                return null;
            }
        }

        public string GetParameter(string name)
        {
            if (_parameters != null)
            {
                for (int i = 0; i < _parameters.Count; i++)
                {
                    if (_parameters[i].Key == name)
                        return GetParameter(i);
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public bool MoveToParameter(int index)
        {
            // save the current property values
            if (_contentLineType == ContentLineType.Property)
            {
                _property = new KeyValuePair<string, string>(_name, _value);
            }

            if (_parameters != null &&
                _parameters.Count > 0 && index < _parameters.Count)
            {
                _curParmIdx = index;
                _contentLineType = ContentLineType.Parameter;
                _name = _parameters[_curParmIdx].Key;
                _value = _parameters[_curParmIdx].Value;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MoveToParameter(string name)
        {
            if (_parameters != null)
            {
                for (int i = 0; i < _parameters.Count; i++)
                {
                    if (_parameters[i].Key == name)
                        return MoveToParameter(i);
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool MoveToFirstParameter()
        {
            return MoveToParameter(0);
        }

        public bool MoveToNextParameter()
        {
            return MoveToParameter(++_curParmIdx);
        }

        public bool MoveToProperty()
        {
            if (_contentLineType != ContentLineType.Parameter)
            {
                return false;
            }
            else
            {
                _contentLineType = ContentLineType.Property;
                _name = _property.Key;
                _value = _property.Value;
                return true;
            }
        }

        public void Close()
        {
            _reader.Close();
            _contentLineType = ContentLineType.None;
            _name = null;
            _value = null;
            _curParmIdx = -1;
            _property = new KeyValuePair<string, string>();
            _parameters = null;
        }

        #endregion

        #region Private Methods

        private void InitCalendarReader(TextReader input)
        {
            _reader = input;
            _contentLineType = ContentLineType.None;
            ResetParameters();
        }

        private void ParseLine(string line)
        {
            ResetParameters();

            int colonIdx = line.IndexOf(':');
            string name = line.Substring(0, colonIdx);
            string value = line.Substring(colonIdx + 1);

            if ("BEGIN".Equals(name))
            {
                _contentLineType = ContentLineType.Component;
                _name = value;
                _value = string.Empty;
            }
            else if ("END".Equals(name))
            {
                _contentLineType = ContentLineType.EndComponent;
                _name = value;
                _value = string.Empty;
            }
            else
            {
                _contentLineType = ContentLineType.Property;

                int semiIdx = name.IndexOf(';');
                if (semiIdx != -1)
                {
                    _name = name.Substring(0, semiIdx);
                    ParseParameters(name.Substring(semiIdx + 1));
                }
                else
                {
                    _name = name;
                }

                _value = value
                    .Replace("\\n", "\n")   // new line
                    .Replace("\\N", "\n")   // new line
                    .Replace("\\;", ";")    // semicolon
                    .Replace("\\,", ",")    // comma
                    .Replace("\\\\", "\\"); // backslash
            }
        }

        private void ParseParameters(string parms)
        {
            // parameter array
            string[] parmArray = parms.Split(';');

            // create a new parameter dictionary
            _parameters = new List<KeyValuePair<string, string>>(parmArray.Length);

            foreach (string p in parmArray)
            {
                int equalIdx = p.IndexOf('=');
                string name = p.Substring(0, equalIdx);
                string value = p.Substring(equalIdx + 1);

                value = value
                    .Trim("\"".ToCharArray())   // remove leading/trailing quotes
                    .Replace("\\n", "\n")       // new line
                    .Replace("\\N", "\n")       // new line
                    .Replace("\\:", ":")        // colon
                    .Replace("\\;", ";")        // semicolon
                    .Replace("\\,", ",")        // comma
                    .Replace("\\\\", "\\");     // backslash

                _parameters.Add(new KeyValuePair<string, string>(name, value));
            }
        }

        private void ResetParameters()
        {
            _curParmIdx = -1;
            _parameters = null;
        }

        private bool IsWhitespace(char ch)
        {
            if (ch == '\t' || ch == ' ')
                return true;
            else
                return false;
        }
        #endregion

        #endregion

    }
}