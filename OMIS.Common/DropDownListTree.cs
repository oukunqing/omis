using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Text;
using System.Data;

namespace OMIS.Common
{
    public class DropDownListTree
    {

        #region  绑定数据生成树形结构下拉菜单
        /// <summary> 
        /// 绑定生成一个有树结构的下拉菜单 
        /// </summary> 
        /// <param name="data">菜单记录数据所在的表</param> 
        /// <param name="parentIdField">表中用于标记父记录的字段</param> 
        /// <param name="rootValue">第一层记录的父记录值(通常设计为0或者-1)用来表示没有父记录</param> 
        /// <param name="valueField">值字段，也就是放在DropDownList的Value里面的字段</param> 
        /// <param name="textField">文本字段，也就是放在DropDownList的Text里面的字段</param> 
        /// <param name="ddlObj">需要绑定的DropDownList</param> 
        /// <param name="level">层级，用来控制缩入量的值，默认为-1</param> 
        /// <param name="spaceCount">缩进的空格个数</param> 
        /// <param name="showTabs">是否显示制表符└ </param> 
        public void MakeTree(DataTable data, string parentIdField, string rootValue, string valueField, string textFieldList,
            DropDownList ddlObj, int level, int spaceCount, bool showTabs)
        {
            //每向下一层，多一个缩入单位 
            level++;

            DataView dvNodeSets = new DataView(data);
            dvNodeSets.RowFilter = parentIdField + "=" + rootValue;

            string strSpace = "";
            string strPading = "";  //缩入字符
            //string strTab = showTabs ? "└ " : "";
            string strTab = showTabs ? "├ " : "";
            string strTabLast = showTabs ? "└ " : "";
            string strLine = "";
            int c = dvNodeSets.Count;
            int n = 0;
            string[] arrTextField = textFieldList.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries);

            for (int k = 0; k < spaceCount; k++)
            {
                strSpace += strLine + "　";　//缩进字符，设置的是一个全角的空格 
            }

            //通过i来控制缩入字符的长度
            for (int j = 0; j < level; j++)
            {
                strPading += strSpace;
            }
            foreach (DataRowView drv in dvNodeSets)
            {
                StringBuilder strText = new StringBuilder();
                int i = 0;
                foreach (string str in arrTextField)
                {
                    strText.Append(i++ > 0 ? " " : "");
                    strText.Append(drv[str].ToString());
                }
                ListItem li = new ListItem(strPading + (n == c - 1 ? strTabLast : strTab) + strText.ToString(), drv[valueField].ToString());
                ddlObj.Items.Add(li);

                MakeTree(data, parentIdField, drv[valueField].ToString(), valueField, textFieldList, ddlObj, level, spaceCount, showTabs);
                n++;
            }

            //递归结束，要回到上一层，所以缩入量减少一个单位 
            level--;
        }
        #endregion

    }
}