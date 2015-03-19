﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CD6{
    public partial class asset : System.Web.UI.Page
    {
        Asset objAsset = new Asset();
        AssetFunctions objAssetFunctions = new AssetFunctions();
        protected void Page_Load(object sender, EventArgs e) {
            btnSubmitModifyAsset.Visible = false;
           
            search_results.Visible=false;
            submit_button.Visible=true;
            asset_form.Visible=true;
            search_button.Visible=false;
            searchHeader.Visible=false;
            createHeader.Visible=true;
            history.Visible=false;
            modifyHeader.Visible=false;
            templateRow.Visible=true;
            lblSerialLeft.Visible=true;
            txtSerialLeft.Visible=true;
            filler.Visible=false;
            lblSerialRight.Visible=false;
            txtSerialRight.Visible=false;
            if(!Page.IsPostBack){
                populateTemplateDropdown();
            }
            if (ddlAssetTemplate.Items[0].Text == "Dell Laptop" && ddlStatus.Items[0].Text == "Active")
            {
                ddlAssetTemplate.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlAssetTemplate.SelectedIndex = 0;

                ddlStatus.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                ddlStatus.SelectedIndex = 0;
            }
        }

        private void populateTemplateDropdown() {
            DataSet dsAssets = new DataSet();

            string selectTemplates = "select assetTemplateID, Name from Asset_Template;";
            dsAssets = Tools.DBAccess.DBCall(selectTemplates);
            ddlAssetTemplate.DataSource = dsAssets;
            ddlAssetTemplate.DataTextField = dsAssets.Tables[0].Columns[1].ColumnName;
            ddlAssetTemplate.DataValueField = dsAssets.Tables[0].Columns[0].ColumnName;
            ddlAssetTemplate.DataBind();
        }

        private String[] getTemplate(int templateID) {
            string[] template = new string[3];
            string sql = string.Format("select Make, Model, Description from Asset_Template where assetTemplateID = {0};", templateID);
            DataSet data = Tools.DBAccess.DBCall(sql);
            template[0] = (string)data.Tables[0].Rows[0][0].ToString();
            template[1] = (string)data.Tables[0].Rows[0][1].ToString();
            template[2] = (string)data.Tables[0].Rows[0][2].ToString();
            return template;
        }

        protected void gvSearchResult_click(object sender, GridViewCommandEventArgs e)
        {
            search_results.Visible = false;
            submit_button.Visible = true;
            asset_form.Visible = true;
            search_button.Visible = false;
            searchHeader.Visible = false;
            createHeader.Visible = false;
            history.Visible = true;
            modifyHeader.Visible = true;
            templateRow.Visible = false;

            
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSearchResults.Rows[index];

            if (e.CommandName == "deleteRecord")
            {
                string submit_type;
                int assetID = Convert.ToInt32(gvSearchResults.DataKeys[index].Value);
                objAsset.assetID = assetID;
                objAssetFunctions.DeleteAsset(objAsset);
                btnSearch_Click(this, e);
                submit_type = "archive";

                string dialog_header, dialog_body;
                if (submit_type == "archive")
                {
                    objAsset.Make = gvSearchResults.Rows[index].Cells[2].Text;
                    objAsset.Model = gvSearchResults.Rows[index].Cells[3].Text;
                    dialog_header = "Asset Archived";
                    dialog_body = string.Format("{0} {1} has been archived successfully and status is Out Of Service.", objAsset.Make, objAsset.Model);
                    modal(dialog_header, dialog_body);
                }

            }
            else if (e.CommandName == "modifyRecord")
            {
                createHeader.Visible = false;
                modifyHeader.Visible = true;

                btnSubmit.Visible = false;
                btnSubmitModifyAsset.Visible = true;

                int assetID = Convert.ToInt32(gvSearchResults.DataKeys[index].Value);


                ////Set the Object values to the gridview
                objAsset.assetID = assetID;
                objAsset.CLATag = gvSearchResults.Rows[index].Cells[1].Text;
                objAsset.Make = gvSearchResults.Rows[index].Cells[2].Text;
                objAsset.Model = gvSearchResults.Rows[index].Cells[3].Text;
                objAsset.SerialNumber = gvSearchResults.Rows[index].Cells[4].Text;
                objAsset.Status = gvSearchResults.Rows[index].Cells[5].Text;
                objAsset.Description = gvSearchResults.Rows[index].Cells[6].Text;
                objAsset.Notes = gvSearchResults.Rows[index].Cells[7].Text;
                objAsset.recordCreated = DateTime.Now;
                objAsset.recordModified = DateTime.Now;

                txtCLAID.Text = objAsset.CLATag;
                try
                {
                    int.Parse(objAsset.CLATag);
                }
                catch (Exception ex)
                {

                }
                txtMake.Text = objAsset.Make;
                txtModel.Text = objAsset.Model;
                txtSerialLeft.Text = objAsset.SerialNumber;
                ddlStatus.Text = objAsset.Status;
                txtDescription.Text = objAsset.Description;
                txtNotes.Text = objAsset.Notes;
                lblAssetID.Text = objAsset.assetID.ToString();

            }
        }
        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string submit_type;

            btnSubmit.Visible = true;
            btnSubmitModifyAsset.Visible = false;
            objAsset.CLATag = txtCLAID.Text;
            objAsset.Make = txtMake.Text;
            objAsset.Model = txtModel.Text;
            objAsset.Description = txtDescription.Text;
            objAsset.SerialNumber = txtSerialLeft.Text;
            objAsset.Status = ddlStatus.SelectedValue;
            objAsset.Notes = txtNotes.Text;
            objAsset.recordCreated = DateTime.Now;
            objAsset.recordModified = DateTime.Now;
            //try
            //{
            //    int.Parse(objAsset.CLATag);
            //}
            //catch (Exception ex)
            //{
            //    Response.Redirect("asset.aspx");
            //    lblInputValidation.Text = "You have put in an invalid claTag for the asset. Please try again";
            //}
            //if (objAsset.Make == " ")
            //{
            //    Response.Redirect("asset.aspx");
            //    lblInputValidation.Text = "You have put in an invalid Make for the asset.Please try again";
            //}
            // if (objAsset.Model == " ")
            //{
            //    Response.Redirect("asset.aspx");
            //    lblInputValidation.Text = "You have put in an invalid Model for the asset.Please try again";
            //}


            // if (objAsset.SerialNumber == " ")
            // {
            //     Response.Redirect("asset.aspx");
            //     lblInputValidation.Text = "You have put in an invalid serial number for the asset.Please try again";
            // }


            // else
            // {
            //     try
            //     {
            //         int.Parse(objAsset.SerialNumber);
                    
            //     }
             
            //     catch (Exception ex)
            //     {
            //         Response.Redirect("asset.aspx");
            //         lblInputValidation.Text = "You have put in a non-integer serialnumber for the asset.Please try again";
            //     }
            // }

            // if (objAsset.Description == " ")
            // {
            //     Response.Redirect("asset.aspx");
            //     lblInputValidation.Text = "You have put in an invalid description. Please try again";
            // }

            // if (objAsset.Notes == " ")
            // {
            //     Response.Redirect("asset.aspx");
            //     lblInputValidation.Text = "You have put in invalid Notes. Please try again";
            // }
            
                objAssetFunctions.CreateNewAsset(objAsset);

                txtCLAID.Text = "";
                txtMake.Text = "";
                txtModel.Text = "";
                txtDescription.Text = "";
                txtSerialLeft.Text = "";
                ddlStatus.Text = "";
                txtNotes.Text = "";

                submit_type = "create";

                string dialog_header, dialog_body;
                if (submit_type == "create")
                {
                    dialog_header = "Asset Created";
                    dialog_body = string.Format("{0} {1} has been created successfully.", objAsset.Make, objAsset.Model );
                    modal(dialog_header, dialog_body);
                }
            
                
            }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            btnSubmit.Visible = false;
            btnSubmitModifyAsset.Visible = false;
            search_results.Visible = true;
            submit_button.Visible = false;
            search_button.Visible = false;
            asset_form.Visible = false;
            history.Visible = false;
            modifyHeader.Visible = false;
            templateRow.Visible = false;

            objAsset.assetID = 0;
            objAsset.CLATag = txtCLAID.Text;
            objAsset.Make = txtMake.Text;
            objAsset.Model = txtModel.Text;
            objAsset.Description = txtDescription.Text;
            objAsset.SerialNumber = txtSerialRight.Text;
            objAsset.Status = ddlStatus.SelectedValue;
            objAsset.Notes = txtNotes.Text;

            DataSet assetDataSource = objAssetFunctions.SearchForAssets(objAsset);
            gvSearchResults.DataSource = assetDataSource;
            
        }

        protected void btnNewSearch_Click(object sender, EventArgs e){
            btnSubmit.Visible = false;
            btnSubmitModifyAsset.Visible = false;
            search_results.Visible=false;
            submit_button.Visible=false;
            asset_form.Visible=true;
            search_button.Visible=true;
            searchHeader.Visible=true;
            createHeader.Visible=false;
            history.Visible=false;
            modifyHeader.Visible=false;
            txtNotes.Rows=1;
            txtDescription.Rows=1;
            templateRow.Visible=false;
            lblSerialLeft.Visible=false;
            txtSerialLeft.Visible=false;
            filler.Visible=true;
            lblSerialRight.Visible=true;
            txtSerialRight.Visible=true;


            txtCLAID.Text = "";
            txtMake.Text = "";
            txtModel.Text = "";
            txtDescription.Text = "";
            txtSerialLeft.Text = "";
            ddlStatus.Text = "";
            txtNotes.Text = "";
        }

        protected void btnCreate_Click(object sender, EventArgs e){
            btnSubmitModifyAsset.Visible = false;
            btnSubmit.Visible = true;
            search_results.Visible=false;
            submit_button.Visible=true;
            asset_form.Visible=true;
            search_button.Visible=false;
            searchHeader.Visible=false;
            createHeader.Visible=true;
            history.Visible=false;
            modifyHeader.Visible=false;
            templateRow.Visible=true;
            lblSerialLeft.Visible=true;
            txtSerialLeft.Visible=true;
            filler.Visible=false;
            lblSerialRight.Visible=false;
            txtSerialRight.Visible=false;
        }

        protected void btnSubmitModifyAsset_Click(object sender, EventArgs e)
        {
            string submit_type;

            btnSubmit.Visible = false;

            objAsset.assetID = Convert.ToInt32(lblAssetID.Text);
            objAsset.CLATag = txtCLAID.Text;
            objAsset.Make = txtMake.Text;
            objAsset.Model = txtModel.Text;
            objAsset.SerialNumber = txtSerialLeft.Text;
            objAsset.Status = ddlStatus.SelectedValue;
            objAsset.Description = txtDescription.Text;
            objAsset.Notes = txtNotes.Text;
            objAsset.recordCreated = DateTime.Now;
            objAsset.recordModified = DateTime.Now;



            DataSet ds = Tools.DBAccess.DBCall(string.Format("select sosID from Asset where assetID = {0}", objAsset.assetID));
            int sosID = 0;
            if (int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out sosID))
            {
	            objAsset.sosID = sosID;
	            objAssetFunctions.ModifyAsset(objAsset);
            } 
            else 
            {
	            objAsset.sosID = sosID;
	            objAssetFunctions.ModifyAsset(objAsset);
            }

            submit_type = "modify";

            string dialog_header, dialog_body;
            if (submit_type == "modify")
            {
                dialog_header = "Asset Modified";
                dialog_body = string.Format("{0} {1} has been modified successfully.", objAsset.Make, objAsset.Model);
                modal(dialog_header, dialog_body);
            }
            btnSearch_Click(this, e);
        }

        protected void ddlAssetTemplate_SelectedIndexChanged(object sender, EventArgs e) {
            int assetTemplateID = Convert.ToInt32(ddlAssetTemplate.SelectedValue);
            string[] template = getTemplate(assetTemplateID);
            txtMake.Text = template[0];
            txtModel.Text = template[1];
            txtDescription.Text = template[2];
        }

        protected void modal(string title, string body)
        {
            this.Master.modal_header = title;
            this.Master.modal_body = body;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
    }
}      


