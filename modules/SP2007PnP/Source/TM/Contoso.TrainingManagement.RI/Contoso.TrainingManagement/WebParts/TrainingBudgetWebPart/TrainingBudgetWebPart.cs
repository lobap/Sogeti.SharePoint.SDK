//===============================================================================
// Microsoft patterns & practices
// SharePoint Guidance version 2.0
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Contoso.AccountingManagement.BusinessEntities;

namespace Contoso.TrainingManagement.WebParts
{
    /// <summary>
    /// The TrainingBudgetWebPart displays information about
    /// the budget for a department.
    /// </summary>
    [Guid("1e98f121-e107-4301-854a-0db8ba88385b")]
    public class TrainingBudgetWebPart : WebPart, ITrainingBudgetView
    {
        #region Private Fields

        private Panel layout;
        private Label totalBudget;
        private Label remainingBudget;
        private DropDownList costCenters;
        private GridView transactions;
        private TrainingBudgetPresenter presenter;

        #endregion

        #region Methods

        protected override void OnInit(EventArgs e)
        {
            this.EnsureChildControls();
            base.OnInit(e);

            presenter = new TrainingBudgetPresenter(this);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            layout = new Panel();
            totalBudget = new Label();
            totalBudget.Attributes.Add("name", "TotalBudget");
            remainingBudget = new Label();
            remainingBudget.Attributes.Add("name", "RemainingBudget");
            costCenters = new DropDownList();
            costCenters.Attributes.Add("name", "CostCenters");
            costCenters.AutoPostBack = true;
            costCenters.SelectedIndexChanged += new EventHandler(costCenters_SelectedIndexChanged);

            transactions = new GridView();
            transactions.Attributes.Add("name", "Transactions");

            Label label = new Label();
            label.Text = "Cost Center: ";
            layout.Controls.Add(label);
            layout.Controls.Add(costCenters);
            layout.Controls.Add(new LiteralControl("<br />"));

            label = new Label();
            label.Text = "Total Budget: ";
            layout.Controls.Add(label);
            layout.Controls.Add(totalBudget);
            layout.Controls.Add(new LiteralControl("<br />"));

            label = new Label();
            label.Text = "Remaining Budget: ";
            layout.Controls.Add(label);
            layout.Controls.Add(remainingBudget);
            layout.Controls.Add(new LiteralControl("<br />"));

            label = new Label();
            label.Text = "Transactions";
            layout.Controls.Add(label);
            layout.Controls.Add(new LiteralControl("<br />"));

            layout.Controls.Add(transactions);

            this.Controls.Add(layout);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if ( !Page.IsPostBack )
            {
                presenter.SetCostCenterSource();
                costCenters.DataBind();
                presenter.SetTransactionSource();
                transactions.DataBind();
            }            
        }

        void costCenters_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.SetTransactionSource();
            transactions.DataBind();
        }

        #endregion

        #region ITrainingBudgetView Members

        public string CostCenter
        {
            get
            {
                return costCenters.SelectedValue;
            }
        }

        public IList<string> CostCenters
        {
            set
            {
                costCenters.DataSource = value;
            }
        }

        public IList<Transaction> Transactions
        {            
            set
            {
                transactions.DataSource = value;
            }
        }

        public float TotalBudget
        {
            set
            {
                totalBudget.Text = value.ToString("C");
            }
        }

        public float RemainingBudget
        {
            set
            {
                remainingBudget.Text = value.ToString("C");
            }
        }

        #endregion
    }
}
