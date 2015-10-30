﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using GTLutils;

namespace CentralControl
{
    public partial class DataQueryForm : Form
    {
        public ControlForm FatherForm;

        private string select, where;

        List<string> selecteditems;

        protected void display()
        {
            if (!where.Equals(""))
                textBox1.Text = "select " + select + " from " + dataTableComboBox.Text + " where " + where;
            else
                textBox1.Text = "select " + select + " from " + dataTableComboBox.Text;
        }



        public DataQueryForm()
        {
            InitializeComponent();
            /*foreach (DeviceType type in EnumHelper.TypeEnums)
            {
                deviceTypeComboBox.Items.Add(EnumHelper.getDeviceTypeString(type));
            }
            deviceTypeComboBox.SelectedIndex = 0;*/

            foreach (Operations op in OperationHelper.OpeEnums)
            {
                operationTypeComboBox.Items.Add(OperationHelper.getOperationString(op));
            }
            operationTypeComboBox.SelectedIndex = 0;

            foreach (Logics logic in LogicHelper.LogicEnums)
            {
                logicTypeComboBox.Items.Add(LogicHelper.getLogicString(logic));
            }
            logicTypeComboBox.SelectedIndex = 0;
            select = "";
            where = "";

        }

        private void DataQueryForm_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            FatherForm.Enabled = false;

            ArrayList list = DBUtil.getTableList();
            dataTableComboBox.Items.Clear();
            foreach (String s in list)
            {
                dataTableComboBox.Items.Add(s);
            }
            dataTableComboBox.SelectedIndex = 0;
            segmentComboBox.Items.Clear();
            segmentComboBox.Items.Add("CurrentTime");
            segmentComboBox.Items.Add("Device_Id");
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataQueryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FatherForm.Enabled = true;
        }

        private void dataTableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tableName = dataTableComboBox.SelectedItem.ToString();
            ArrayList list = DBUtil.getTableColumns(tableName);
            filtercombobox.Items.Clear();
            foreach (String s in list) 
            {
                filtercombobox.Items.Add(s);
            }
            if(list.Count > 0) filtercombobox.SelectedIndex = 0;
            textBox1.Text = "";
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (opValueTextBox.Text.Equals("")) return;
            String con = "";
            con = segmentComboBox.SelectedItem.ToString() + " " + operationTypeComboBox.SelectedItem.ToString() + " '" + opValueTextBox.Text + "' ";
            String opStr = "and";
            if (logicTypeComboBox.SelectedIndex > 0) opStr = "or";
            if (where.Equals("")) where = con;
            else where = "(" + where + ") " + opStr + " " + con;
            display();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            select = "";
            where = "";
        }

        private void searchDataButton_Click(object sender, EventArgs e)
        {
            List<string> colList;
            DBUtil mydb=new DBUtil();
            List<List<string>> list = mydb.executeQueryCmd(textBox1.Text);
            colList = mydb.getcloumnname();
            searchResultListView.Columns.Clear();
            for (int i = 0; i < colList.Count; i++)
            {
                ColumnHeader header = new ColumnHeader();
                header.Text = (String)colList[i];
                searchResultListView.Columns.Add(header);
            }

            searchResultListView.Items.Clear();
            foreach (List<string> ele in list)
            {
                ListViewItem item = new ListViewItem();
                item.Text = ele[0];
                for (int i = 1; i < ele.Count; i++)
                {
                    item.SubItems.Add(ele[i]);
                }
                searchResultListView.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String con = filtercombobox.Text;
            if (select.Equals("")) select = con;
            else select = select+" , " + con;
            display();
        }

        private void filtercombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
