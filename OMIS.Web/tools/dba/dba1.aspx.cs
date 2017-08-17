using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using OMIS.DBUtility;

public partial class tools_dba1 : System.Web.UI.Page
{
    protected const int Mysql = 1;
    protected const int Mssql = 2;
    protected const int Oracle = 3;
    protected int DBTYPE = 1;


    protected string DBConnString = "";

    protected DataSet dsField = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DBConnString = this.GetDBConnString();
    }

    #region  GetDBConnString
    protected string GetDBConnString()
    {
        string host = this.txtHost.Text.Trim();
        string port = this.txtPort.Text.Trim();
        string user = this.txtUser.Text.Trim();
        string pwd = this.txtPwd.Text.Trim();
        string database = this.txtDatabase.Text.Trim();
        string charset = this.txtCharset.Text.Trim();

        string text = "host={0};port={1};user id={2};password={3};database={4};allow zero datetime=true;charset={5};";
        return String.Format(text, host, port, user, pwd, database, charset);
    }
    #endregion

    #region  ConnectDB
    protected void btnConnect_Click(object sender, EventArgs e)
    {
        try
        {
            this.DBConnString = this.GetDBConnString();

            this.GetTable();
        }
        catch (Exception ex) { this.divError.InnerHtml = ex.Message; }
    }
    #endregion

    #region  GetTable
    protected void GetTable()
    {
        try
        {
            this.ddlTable.Items.Clear();

            this.ClearData();

            DataSet dsTable = MysqlHelper.ExecuteDataSet(this.DBConnString, "show tables;");
            if (dsTable != null && dsTable.Tables[0] != null && dsTable.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsTable.Tables[0].Rows)
                {
                    ListItem li = new ListItem(dr[0].ToString(), dr[0].ToString());
                    this.ddlTable.Items.Add(li);
                }
                this.lblTable.Text = String.Format("Total of <b>{0}</b> data tables. ", dsTable.Tables[0].Rows.Count);
            }
            this.ddlTable.Items.Insert(0, new ListItem("Please select table", ""));

            string tableName = this.ddlTable.SelectedValue.ToString();

            this.GetField(tableName);
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  GetFieldData
    public DataSet GetFieldData()
    {
        try
        {
            this.dsField = null;

            string tableName = this.ddlTable.SelectedValue.ToString().Trim();
            if (!tableName.Equals(string.Empty))
            {
                string sql = String.Format("show full fields from {0};", tableName);
                DataSet ds = MysqlHelper.ExecuteDataSet(this.DBConnString, sql);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    //保存DataSet
                    this.dsField = ds;
                }
            }
        }
        catch (Exception ex) { this.divError.InnerHtml = ex.Message; }

        return this.dsField;
    }
    #endregion


    #region  GetTableComment
    public string GetTableComment(string tableName)
    {
        try
        {
            this.dsField = null;

            string dbName = this.txtDatabase.Text.Trim();
            if (tableName.Equals(string.Empty))
            {
                tableName = this.ddlTable.SelectedValue.ToString().Trim();
            }
            if (!tableName.Equals(string.Empty))
            {
                string sql = String.Format("use information_schema;select table_comment from TABLES where TABLE_SCHEMA='{0}' and TABLE_NAME='{1}';", dbName, tableName);
                return MysqlHelper.ExecuteScalar(this.DBConnString, sql).ToString();
            }
            return "";
        }
        catch (Exception ex) { return ""; }
    }
    #endregion

    #region  GetField
    public void GetField(string tableName)
    {
        try
        {
            bool isBuild = false;

            this.lblField.Text = "";

            this.GetFieldData();

            if (this.dsField != null)
            {
                this.gvField.DataSource = this.dsField.Tables[0];
                this.gvField.DataBind();

                string className = this.ParseTableName(tableName, "Info");
                this.txtClassName.Text = className;

                string classDesc = this.GetTableComment(tableName);
                this.txtClassComment.Text = classDesc;

                this.BuildFieldModel(this.dsField, className, classDesc);

                this.lblField.Text = String.Format("Total of <b>{0}</b> fields. ", this.dsField.Tables[0].Rows.Count);

                isBuild = true;
            }
            else
            {
                this.gvField.DataSource = null;
                this.gvField.DataBind();
            }
        }
        catch (Exception ex) { this.divError.InnerHtml = ex.Message; }
    }

    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetField(this.ddlTable.SelectedValue.ToString());
    }
    #endregion

    #region  ClearData
    public void ClearData()
    {
        this.divError.InnerHtml = "";
        this.lblField.Text = "";
        this.txtRowsId.Text = "";

        this.txtModel.Text = "";
        this.txtConstructor.Text = "";
        this.txtFillData.Text = "";

        this.txtAddSql.Text = "";
        this.txtUpdateSql.Text = "";
        this.txtAddSqlParam.Text = "";
        this.txtUpdateSqlParam.Text = "";
        this.txtJsonBuild.Text = "";
        this.txtJsonFormat.Text = "";

        this.txtAddSqlString.Text = "";
        this.txtAddSqlStringBatch.Text = "";

        this.txtAddMethod.Text = "";
        this.txtUpdateMethod.Text = "";
        this.txtDelMethod.Text = "";
    }
    #endregion

    #region  BuildModel
    public void BuildFieldModel(DataSet ds, string className, string classDesc)
    {
        try
        {
            if (ds == null)
            {
                ds = this.GetFieldData();
            }
            if (ds == null || ds.Tables[0] == null)
            {
                return;
            }

            string rowIds = this.txtRowsId.Text.Trim();
            string[] rowIdList = rowIds.Split(',');
            bool isFilter = rowIds.Length > 0;

            int rowCount = ds.Tables[0].Rows.Count;

            this.DBTYPE = Convert.ToInt32(this.ddlDbType.SelectedValue);

            #region  Filter Row
            if (isFilter)
            {
                int tc = ds.Tables[0].Rows.Count;
                for (int i = tc - 1; i >= 0; i--)
                {
                    bool isKeep = false;
                    foreach (string rid in rowIdList)
                    {
                        if (Convert.ToInt32(rid) == i)
                        {
                            isKeep = true;
                            break;
                        }
                    }
                    if (!isKeep && ds.Tables[0].Rows[i]["Extra"].ToString() != "auto_increment")
                    {
                        ds.Tables[0].Rows[i].Delete();
                        rowCount--;
                    }
                }
            }
            #endregion

            StringBuilder classInfo = new StringBuilder();
            classInfo.Append(String.Format("#region  {0}信息\r\n\r\n", classDesc));
            classInfo.Append("/// <summary>\r\n");
            classInfo.Append(String.Format("/// {0}信息\r\n", classDesc));
            classInfo.Append("/// </summary>\r\n");
            classInfo.Append(String.Format("public class {0}\r\n", className));
            classInfo.Append("{\r\n\r\n");

            StringBuilder property = new StringBuilder();
            property.Append("#region  字段属性\r\n\r\n");

            StringBuilder constructor = new StringBuilder();
            constructor.Append("#region  构造函数\r\n");
            constructor.Append(String.Format("public {0}()\r\n{{\r\n", className));

            StringBuilder fill = new StringBuilder();
            #region  填充数据
            bool isShowFillAll = !this.chbOnlyField.Checked;
            string infoName = this.txtInfoName.Text.Trim();
            string convertFunc = this.txtConvertFunc.Text.Trim();
            if (isShowFillAll)
            {
                fill.Append("#region  填充数据\r\n");
                fill.Append(String.Format("public {0} Fill{1}(DataRow dr)\r\n", className, className));
                fill.Append("{\r\n");
                fill.Append("try{\r\n");
                fill.Append(String.Format("\t{0} {1} = new {2}();\r\n\r\n", className, infoName, className));
            }
            #endregion

            string tableName = this.ddlTable.SelectedValue.ToString();
            DBA_PriKeyInfo priKey = new DBA_PriKeyInfo();
            bool hasPriKey = false;

            string keyName = "";
            string classField = this.ParseTableName(tableName, "");

            string dbconn = this.txtDBConnString.Text.Trim();
            string dbconn1 = this.txtDBConnString1.Text.Trim();

            StringBuilder strGet1 = new StringBuilder();
            StringBuilder strGet2 = new StringBuilder();

            StringBuilder strGet11 = new StringBuilder();
            StringBuilder strGet22 = new StringBuilder();

            StringBuilder strGet3 = new StringBuilder();
            StringBuilder strGet33 = new StringBuilder();

            #region  SQL Edit
            StringBuilder sqlAdd = new StringBuilder();
            StringBuilder sqlAdd1 = new StringBuilder();

            StringBuilder sqlA = new StringBuilder();
            StringBuilder sqlAF = new StringBuilder();
            StringBuilder sqlAV = new StringBuilder();

            StringBuilder sqlUpdate = new StringBuilder();
            StringBuilder sqlUpdate1 = new StringBuilder();

            StringBuilder sqlU = new StringBuilder();
            StringBuilder sqlUV = new StringBuilder();
            StringBuilder sqlUF = new StringBuilder();

            StringBuilder sqlDel = new StringBuilder();
            StringBuilder sqlDel1 = new StringBuilder();
            #endregion

            #region  SQL Param
            StringBuilder sqlAParam = new StringBuilder();
            //因排除了主键，字段数量有变化，放在最后 插入数据
            /*
            sqlAParam.Append(String.Format("int c = {0};\r\nint n = 0;\r\n", rowCount));
            sqlAParam.Append("MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[c];\r\n");
            */
            StringBuilder sqlAParam1 = new StringBuilder();
            StringBuilder sqlAParam1N = new StringBuilder();
            StringBuilder sqlAParam1V = new StringBuilder();

            StringBuilder sqlUParam = new StringBuilder();
            sqlUParam.Append(String.Format("int c = {0};\r\nint n = 0;\r\n", rowCount));
            sqlUParam.Append("MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[c];\r\n");

            StringBuilder sqlUParam1 = new StringBuilder();
            StringBuilder sqlUParam1N = new StringBuilder();
            StringBuilder sqlUParam1V = new StringBuilder();
            #endregion

            StringBuilder jsonFormat = new StringBuilder();
            jsonFormat.Append("{");

            StringBuilder jsonBuild = new StringBuilder();
            jsonBuild.Append("{");

            int n = 0;
            int kn = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }
                string rowSeparate = n > 0 && (n + 1) % 10 == 0 && n < rowCount - 1 ? "\r\n" : "";
                string fieldSeparate = n > 0 ? "," : "";

                string field = dr["Field"].ToString();
                string fieldName = this.ParseFieldName(field);
                string comment = this.ParseFieldComment(dr["Comment"].ToString());
                string fieldType = this.ParseFieldType(dr["Type"].ToString());

                bool isPriKey = false;

                #region  判断是否为主键
                if (dr["Extra"].ToString().Equals("auto_increment"))
                {
                    priKey = new DBA_PriKeyInfo();
                    priKey.field = field;
                    priKey.fieldType = fieldType;
                    priKey.fieldName = fieldName;
                    kn = 1;

                    isPriKey = true;
                    keyName = field;

                    hasPriKey = true;
                }
                #endregion


                #region  生成属性、变量
                property.Append(String.Format("/// <summary>\r\n/// {0}\r\n/// </summary>\r\n", comment));
                property.Append(String.Format("public {0} {1} {{ get; set; }}\r\n\r\n", fieldType, fieldName));
                #endregion

                #region  构造函数
                constructor.Append(String.Format("\tthis.{0} = {1};\r\n", fieldName, this.BuildDefaultValue(fieldType)));
                constructor.Append(rowSeparate);
                #endregion

                #region  填充数据
                fill.Append(isShowFillAll ? "\t" : "");
                fill.Append(String.Format("{0}.{1} = {2};\r\n", infoName, fieldName, this.ConvertData(fieldType, field, convertFunc)));
                fill.Append(rowSeparate);
                #endregion



                #region  新增的SQL
                if (!isPriKey)
                {
                    if (((n - kn) > 0 && (n - kn) % 10 == 0))
                    {
                        sqlAF.Append("\");\r\nsql.Append(\"");
                        sqlAV.Append("\");\r\nsql.Append(\"");
                    }
                    if ((n - kn) > 0)
                    {
                        sqlAF.Append(",");
                        sqlAV.Append(",");
                    }
                    switch (DBTYPE)
                    {
                        case Mysql:
                            sqlAF.Append(String.Format("`{0}`", field));
                            sqlAV.Append(String.Format("?{0}", fieldName));
                            break;
                        case Mssql:
                            sqlAF.Append(String.Format("[{0}]", field));
                            sqlAV.Append(String.Format("@{0}", fieldName));
                            break;
                        case Oracle:
                            sqlAF.Append(String.Format("\\\"{0}\\\"", field));
                            sqlAV.Append(String.Format(":{0}", fieldName));
                            break;
                    }
                }
                #endregion

                #region  更新的SQL
                if (!isPriKey)
                {
                    if ((n - kn) > 0 && (n - kn) % 5 == 0)
                    {
                        sqlUV.Append("\");\r\nsql.Append(\"");
                    }
                    sqlUV.Append((n - kn) > 0 ? "," : "");
                    switch (DBTYPE)
                    {
                        case Mysql:
                            sqlUV.Append(String.Format("`{0}` = ?{1}", field, fieldName));
                            break;
                        case Mssql:
                            sqlUV.Append(String.Format("[{0}] = @{1}", field, fieldName));
                            break;
                        case Oracle:
                            sqlUV.Append(String.Format("\\\"{0}\\\" = :{1}", field, fieldName));
                            break;
                    }
                }
                #endregion

                #region  SQL参数
                if (!isPriKey)
                {
                    switch (DBTYPE)
                    {
                        case Mysql:
                            sqlAParam.Append(String.Format("param[n++] = new MySql.Data.MySqlClient.MySqlParameter(\"?{0}\", {1}.{2});\r\n", fieldName, infoName, fieldName));
                            sqlUParam.Append(String.Format("param[n++] = new MySql.Data.MySqlClient.MySqlParameter(\"?{0}\", {1}.{2});\r\n", fieldName, infoName, fieldName));
                            break;
                        case Mssql:
                            sqlAParam.Append(String.Format("param[n++] = new System.Data.SqlClient.SqlParameter(\"@{0}\", {1}.{2});\r\n", fieldName, infoName, fieldName));
                            sqlUParam.Append(String.Format("param[n++] = new System.Data.SqlClient.SqlParameter(\"@{0}\", {1}.{2});\r\n", fieldName, infoName, fieldName));
                            break;
                        case Oracle:
                            sqlAParam.Append(String.Format("param[n++] = new System.Data.OracleClient.OracleParameter(\":{0}\", {1}.{2});\r\n", fieldName, infoName, fieldName));
                            sqlUParam.Append(String.Format("param[n++] = new System.Data.OracleClient.OracleParameter(\":{0}\", {1}.{2});\r\n", fieldName, infoName, fieldName));
                            break;
                    }
                    if ((n - kn) > 0)
                    {
                        sqlAParam1N.Append(", ");
                        sqlAParam1V.Append(", ");

                        sqlUParam1N.Append(", ");
                        sqlUParam1V.Append(", ");
                    }
                    if ((n - kn) > 0 && (n - kn) % 10 == 0)
                    {
                        sqlAParam1N.Append("\r\n\t\t\t\t\t");
                        sqlAParam1V.Append("\r\n\t\t\t\t\t");

                        sqlUParam1N.Append("\r\n\t\t\t\t\t");
                        sqlUParam1V.Append("\r\n\t\t\t\t\t");
                    }
                    switch (DBTYPE)
                    {
                        case Mysql:
                            sqlAParam1N.Append(String.Format("\"?{0}\"", fieldName));
                            sqlUParam1N.Append(String.Format("\"?{0}\"", fieldName));
                            break;
                        case Mssql:
                            sqlAParam1N.Append(String.Format("\"@{0}\"", fieldName));
                            sqlUParam1N.Append(String.Format("\"@{0}\"", fieldName));
                            break;
                        case Oracle:
                            sqlAParam1N.Append(String.Format("\":{0}\"", fieldName));
                            sqlUParam1N.Append(String.Format("\":{0}\"", fieldName));
                            break;
                    }
                    if (fieldName.Equals("CreateTime") || fieldName.Equals("UpdateTime"))
                    {
                        sqlAParam1V.Append(String.Format("CheckDateTime({0}.{1})", infoName, fieldName));
                        sqlUParam1V.Append(String.Format("CheckDateTime({0}.{1})", infoName, fieldName));
                    }
                    else
                    {
                        sqlAParam1V.Append(String.Format("{0}.{1}", infoName, fieldName));
                        sqlUParam1V.Append(String.Format("{0}.{1}", infoName, fieldName));
                    }
                }
                #endregion

                #region  Json格式
                jsonFormat.Append(fieldSeparate);
                jsonFormat.Append("\r\n");
                jsonFormat.Append(String.Format("\t\"{0}\": {1}", fieldName, this.BuildJsonValue(fieldType)));
                #endregion

                #region  Json创建
                jsonBuild.Append(fieldSeparate);
                jsonBuild.Append("\r\n");
                jsonBuild.Append(String.Format("\t{0}: {1}", fieldName, this.LowerCaseLetter(fieldName)));
                #endregion

                n++;
            }

            string endregion = "#endregion\r\n";
            property.Append(endregion);

            property.Append("\r\n");
            property.Append("#region  扩展属性\r\n\r\n");
            property.Append("public Dictionary<string, object> Extend { get; set; }\r\n\r\n");
            property.Append(endregion);

            constructor.Append("\r\n\t");
            constructor.Append("this.Extend = new Dictionary<string, object>();\r\n");
            constructor.Append("}\r\n");
            constructor.Append(endregion);

            #region  填充数据
            if (isShowFillAll)
            {
                fill.Append(String.Format("\r\n\treturn {0};\r\n", infoName));
                fill.Append("}\r\ncatch(Exception ex){throw(ex);}\r\n");
                fill.Append("}\r\n\r\n");

                //DataRowView
                fill.Append(String.Format("public {0} Fill{1}(DataRowView drv)\r\n", className, className));
                fill.Append("{\r\n");
                fill.Append("try{\r\n");
                fill.Append(String.Format("\treturn this.Fill{0}(drv.Row);\r\n", className));
                fill.Append("}\r\ncatch(Exception ex){throw(ex);}\r\n");
                fill.Append("}\r\n");
                fill.Append(endregion);
            }
            #endregion

            string classKey = ParseTableName(this.ddlTable.SelectedValue, "");
            string idKey = FirstCharToLower(priKey.fieldName);

            #region  获得单个信息
            strGet1.Append(String.Format("#region  获得单个{0}\r\n", classDesc));
            strGet1.Append(String.Format("public DataResult Get{0}(int {1})\r\n", classKey, idKey));
            strGet1.Append("{\r\ntry\r\n{\r\n");
            strGet1.Append("StringBuilder sql = new StringBuilder();\r\n");
            switch (DBTYPE)
            {
                case Mysql:
                    strGet1.Append(String.Format("sql.Append(\" select d.* from `{0}` d \");\r\n", tableName));
                    strGet1.Append(String.Format("sql.Append(String.Format(\" where d.`{0}` = {{0}} \", {1}));\r\n", priKey.field, idKey));
                    break;
                case Mssql:
                    strGet1.Append(String.Format("sql.Append(\" select d.* from [{0}] d \");\r\n", tableName));
                    strGet1.Append(String.Format("sql.Append(String.Format(\" where d.[{0}] = {{0}} \", {1}));\r\n", priKey.field, idKey));
                    break;
                case Oracle:
                    strGet1.Append(String.Format("sql.Append(\" select d.* from \\\"{0}\\\" d \");\r\n", tableName));
                    strGet1.Append(String.Format("sql.Append(String.Format(\" where d.\\\"{0}\\\" = {{0}} \", {1}));\r\n", priKey.field, idKey));
                    break;
            }
            strGet1.Append("sql.Append(\";\");\r\n\r\n");
            strGet1.Append("return new DataResult(sql, Select(DBConnString, sql));\r\n");
            strGet1.Append("}\r\ncatch (Exception ex) { throw (ex); }\r\n");
            strGet1.Append("}\r\n#endregion");


            strGet11.Append(String.Format("#region  获得单个{0}信息\r\n", classDesc));
            strGet11.Append(String.Format("public {0} Get{1}Info(int {2})\r\n", className, classKey, idKey));
            strGet11.Append("{\r\ntry\r\n{\r\n");
            strGet11.Append(String.Format("DataSet ds = this.Get{0}({1}).DataSet;\r\n\r\n", classKey, idKey));
            strGet11.Append(String.Format("return CheckTable(ds, 0) ? this.Fill{0}(ds.Tables[0].Rows[0]) : null;\r\n", className));
            strGet11.Append("}\r\ncatch (Exception ex) { throw (ex); }\r\n");
            strGet11.Append("}\r\n#endregion");
            #endregion

            #region  获取多个信息
            strGet2.Append(String.Format("#region  获得多个{0}\r\n", classDesc));
            strGet2.Append(String.Format("public DataResult Get{0}(string {1}List)\r\n", classKey, idKey));
            strGet2.Append("{\r\ntry\r\n{\r\n");
            strGet2.Append(String.Format("if (!CheckIdList(ref {0}List))\r\n{{\r\nreturn new DataResult();\r\n}}\r\n", idKey));

            strGet2.Append("StringBuilder sql = new StringBuilder();\r\n");
            switch (DBTYPE)
            {
                case Mysql:
                    strGet2.Append(String.Format("sql.Append(\" select d.* from `{0}` d \");\r\n", tableName));
                    strGet2.Append(String.Format("sql.Append(String.Format(\" where d.`{0}` in({{0}}) \", {1}List));\r\n", priKey.field, idKey));
                    break;
                case Mssql:
                    strGet2.Append(String.Format("sql.Append(\" select d.* from [{0}] d \");\r\n", tableName));
                    strGet2.Append(String.Format("sql.Append(String.Format(\" where d.[{0}] in({{0}}) \", {1}List));\r\n", priKey.field, idKey));
                    break;
                case Oracle:
                    strGet2.Append(String.Format("sql.Append(\" select d.* from \\\"{0}\\\" d \");\r\n", tableName));
                    strGet2.Append(String.Format("sql.Append(String.Format(\" where d.\\\"{0}\\\" in({{0}}) \", {1}List));\r\n", priKey.field, idKey));
                    break;
            }
            strGet2.Append("sql.Append(\";\");\r\n\r\n");
            strGet2.Append("return new DataResult(sql, Select(DBConnString, sql));\r\n");
            strGet2.Append("}\r\ncatch (Exception ex) { throw (ex); }\r\n");
            strGet2.Append("}\r\n#endregion");


            strGet22.Append(String.Format("#region  获得多个{0}信息\r\n", classDesc));
            strGet22.Append(String.Format("public List<{0}> Get{1}Info(string {2}List)\r\n", className, classKey, idKey));
            strGet22.Append("{\r\ntry\r\n{\r\n");
            strGet22.Append(String.Format("List<{0}> list = new List<{0}>();\r\n\r\n", className));

            strGet22.Append(String.Format("DataSet ds = this.Get{0}({1}List).DataSet;\r\n", classKey, idKey));
            strGet22.Append("if (CheckTable(ds, 0))\r\n{\r\n");
            strGet22.Append("foreach (DataRow dr in ds.Tables[0].Rows)\r\n{\r\n");
            strGet22.Append(String.Format("list.Add(this.Fill{0}(dr));", className));
            strGet22.Append("}\r\n");
            strGet22.Append("}\r\nreturn list;\r\n");
            strGet22.Append("}\r\ncatch (Exception ex) { throw (ex); }\r\n");
            strGet22.Append("}\r\n#endregion");
            #endregion

            #region  获得信息
            strGet3.Append(String.Format("#region  获得{0}\r\n", classDesc));
            strGet3.Append(String.Format("public DataResult Get{0}(Dictionary<string, object> dic)\r\n", classKey));
            strGet3.Append("{\r\ntry\r\n{\r\n");
            strGet3.Append("#region  Condition\r\n");
            strGet3.Append("StringBuilder con = new StringBuilder();\r\n\r\n");
            strGet3.Append("//TODO:\r\n\r\n");
            strGet3.Append("#endregion\r\n\r\n");

            strGet3.Append("#region  Sql\r\n");
            strGet3.Append("StringBuilder sql = new StringBuilder();\r\n\r\n");
            switch (DBTYPE)
            {
                case Mysql:
                    strGet3.Append(String.Format("sql.Append(\" select d.* from `{0}` d \");\r\n", tableName));
                    break;
                case Mssql:
                    strGet3.Append(String.Format("sql.Append(\" select d.* from [{0}] d \");\r\n", tableName));
                    break;
                case Oracle:
                    strGet3.Append(String.Format("sql.Append(\" select d.* from \\\"{0}\\\" d \");\r\n", tableName));
                    break;
            }
            strGet3.Append("sql.Append(\" where 1 = 1 \");\r\n");
            strGet3.Append("sql.Append(con.ToString());\r\n");
            if (priKey != null)
            {
                switch (DBTYPE)
                {
                    case Mysql:
                        strGet3.Append(String.Format("sql.Append(\" order by d.`{0}` \");\r\n", priKey.field));
                        break;
                    case Mssql:
                        strGet3.Append(String.Format("sql.Append(\" order by d.[{0}] \");\r\n", priKey.field));
                        break;
                    case Oracle:
                        strGet3.Append(String.Format("sql.Append(\" order by d.\\\"{0}\\\" \");\r\n", priKey.field));
                        break;
                }
            }
            strGet3.Append("sql.Append(DBC.BuildLimitCondition(ConvertValue(dic, \"PageIndex\", 0), ConvertValue(dic, \"PageSize\", 0)));\r\n");
            strGet3.Append("sql.Append(\";\");\r\n");

            if (priKey != null)
            {
                strGet3.Append("\r\n");
                switch (DBTYPE)
                {
                    case Mysql:
                        strGet3.Append(String.Format("sql.Append(\" select count(distinct d.`{0}`) as dataCount from `{1}` d \");\r\n", priKey.field, tableName));
                        break;
                    case Mssql:
                        strGet3.Append(String.Format("sql.Append(\" select count(distinct d.[{0}]) as dataCount from [{1}] d \");\r\n", priKey.field, tableName));
                        break;
                    case Oracle:
                        strGet3.Append(String.Format("sql.Append(\" select count(distinct d.\\\"{0}\\\") as dataCount from \\\"{1}\\\" d \");\r\n", priKey.field, tableName));
                        break;
                }

                strGet3.Append("sql.Append(\" where 1 = 1 \");\r\n");
                strGet3.Append("sql.Append(con.ToString());\r\n");
                strGet3.Append("sql.Append(\";\");\r\n");
            }

            strGet3.Append("#endregion\r\n\r\n");
            strGet3.Append("return new DataResult(sql, Select(DBConnString, sql));\r\n");
            strGet3.Append("}\r\ncatch (Exception ex) { throw (ex); }\r\n");
            strGet3.Append("}\r\n#endregion");


            #region  BLL方法
            strGet33.Append(String.Format("#region  获得{0}信息\r\n", classDesc));

            strGet33.Append(String.Format("public List<Dictionary<string, object>> Get{0}Info(DataSet ds, Dictionary<string, string> dicField, out int dataCount)\r\n", classKey));
            strGet33.Append("{\r\ntry\r\n{\r\n");
            strGet33.Append("List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();\r\n\r\n");

            strGet33.Append("if (CheckTable(ds, 0))\r\n{\r\n");
            strGet33.Append("foreach (DataRow dr in ds.Tables[0].Rows)\r\n{\r\n");
            strGet33.Append(String.Format("{0}Info info = this.Fill{0}Info(dr);\r\n", classKey));
            strGet33.Append("if (info != null)\r\n");
            strGet33.Append("{\r\n");
            strGet33.Append("list.Add(ConvertClassValue(info, dicField, true));");
            strGet33.Append("}\r\n");
            strGet33.Append("}\r\n");
            strGet33.Append("}\r\n");
            strGet33.Append("dataCount = ConvertFieldValue(ds, 1, list.Count);\r\n\r\n");
            strGet33.Append("return list;\r\n");
            strGet33.Append("}\r\ncatch (Exception ex) { throw (ex); }\r\n");
            strGet33.Append("}\r\n\r\n");

            //重写方法
            strGet33.Append(String.Format("public List<Dictionary<string, object>> Get{0}Info(DataSet ds, Dictionary<string, string> dicField)\r\n", classKey));
            strGet33.Append("{\r\ntry\r\n{\r\n");
            strGet33.Append(String.Format("int dataCount = 0;\r\n"));
            strGet33.Append(String.Format("return Get{0}(ds, dicField, out dataCount);\r\n", className));
            strGet33.Append("}\r\ncatch (Exception ex) { throw (ex); }\r\n");
            strGet33.Append("}\r\n");

            strGet33.Append(String.Format("public List<{0}> Get{1}Info(DataSet ds, out int dataCount)\r\n", className, classKey));
            strGet33.Append("{\r\ntry\r\n{\r\n");
            strGet33.Append(String.Format("List<{0}> list = new List<{0}>();\r\n\r\n", className));

            strGet33.Append("if (CheckTable(ds, 0))\r\n{\r\n");
            strGet33.Append("foreach (DataRow dr in ds.Tables[0].Rows)\r\n{\r\n");
            strGet33.Append(String.Format("list.Add(this.Fill{0}(dr));", className));
            strGet33.Append("}\r\n");
            strGet33.Append("}\r\n");
            strGet33.Append("dataCount = ConvertFieldValue(ds, 1, list.Count);\r\n\r\n");
            strGet33.Append("return list;\r\n");
            strGet33.Append("}\r\ncatch (Exception ex) { throw (ex); }\r\n");
            strGet33.Append("}\r\n\r\n");

            //重写方法
            strGet33.Append(String.Format("public List<{0}> Get{1}Info(DataSet ds)\r\n", className, classKey));
            strGet33.Append("{\r\ntry\r\n{\r\n");
            strGet33.Append(String.Format("int dataCount = 0;\r\n"));
            strGet33.Append(String.Format("return Get{0}(ds, out dataCount);\r\n", className));
            strGet33.Append("}\r\ncatch (Exception ex) { throw (ex); }\r\n");
            strGet33.Append("}\r\n");

            strGet33.Append("#endregion");

            #endregion

            #endregion

            #region  新增的SQL
            sqlA.Append(String.Format("StringBuilder sql = new StringBuilder();\r\n", tableName));
            switch (DBTYPE)
            {
                case Mysql:
                    sqlA.Append(String.Format("sql.Append(\"insert into `{0}`(\");\r\n", tableName));
                    break;
                case Mssql:
                    sqlA.Append(String.Format("sql.Append(\"insert into [{0}](\");\r\n", tableName));
                    break;
                case Oracle:
                    sqlA.Append(String.Format("sql.Append(\"insert into \\\"{0}\\\"(\");\r\n", tableName));
                    break;
            }
            sqlA.Append(String.Format("sql.Append(\"{0}\");\r\n", sqlAF.ToString()));
            sqlA.Append("sql.Append(\")values(\");\r\n");
            sqlA.Append(String.Format("sql.Append(\"{0}\");\r\n", sqlAV.ToString()));
            sqlA.Append("sql.Append(\");\");\r\n");
            switch (DBTYPE)
            {
                case Mysql:
                    sqlAParam.Insert(0, "MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[c];\r\n");
                    break;
                case Mssql:
                    sqlAParam.Insert(0, "System.Data.SqlClient.SqlParameter[] param = new System.Data.SqlClient.SqlParameter[c];\r\n");
                    break;
                case Oracle:
                    sqlAParam.Insert(0, "System.Data.OracleClient.OracleParameter[] param = new System.Data.OracleClient.OracleParameter[c];\r\n");
                    break;
            }
            sqlAParam.Insert(0, String.Format("int c = {0};\r\nint n = 0;\r\n", rowCount - kn));

            sqlAParam.Append("\r\nDataResult result = new DataResult(sql.ToString());\r\n");
            switch (DBTYPE)
            {
                case Mysql:
                    sqlAParam.Append("result.Result = MySqlHelper.ExecuteNonQuery(" + dbconn + ", CommandType.Text, sql.ToString(), param);\r\n");
                    break;
                case Mssql:
                    sqlAParam.Append("result.Result = SqlHelper.ExecuteNonQuery(" + dbconn + ", CommandType.Text, sql.ToString(), param);\r\n");
                    break;
                case Oracle:
                    sqlAParam.Append("result.Result = OracleHelper.ExecuteNonQuery(" + dbconn + ", CommandType.Text, sql.ToString(), param);\r\n");
                    break;
            }
            sqlAParam.Append("if (result.iResult > 0)\r\n{\r\n");
            sqlAParam.Append(String.Format("result.Result = DBConnection.GetMaxId(" + dbconn + ", \"{0}\", \"{1}\");\r\n", tableName, keyName));
            sqlAParam.Append("}\r\n");
            sqlAParam.Append("\r\nreturn result;");

            //方式二
            sqlAParam1.Append("List<string> name = new List<string>() {\r\n");
            sqlAParam1.Append("\t\t\t\t\t");
            sqlAParam1.Append(sqlAParam1N.ToString());
            sqlAParam1.Append("\r\n\t\t\t\t};\r\n");

            sqlAParam1.Append("\r\n");

            sqlAParam1.Append("List<object> value = new List<object>() {\r\n");
            sqlAParam1.Append("\t\t\t\t\t");
            sqlAParam1.Append(sqlAParam1V.ToString());
            sqlAParam1.Append("\r\n\t\t\t\t};\r\n\r\n");

            sqlAParam1.Append("DataResult result = new DataResult(sql.ToString(), Insert(" + dbconn1 + ", sql.ToString(), BuildSqlParam(name.Count, name, value)));\r\n");
            sqlAParam1.Append(String.Format("result.Result = result.Result > 0 ? GetMaxId(" + dbconn1 + ", \"{0}\", \"{1}\") : 0;\r\n", tableName, keyName));
            sqlAParam1.Append("\r\nreturn result;");
            #endregion

            #region  更新的SQL
            sqlU.Append("StringBuilder sql = new StringBuilder();\r\n");
            switch (DBTYPE)
            {
                case Mysql:
                    sqlU.Append(String.Format("sql.Append(\" update `{0}` set \");\r\n", tableName));
                    break;
                case Mssql:
                    sqlU.Append(String.Format("sql.Append(\" update [{0}] set \");\r\n", tableName));
                    break;
                case Oracle:
                    sqlU.Append(String.Format("sql.Append(\" update \\\"{0}\\\" set \");\r\n", tableName));
                    break;
            }
            sqlU.Append(String.Format("sql.Append(\"{0}\");\r\n", sqlUV.ToString()));

            if (priKey != null)
            {
                switch (DBTYPE)
                {
                    case Mysql:
                        sqlUF.Append(String.Format(" where `{0}` = ?{1};", priKey.field, priKey.fieldName));
                        break;
                    case Mssql:
                        sqlUF.Append(String.Format(" where [{0}] = @{1};", priKey.field, priKey.fieldName));
                        break;
                    case Oracle:
                        sqlUF.Append(String.Format(" where \\\"{0}\\\" = :{1};", priKey.field, priKey.fieldName));
                        break;
                }
            }
            sqlU.Append(String.Format("sql.Append(\"{0}\");\r\n", sqlUF.ToString()));


            if (priKey != null)
            {
                switch (DBTYPE)
                {
                    case Mysql:
                        sqlUParam.Append(String.Format("param[n++] = new MySql.Data.MySqlClient.MySqlParameter(\"?{0}\", {1}.{2});\r\n", priKey.fieldName, infoName, priKey.fieldName));
                        break;
                    case Mssql:
                        sqlUParam.Append(String.Format("param[n++] = new System.Data.SqlClient.SqlParameter(\"@{0}\", {1}.{2});\r\n", priKey.fieldName, infoName, priKey.fieldName));
                        break;
                    case Oracle:
                        sqlUParam.Append(String.Format("param[n++] = new System.Data.OracleClient.OracleParameter(\":{0}\", {1}.{2});\r\n", priKey.fieldName, infoName, priKey.fieldName));
                        break;
                }
            }
            sqlUParam.Append("\r\nDataResult result = new DataResult(sql.ToString());\r\n");
            switch (DBTYPE)
            {
                case Mysql:
                    sqlUParam.Append("result.Result = MySqlHelper.ExecuteNonQuery(" + dbconn + ", CommandType.Text, sql.ToString(), param);\r\n");
                    break;
                case Mssql:
                    sqlUParam.Append("result.Result = SqlHelper.ExecuteNonQuery(" + dbconn + ", CommandType.Text, sql.ToString(), param);\r\n");
                    break;
                case Oracle:
                    sqlUParam.Append("result.Result = OracleHelper.ExecuteNonQuery(" + dbconn + ", CommandType.Text, sql.ToString(), param);\r\n");
                    break;
            }
            sqlUParam.Append("\r\nreturn result;");


            //方式二
            if (priKey != null)
            {
                string ups_tmp = (n - kn) > 0 && (n - kn) % 10 == 0 ? "\r\n" : " ";
                sqlUParam1N.Append(String.Format(",{0}\"?{1}\"", ups_tmp, priKey.fieldName));
                sqlUParam1V.Append(String.Format(",{0}{1}.{2}", ups_tmp, infoName, priKey.fieldName));
            }

            sqlUParam1.Append("List<string> name = new List<string>() {\r\n");
            sqlUParam1.Append("\t\t\t\t\t");
            sqlUParam1.Append(sqlUParam1N.ToString());
            sqlUParam1.Append("\r\n\t\t\t\t};\r\n");

            sqlUParam1.Append("\r\n");

            sqlUParam1.Append("List<object> value = new List<object>() {\r\n");
            sqlUParam1.Append("\t\t\t\t\t");
            sqlUParam1.Append(sqlUParam1V.ToString());
            sqlUParam1.Append("\r\n\t\t\t\t};\r\n\r\n");

            sqlUParam1.Append("return new DataResult(sql.ToString(), Update(" + dbconn1 + ", sql.ToString(), BuildSqlParam(name.Count, name, value)));");

            #endregion

            #region  新增的方法
            sqlAdd.Append(String.Format("#region 新增{0}\r\n", classDesc));
            sqlAdd.Append(String.Format("public DataResult Add{0}({1} {2})\r\n", classField, className, infoName));
            sqlAdd.Append("{\r\n\ttry\r\n\t{\r\n");

            sqlAdd.Append(sqlA.ToString());
            sqlAdd.Append("\r\n");
            sqlAdd.Append(sqlAParam.ToString());
            sqlAdd.Append("\r\n");

            sqlAdd.Append("\t}\r\n");
            sqlAdd.Append("\tcatch (Exception ex) { throw (ex); }\r\n");
            sqlAdd.Append("}\r\n");
            sqlAdd.Append("#endregion");

            //方式二
            sqlAdd1.Append(String.Format("#region 新增{0}\r\n", classDesc));
            sqlAdd1.Append(String.Format("public DataResult Add{0}({1} {2})\r\n", classField, className, infoName));
            sqlAdd1.Append("{\r\n\ttry\r\n\t{\r\n");

            sqlAdd1.Append(sqlA.ToString());
            sqlAdd1.Append("\r\n");
            sqlAdd1.Append(sqlAParam1.ToString());
            sqlAdd1.Append("\r\n");

            sqlAdd1.Append("\t}\r\n");
            sqlAdd1.Append("\tcatch (Exception ex) { throw (ex); }\r\n");
            sqlAdd1.Append("}\r\n");
            sqlAdd1.Append("#endregion");
            #endregion

            #region  更新的方法
            sqlUpdate.Append(String.Format("#region 更新{0}\r\n", classDesc));
            sqlUpdate.Append(String.Format("public DataResult Update{0}({1} {2})\r\n", classField, className, infoName));
            sqlUpdate.Append("{\r\n\ttry\r\n\t{\r\n");

            sqlUpdate.Append(sqlU.ToString());
            sqlUpdate.Append("\r\n");
            sqlUpdate.Append(sqlUParam.ToString());
            sqlUpdate.Append("\r\n");

            sqlUpdate.Append("\t}\r\n");
            sqlUpdate.Append("\tcatch (Exception ex) { throw (ex); }\r\n");
            sqlUpdate.Append("}\r\n");
            sqlUpdate.Append("#endregion");


            sqlUpdate1.Append(String.Format("#region 更新{0}\r\n", classDesc));
            sqlUpdate1.Append(String.Format("public DataResult Update{0}({1} {2})\r\n", classField, className, infoName));
            sqlUpdate1.Append("{\r\n\ttry\r\n\t{\r\n");

            sqlUpdate1.Append(sqlU.ToString());
            sqlUpdate1.Append("\r\n");
            sqlUpdate1.Append(sqlUParam1.ToString());
            sqlUpdate1.Append("\r\n");

            sqlUpdate1.Append("\t}\r\n");
            sqlUpdate1.Append("\tcatch (Exception ex) { throw (ex); }\r\n");
            sqlUpdate1.Append("}\r\n");
            sqlUpdate1.Append("#endregion");
            #endregion

            #region 删除的方法
            sqlDel.Append(String.Format("#region 删除{0}\r\n", classDesc));
            sqlDel.Append(String.Format("public DataResult Delete{0}(int {1})\r\n", classField, this.LowerCaseLetter(priKey.fieldName)));
            sqlDel.Append("{\r\n\ttry\r\n\t{\r\n");
            switch (DBTYPE)
            {
                case Mysql:
                    sqlDel.Append(String.Format("\t\tstring sql = String.Format(\"delete from `{0}` where `{1}` = {{0}};\", {2});\r\n",
                        tableName, priKey.field, this.LowerCaseLetter(priKey.fieldName)));
                    break;
                case Mssql:
                    sqlDel.Append(String.Format("\t\tstring sql = String.Format(\"delete from [{0}] where [{1}] = {{0}};\", {2});\r\n",
                        tableName, priKey.field, this.LowerCaseLetter(priKey.fieldName)));
                    break;
                case Oracle:
                    sqlDel.Append(String.Format("\t\tstring sql = String.Format(\"delete from \\\"{0}\\\" where \\\"{1}\\\" = {{0}};\", {2});\r\n",
                        tableName, priKey.field, this.LowerCaseLetter(priKey.fieldName)));
                    break;
            }
            sqlDel.Append(String.Format("\r\n"));

            #region  方式二
            sqlDel1.Append(sqlDel.ToString());
            sqlDel1.Append("\t\treturn new DataResult(sql, Delete(" + dbconn1 + ", sql));\r\n");

            sqlDel1.Append("\t}\r\n");
            sqlDel1.Append("\tcatch (Exception ex) { throw (ex); }\r\n");
            sqlDel1.Append("}\r\n");
            sqlDel1.Append("#endregion");
            #endregion

            switch (DBTYPE)
            {
                case Mysql:
                    sqlDel.Append("\t\treturn new DataResult(sql, MysqlHelper.ExecuteNonQuery(" + dbconn + ", sql));\r\n");
                    break;
                case Mssql:
                    sqlDel.Append("\t\treturn new DataResult(sql, MssqlHelper.ExecuteNonQuery(" + dbconn + ", sql));\r\n");
                    break;
                case Oracle:
                    sqlDel.Append("\t\treturn new DataResult(sql, OracleHelper.ExecuteNonQuery(" + dbconn + ", sql));\r\n");
                    break;
            }

            sqlDel.Append("\t}\r\n");
            sqlDel.Append("\tcatch (Exception ex) { throw (ex); }\r\n");
            sqlDel.Append("}\r\n");
            sqlDel.Append("#endregion");
            #endregion


            #region  JSON格式
            jsonFormat.Append("\r\n}");
            #endregion

            #region  JSON数据
            jsonBuild.Append("\r\n}");
            #endregion

            #region  Class数据
            classInfo.Append(property.ToString());
            classInfo.Append("\r\n");
            classInfo.Append(constructor.ToString());
            classInfo.Append("\r\n}");

            classInfo.Append("\r\n\r\n#endregion\r\n");
            #endregion

            this.txtClassData.Text = classInfo.ToString();

            this.txtModel.Text = property.ToString();
            this.txtConstructor.Text = constructor.ToString();

            this.txtFillData.Text = fill.ToString();

            this.txtGetInfo1.Text = strGet1.ToString();
            this.txtGetInfo2.Text = strGet2.ToString();

            this.txtGetInfo11.Text = strGet11.ToString();
            this.txtGetInfo22.Text = strGet22.ToString();

            this.txtGetInfo3.Text = strGet3.ToString();
            this.txtGetInfo33.Text = strGet33.ToString();

            this.txtAddMethod.Text = sqlAdd.ToString();
            this.txtAddMethod1.Text = sqlAdd1.ToString();

            this.txtUpdateMethod.Text = sqlUpdate.ToString();
            this.txtUpdateMethod1.Text = sqlUpdate1.ToString();

            this.txtDelMethod.Text = sqlDel.ToString();
            this.txtDelMethod1.Text = sqlDel1.ToString();

            this.txtAddSql.Text = sqlA.ToString();
            this.txtUpdateSql.Text = sqlU.ToString();

            this.txtAddSqlParam.Text = sqlAParam.ToString();
            this.txtAddSqlParam1.Text = sqlAParam1.ToString();

            this.txtUpdateSqlParam.Text = sqlUParam.ToString();
            this.txtUpdateSqlParam1.Text = sqlUParam1.ToString();

            this.txtJsonFormat.Text = jsonFormat.ToString();
            this.txtJsonBuild.Text = jsonBuild.ToString();

            //SQL 语句

            #region  新增的SQL语句

            #region  新增的SQL语句
            StringBuilder sqlAS = new StringBuilder();
            StringBuilder sqlAFS = new StringBuilder();
            StringBuilder sqlAVS = new StringBuilder();

            sqlAS.Append(String.Format("StringBuilder sql = new StringBuilder();\r\n", tableName));
            sqlAS.Append(String.Format("sql.Append(\"insert into `{0}`(\");\r\n", tableName));
            sqlAS.Append(String.Format("sql.Append(\"{0}\");\r\n", sqlAF.ToString()));
            sqlAS.Append("sql.Append(\")values(\");\r\n");
            #endregion

            #region  批量新增的SQL语句
            StringBuilder sqlASB = new StringBuilder();
            StringBuilder sqlAFSB = new StringBuilder();
            StringBuilder sqlAVSB = new StringBuilder();
            sqlASB.Append(String.Format("StringBuilder sql = new StringBuilder();\r\n", tableName));
            sqlASB.Append(String.Format("sql.Append(\"insert into `{0}`(\");\r\n", tableName));
            sqlASB.Append(String.Format("sql.Append(\"{0}\");\r\n", sqlAF.ToString()));
            sqlASB.Append("sql.Append(\")values\");\r\n");
            sqlASB.Append("\r\nint n = 0;\r\n");
            sqlASB.Append(String.Format("foreach ({0} o in list)\r\n", className));
            sqlASB.Append("{\r\n");
            sqlASB.Append("sql.Append(n++ > 0 ? \",\" : \"\");\r\n");
            sqlASB.Append("sql.Append(\"(\");\r\n");
            #endregion

            int y = 1;
            n = 0;
            kn = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                string field = dr["Field"].ToString();
                string fieldName = this.ParseFieldName(field);
                string comment = this.ParseFieldComment(dr["Comment"].ToString());
                string fieldType = this.ParseFieldType(dr["Type"].ToString());

                #region  SQL语句
                if (dr["Extra"].ToString().Equals("auto_increment"))
                {
                    kn = 1;
                }
                else
                {
                    if ((n) > 0)
                    {
                        sqlAFS.Append(",");
                        sqlAVS.Append((n) % 10 == 0 ? "" : ", ");

                        sqlAFSB.Append(",");
                        sqlAVSB.Append((n) % 10 == 0 ? "" : ", ");
                    }
                    sqlAFS.Append(this.BuildSqlValue(fieldType, (y - kn)));
                    sqlAVS.Append(String.Format("{0}.{1}", infoName, fieldName));

                    sqlAFSB.Append(this.BuildSqlValue(fieldType, (y - kn)));
                    sqlAVSB.Append(String.Format("{0}.{1}", infoName, fieldName));

                    if (((n) > 0 && (n + kn) % 10 == 0) || (n) >= rowCount - (1 + kn))
                    {
                        sqlAS.Append(String.Format("sql.Append(String.Format(\"{0}\",\r\n\t{1}));\r\n", sqlAFS.ToString(), sqlAVS.ToString()));

                        sqlAFS.Length = 0;
                        sqlAVS.Length = 0;

                        sqlASB.Append(String.Format("sql.Append(String.Format(\"{0}\",\r\n{1}));\r\n", sqlAFSB.ToString(), sqlAVSB.ToString()));

                        sqlAFSB.Length = 0;
                        sqlAVSB.Length = 0;

                        y = 0;
                    }

                    n++;
                    y++;
                }
                #endregion

            }

            sqlAS.Append("sql.Append(\");\");\r\n");

            sqlASB.Append("sql.Append(\")\");\r\n");
            sqlASB.Append("}\r\n");
            sqlASB.Append("sql.Append(\";\");\r\n");
            #endregion

            this.txtAddSqlString.Text = sqlAS.ToString();

            this.txtAddSqlStringBatch.Text = sqlASB.ToString();


            this.txtDataOperateDAL.Text = this.BuildResult(new List<string>() { 
                this.txtGetInfo1.Text, this.txtGetInfo2.Text, this.txtGetInfo3.Text,
                this.txtAddMethod1.Text, this.txtUpdateMethod1.Text, this.txtDelMethod1.Text,
                this.txtFillData.Text
            });

            this.txtDataOperateBLL.Text = this.BuildResult(new List<string>() {
                this.txtGetInfo11.Text, this.txtGetInfo22.Text, this.txtGetInfo33.Text 
            });
        }
        catch (Exception ex) { this.divError.InnerHtml = ex.Message; }
    }
    #endregion

    protected string BuildResult(List<string> list)
    {
        StringBuilder rs = new StringBuilder();
        int n = 0;
        foreach (string s in list)
        {
            rs.Append(n++ > 0 ? "\r\n\r\n" : "");
            rs.Append(s);
        }
        return rs.ToString();
    }

    #region  Form Action
    protected void btnBuild_Click(object sender, EventArgs e)
    {
        string className = this.txtClassName.Text.Trim();
        string classDesc = this.txtClassComment.Text.Trim();
        this.BuildFieldModel(dsField, className, classDesc);
    }

    protected void chbCutFirstPrefix_CheckedChanged(object sender, EventArgs e)
    {
        string tableName = this.ddlTable.SelectedValue.ToString();
        if (tableName.Length > 0)
        {
            string className = this.ParseTableName(tableName, "Info");
            this.txtClassName.Text = className;
            string classDesc = this.txtClassComment.Text.Trim();
            this.BuildFieldModel(dsField, className, classDesc);
        }
        else
        {
            this.ClearData();
        }
    }

    protected void chbOnlyField_CheckedChanged(object sender, EventArgs e)
    {
        string className = this.txtClassName.Text.Trim();
        string classDesc = this.txtClassComment.Text.Trim();
        this.BuildFieldModel(dsField, className, classDesc);
    }
    #endregion


    #region LowerCaseLetter
    public string LowerCaseLetter(string con)
    {
        if (con.Length > 0)
        {
            return con.Substring(0, 1).ToLower() + con.Substring(1);
        }
        return con;
    }
    #endregion

    #region  Parse Field Type
    public string ParseFieldType(string fieldType)
    {
        string type = fieldType.Split('(')[0].Trim();
        switch (type)
        {
            case "int":
            case "smallint":
            case "tinyint":
                type = "int";
                break;
            case "float":
                type = "float";
                break;
            case "varchar":
            case "nvarchar":
            case "text":
            case "char":
            case "datetime":
            case "date":
            case "enum":
                type = "string";
                break;
            default:
                type = "string";
                break;
        }
        return type;
    }
    #endregion


    public string FirstCharToLower(string s)
    {
        return s.Substring(0, 1).ToLower() + s.Substring(1);
    }

    #region  Parse Table Name
    public string ParseTableName(string tableName, string postfix)
    {
        if (tableName.Equals(string.Empty))
        {
            return string.Empty;
        }
        StringBuilder name = new StringBuilder();

        string[] arrName = tableName.Split('_');
        int n = 0;
        bool isCutFirstPrefix = this.chbCutFirstPrefix.Checked;
        bool hasPrefix = tableName.IndexOf('_') > 0;

        foreach (string str in arrName)
        {
            if (isCutFirstPrefix && hasPrefix && 0 == n++)
            {
                continue;
            }
            name.Append(str.Substring(0, 1).ToUpper());
            name.Append(str.Substring(1));
        }
        if (name.ToString().EndsWith("Info") && postfix == "Info")
        {
            postfix = "";
        }
        return name.ToString() + postfix;
    }
    #endregion

    #region  Parse Field Name
    public string ParseFieldName(string fieldName)
    {
        if (fieldName.Equals(string.Empty))
        {
            return string.Empty;
        }
        StringBuilder name = new StringBuilder();

        string[] arrName = fieldName.Split('_');
        foreach (string str in arrName)
        {
            name.Append(str.Substring(0, 1).ToUpper());
            name.Append(str.Substring(1));
        }
        return name.ToString();
    }
    #endregion


    #region  Parse Comment
    public string ParseFieldComment(string comment)
    {
        if (comment.Equals(string.Empty))
        {
            return string.Empty;
        }
        string[] strDelimiter = { ",", "，", "。", "：", ":" };
        return comment.Split(strDelimiter, StringSplitOptions.RemoveEmptyEntries)[0];
    }
    #endregion

    #region  Build Default Value
    public string BuildDefaultValue(string type)
    {
        if (type.IndexOf("int") >= 0)
        {
            return "0";
        }
        else if (type == "float")
        {
            return "0.0f";
        }
        else
        {
            return "string.Empty";
        }
    }
    #endregion

    #region  Build Json Value
    public string BuildJsonValue(string type)
    {
        if (type.IndexOf("int") >= 0 || type == "float")
        {
            return "0";
        }
        else
        {
            return "\"\"";
        }
    }
    #endregion

    #region  Build Sql Value
    public string BuildSqlValue(string type, int idx)
    {
        if (type.IndexOf("int") >= 0 || type == "float")
        {
            return String.Format("{{{0}}}", idx);
        }
        else
        {
            return String.Format("'{{{0}}}'", idx);
        }
    }
    #endregion


    #region  Convert Data
    public string ConvertData(string type, string field, string convertFunc)
    {
        if (type.IndexOf("int") >= 0)
        {
            return String.Format("{0}(dr[\"{1}\"], 0)", convertFunc, field);
        }
        else if (type == "float")
        {
            return String.Format("{0}(dr[\"{1}\"], 0.0f)", convertFunc, field);
        }
        else
        {
            return String.Format("dr[\"{0}\"].ToString()", field);
        }
    }
    #endregion

    protected void ddlDbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.DBTYPE = Convert.ToInt32(this.ddlDbType.SelectedValue);
        string tableName = this.ddlTable.SelectedValue.ToString();
        if (tableName.Length > 0)
        {
            string className = this.ParseTableName(tableName, "Info");
            this.txtClassName.Text = className;
            string classDesc = this.txtClassComment.Text.Trim();
            this.BuildFieldModel(dsField, className, classDesc);
        }
        else
        {
            this.ClearData();
        }

    }
}
/*
public class DBA_PriKeyInfo
{
    public string field { get; set; }
    public string fieldName { get; set; }
    public string fieldType { get; set; }


    public DBA_PriKeyInfo()
    {
        this.field = "";
        this.fieldName = "";
        this.fieldType = "";
    }
}*/