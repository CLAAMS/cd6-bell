﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CD6{
    public partial class login : System.Web.UI.Page{
        protected void Page_Load(object sender, EventArgs e){
            
        }

        protected void btnLogin_Click(object sender, EventArgs e){
            if (LDAP.AuthenticateUser(txtUsername.Text, txtPassword.Text) == txtUsername.Text){
                Session["user"] = txtUsername.Text;
                Response.Redirect("asset.aspx");
            }
        }
    }
}