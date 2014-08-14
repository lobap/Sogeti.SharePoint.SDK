//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace ExecutionModels.Common
{
    public static class PresenterUtilities
    {
        private static bool IsNotSystemColumn(string columnName)
        {
            return Constants.GetSystemColumns().All(colName => string.Compare(colName, columnName, StringComparison.Ordinal) != 0);
        }

        /// <summary>
        /// Common method to format a GridView by removing unwanted columns
        /// </summary>
        /// <param name="gridView"></param>
        public static void FormatGridDisplay(GridView gridView, DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                if (PresenterUtilities.IsNotSystemColumn(column.ColumnName))
                {
                    BoundField col = new BoundField();
                    col.DataField = column.ColumnName;
                    col.HeaderText = string.IsNullOrEmpty(column.Caption) ? column.ColumnName : column.Caption;
                    gridView.Columns.Add(col);
                }
            }
        }

    }
}
