﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Utilities;
using System.IO;
using System.Text;
using System.Collections;

namespace CD6 {
    public partial class sos_track : System.Web.UI.Page {
        SignOutSheet mySOS = new SignOutSheet();

        protected void Page_Load(object sender, EventArgs e) {
            String SqlConnectString = Global.Connection_String;

            DBConnect myDB = new DBConnect(SqlConnectString);
            lblTrackingDirections.Visible = true;
            lblTrackingDirections.Text = "Shown below are Sign Out Sheets that are due within the next 14 days from the current date, as well as overdue Sign Out Sheets (if applicable).";
            SqlConnection myConn = new SqlConnection(SqlConnectString);
            SqlCommand MyCommand = new SqlCommand();
            myConn.Open();

            MyCommand.Connection = myConn;
            MyCommand.CommandType = CommandType.StoredProcedure;
            MyCommand.CommandText = "sosTracking";

            gvSosTracking.DataSource = myDB.GetDataSetUsingCmdObj(MyCommand);
            gvSosTracking.DataBind();

            foreach (GridViewRow row in gvSosTracking.Rows) {
                if (int.Parse(row.Cells[3].Text) < 0) {
                    row.BackColor = System.Drawing.Color.FromName("red");
                    row.ForeColor = System.Drawing.Color.FromName("white");
                }
            }
        }

        protected void gvSosTracking_RowCommand(object sender, GridViewCommandEventArgs e) {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSosTracking.Rows[index];
            int sosID = (int)gvSosTracking.DataKeys[index].Value;

            if (e.CommandName == "view") {
                mySOS.sosID = Convert.ToInt32(gvSosTracking.Rows[index].Cells[0].Text);
                Session.Add("SOSID", mySOS.sosID);
                Response.Redirect("./sos_view.aspx");
            }
        }

        protected void linkExport_Click(object sender, EventArgs e) {
            string results = Tools.CSV.gvToCsv(gvSosTracking);
            string[] lines = results.Split('\n');

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=myfile.txt");
            Response.ContentType = "text/xml";
            foreach (string line in lines) {
                Response.Write(line + Environment.NewLine);
            }
            Response.Flush();
            Response.End();
        }
    }
}
