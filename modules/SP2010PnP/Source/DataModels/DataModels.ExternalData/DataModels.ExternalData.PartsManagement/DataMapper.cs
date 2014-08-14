//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace DataModels.ExternalData.PartsManagement
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public class DataMapper<T>
    {
        private DataTable dataTable;
        private int rowNumber;

        public DataMapper(DataTable dataTable)
        {
            this.dataTable = dataTable;
            this.rowNumber = 0;
        }

        private DataMapper(DataTable dataTable,int rowNumber)
        {
            this.dataTable = dataTable;
            this.rowNumber = rowNumber;
        }

        public T Instance
        {
            get
            {
                var dataMapperInstance = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertyInfos = typeof(T).GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    // Set the Properties on the instance           
                    propertyInfo.SetValue(dataMapperInstance, dataTable.Rows[rowNumber][propertyInfo.Name], null);
                }
                return dataMapperInstance; 
            }
        }


        public List<T> Collection
        {
            get
            {
                var dataMapperCollection = new List<T>();
                int i = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    dataMapperCollection.Add(new DataMapper<T>(dataTable,i).Instance);
                    i++;
                }
                return dataMapperCollection;
            }
        }
    }
}
