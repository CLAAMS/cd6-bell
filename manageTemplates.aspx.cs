﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Utilities;
using Tools;


namespace CD6{
    public partial class manageTemplates : System.Web.UI.Page{
        protected void Page_Load(object sender, EventArgs e){
            getTemplates();
        }

        protected void btnAddTemplate_Click(object sender, EventArgs e){
            string name, make, model, description;
            int templateID = 0;
            string procedure = null;

            name = txtTemplate.Text;
            make = txtMake.Text;
            model = txtModel.Text;
            description = txtDescription.Text;

            if (btnAddTemplate.Text == "Add Template") {
                procedure = "createTemplate";
            } else if (btnAddTemplate.Text == "Save Template") {
                templateID = (int)Session["assetTemplateID"];
                procedure = "updateTemplate";
            }

            if (procedure != null) {
                DBConnect objDB = new DBConnect();

                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = procedure;

                SqlParameter param_name = new SqlParameter("@name", name);
                param_name.Direction = ParameterDirection.Input;
                param_name.SqlDbType = SqlDbType.VarChar;
                param_name.Size = 100;
                objCommand.Parameters.Add(param_name);

                SqlParameter param_make = new SqlParameter("@make", make);
                param_make.Direction = ParameterDirection.Input;
                param_make.SqlDbType = SqlDbType.VarChar;
                param_make.Size = 100;
                objCommand.Parameters.Add(param_make);

                SqlParameter param_model = new SqlParameter("@model", model);
                param_model.Direction = ParameterDirection.Input;
                param_model.SqlDbType = SqlDbType.VarChar;
                param_model.Size = 100;
                objCommand.Parameters.Add(param_model);

                SqlParameter param_description = new SqlParameter("@description", description);
                param_description.Direction = ParameterDirection.Input;
                param_description.SqlDbType = SqlDbType.VarChar;
                param_description.Size = 100;
                objCommand.Parameters.Add(param_description);

                if (procedure == "updateTemplate") {
                    SqlParameter param_templateID = new SqlParameter("@assetTemplateID", templateID);
                    param_templateID.Direction = ParameterDirection.Input;
                    param_templateID.SqlDbType = SqlDbType.Int;
                    param_templateID.Size = 100;
                    objCommand.Parameters.Add(param_templateID);
                }

                objDB.DoUpdateUsingCmdObj(objCommand);
                objDB.CloseConnection();
            }

            blankTemplateFields();
            btnAddTemplate.Text = "Add Template";
            Session.Remove("assetTemplateID");
            getTemplates();
        }

        private void blankTemplateFields() {
            txtTemplate.Text = "";
            txtDescription.Text = "";
            txtMake.Text = "";
            txtModel.Text = "";
        }

        protected void getTemplates() {
            DataSet dsAssets = new DataSet();

            string selectTemplates = "select * from Asset_Template;";
            dsAssets = Tools.DBAccess.DBCall(selectTemplates);

            gvTemplates.DataSource = dsAssets;
            gvTemplates.DataBind();
        }

        protected void gvTemplates_RowCommand(object sender, GridViewCommandEventArgs e) {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvTemplates.Rows[index];
            int assetTemplateID = Convert.ToInt32(gvTemplates.DataKeys[index].Value);
            Session["assetTemplateID"] = assetTemplateID;
            if (e.CommandName == "delTemplate"){
                DBConnect DBObj = new DBConnect();
                SqlCommand commandObject = new SqlCommand();
                commandObject.CommandType = CommandType.StoredProcedure;
                commandObject.CommandText = "delAssetTemplate";

                SqlParameter param_assetTemplateID = new SqlParameter("@assetTemplateID", assetTemplateID);
                param_assetTemplateID.Direction = ParameterDirection.Input;
                param_assetTemplateID.SqlDbType = SqlDbType.Int;
                param_assetTemplateID.Size = 100;
                commandObject.Parameters.Add(param_assetTemplateID);

                DBObj.DoUpdateUsingCmdObj(commandObject);
                DBObj.CloseConnection();

                getTemplates();
            }

            if (e.CommandName == "editTemplate"){
                string name, make, model, description, sql;
                sql = string.Format("select Name, Make, Model, Description from Asset_Template where assetTemplateID = {0};", assetTemplateID);

                DataSet data = Tools.DBAccess.DBCall(sql);
                name = (string)data.Tables[0].Rows[0][0];
                make = (string)data.Tables[0].Rows[0][1];
                model = (string)data.Tables[0].Rows[0][2];
                description = (string)data.Tables[0].Rows[0][3];

                txtTemplate.Text = name;
                txtMake.Text = make;
                txtModel.Text = model;
                txtDescription.Text = description;

                btnAddTemplate.Text = "Save Template";
            }
        }
    }
}