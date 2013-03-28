/********************************************************************************
 *  
 *  Product: GSALib
 *  Description: A C# API for accessing the Google Search Appliance.
 *
 *  (c) Copyright 2008 Michael Cizmar + Associates Ltd.  (MC+A)
 *  
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace GSALib.Constants
{
    /// <summary>
    /// Encoding class provides encoding values supported by GSA
    /// <para>Author Albert Ghukasyan</para>    
    /// </summary>
    public sealed class Encoding
    {
        #region Constant Variables

        /// <summary>
        /// Provides Chinese (Simplified) Encoding ^GB2312
        /// </summary>
        public const string GB2312 = "GB2312";

        /// <summary>
        /// Provides Chinese (Traditional) Encoding ^Big5 
        /// </summary>
        public const string Big5 = "Big5";

        /// <summary>
        /// Provides Unicode (All Languages) Encoding ^UTF-8
        /// </summary>
        public const string UTF8 = "UTF-8";       

        /// <summary>
        /// Provides Russian Encoding ^ISO-8859-5
        /// </summary>
        public const string cyrillic = "cyrillic";
        
        /// <summary>
        /// Provides Danish,Dutch,English,Finnish,French,German,Icelandic,Italian,Norwegian,Portuguese,Spanish,Swedish Encoding ^ISO-8859-1
        /// </summary>
        public const string latin1 = "latin1";

        /// <summary>
        /// Provides Czech,Hungarian,Polish,Romanian Encoding ^ISO-8859-2
        /// </summary>
        public const string latin2 = "latin2";

        /// <summary>
        /// Provides Turkish Encoding ^ISO-8859-3
        /// </summary>
        public const string latin3 = "latin3";

        /// <summary>
        /// Provides Estonian,Latvian,Lithuanian Encoding ^ISO-8859-4
        /// </summary>
        public const string latin4 = "latin4";

        /// <summary>
        /// Provides Turkish Encoding ^ISO-8859-9
        /// </summary>
        public const string latin5 = "latin5";

        /// <summary>
        /// Provides Greek Encoding ^ISO-8859-7
        /// </summary>
        public const string greek = "greek";

        /// <summary>
        /// Provides Hebrew Encoding ^ISO-8859-8
        /// </summary>
        public const string hebrew = "hebrew";

        /// <summary>
        /// Provides Japanese Encoding ^Shift_JIS
        /// </summary>
        public const string sjis = "sjis";

        /// <summary>
        /// Provides Japanese Encoding ^ISO-2022-JP
        /// </summary>
        public const string jis = "jis";

        /// <summary>
        /// Provides Japanese Encoding ^EUC-JP
        /// </summary>
        public const string eucjp = "euc-jp";

        /// <summary>
        /// Provides Korean Encoding ^EUC-KR
        /// </summary>
        public const string euckr = "euc-kr";

        #endregion
    }
}
