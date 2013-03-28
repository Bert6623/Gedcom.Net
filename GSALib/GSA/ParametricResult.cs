/********************************************************************************
 *  
 *  Product: GSALib
 *  Description: A C# API for accessing the Google Search Appliance.
 *
 *  (c) Copyright 2011 Michael Cizmar + Associates Ltd.  (MC+A)
 *  
********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GSALib.GSA
{
    /// <summary>
    /// Provides methods to create Parametric  objects
    /// <para>Author Michael CIzmar</para>
    /// </summary>
    [Serializable]
    public sealed class ParametricResult
    {
        #region Varaibles

        private String value;
        private String low;
        private String high;
        private int count;

        #endregion

        #region Contructor

        public ParametricResult()
        {
            
        }

        public ParametricResult(string name, string low, string high, int count)
        {
            this.value = name;
            this.low = low;
            this.high = high;
            this.count = count;
        }

        #endregion

        #region Get/Set Properties

        public String getValue()
        {
            return this.value ;
        }

        public void setValue(String name)
        {
            this.value = name;
        }

        public String getLow()
        {
            return this.low;
        }

        public void setLow(String low)
        {
            this.low = low;
        }
        public String getHigh()
        {
            return this.high;
        }

        public void setHigh(String high)
        {
            this.high = high;
        }
        public int getCount()
        {
            return this.count;
        }

        public void setCount(int count)
        {
            this.count = count;
        }
        
        #endregion
    }

    

}
