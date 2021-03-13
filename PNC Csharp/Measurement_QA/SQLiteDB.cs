using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace PNC_Csharp.Measurement_QA
{
    class SQLiteDB
    {
        Form1 f1()
        {
            return (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
        }


        List<DataGridView> DGVs;   
        public SQLiteDB(DataGridView datagridview1, DataGridView datagridview2, DataGridView datagridview3, DataGridView datagridview4, DataGridView datagridview5, DataGridView datagridview6, DataGridView datagridview7
            , DataGridView datagridview8, DataGridView datagridview9, DataGridView datagridview10, DataGridView datagridview11, DataGridView datagridview12, DataGridView datagridview13, DataGridView datagridview14, DataGridView datagridview15) 
        {
            DGVs = new List<DataGridView>();
            DGVs.Add(datagridview1);
            DGVs.Add(datagridview2);
            DGVs.Add(datagridview3);
            DGVs.Add(datagridview4);
            DGVs.Add(datagridview5);
            DGVs.Add(datagridview6);
            DGVs.Add(datagridview7);
            DGVs.Add(datagridview8);
            DGVs.Add(datagridview9);
            DGVs.Add(datagridview10);
            DGVs.Add(datagridview11);
            DGVs.Add(datagridview12);
            DGVs.Add(datagridview13);
            DGVs.Add(datagridview14);
            DGVs.Add(datagridview15);
        }

        public void Create_New_DB(string DB_Name)
        {
            if (Create_Empty_DB_File(DB_Name))
            {
                Create_Empty_DGVs_tables(DB_Name);
                Fill_DB_From_DGVs(DB_Name);
            }
           
        }




        private bool Create_Empty_DB_File(string DB_Name)
        {
            try 
            {
                SQLiteConnection.CreateFile(Directory.GetCurrentDirectory() + @"\Optic_Measurement\DataBase" + @"\" + DB_Name + ".db");
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void Create_Empty_DGVs_tables(string DB_Name)
        {
            //Create DBVs' tables
            string connectionString = "Data Source=" + Directory.GetCurrentDirectory() + @"\Optic_Measurement\DataBase" + @"\" + DB_Name + ".db;" + " Version=3;";
            using (SQLiteConnection sqliteCon = new SQLiteConnection(connectionString))
            {
                sqliteCon.Open();

                for (int i = 0; i < DGVs.Count; i++)
                {
                    //Create Table
                    string Create_Table_CMD = Get_Fill_Table_SQL_CMD(i);
                    f1().GB_Status_AppendText_Nextline(Create_Table_CMD, Color.Blue);

                    SQLiteCommand command = new SQLiteCommand(Create_Table_CMD, sqliteCon);
                    command.ExecuteNonQuery();
                }
            }
        }


        private void Fill_DB_From_DGVs(string DB_Name)
        {
            string connectionString = "Data Source=" + Directory.GetCurrentDirectory() + @"\Optic_Measurement\DataBase" + @"\" + DB_Name + ".db;" + " Version=3;";
            using (SQLiteConnection sqliteCon = new SQLiteConnection(connectionString))
            {
                sqliteCon.Open();

                for (int dgv_index = 0; dgv_index < DGVs.Count; dgv_index++)
                {
                    for (int dgv_row = 0; dgv_row < DGVs[dgv_index].Rows.Count - 1; dgv_row++)
                    {
                        string Insert_SQL_CMD = Get_Insert_SQL_CMD(dgv_row, dgv_index);
                        f1().GB_Status_AppendText_Nextline(Insert_SQL_CMD, Color.DarkBlue);

                        SQLiteCommand command = new SQLiteCommand(Insert_SQL_CMD, sqliteCon);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }


        private string Get_Insert_SQL_CMD(int dgv_row,int dgv_index)
        {
            string table_name = "datagridview" + (dgv_index + 1).ToString();
            StringBuilder insert_column_sb = new StringBuilder();
            insert_column_sb.Append("INSERT INTO " + table_name + "VALUES (");
            for (int col = 0; col < DGVs[dgv_index].Columns.Count; col++)
            {
                insert_column_sb.Append(DGVs[dgv_index].Rows[dgv_row].Cells[col].Value);

                if (col != DGVs[dgv_index].Columns.Count - 1)
                    insert_column_sb.Append(", ");
                else
                    insert_column_sb.Append(");");
            }
            return insert_column_sb.ToString();
        }

        public void Load_Data_From_DB(string DB_Name)
        {

        }

        private string Get_Fill_Table_SQL_CMD(int DGV_index)
        {
            //Delta E3
            if (DGV_index == 1)
                return "CREATE TABLE datagridview1 (Gray int,   X_1 real,Y_1 real,Lv_1 real,E3_1 real,   X_2 real,Y_2 real,Lv_2 real,E3_2 real,   X_3 real,Y_3 real,Lv_3 real,E3_3 real,  X_4 real,Y_4 real,Lv_4 real,E3_4 real,   X_5 real,Y_5 real,Lv_5 real,E3_5 real,   X_6 real,Y_6 real,Lv_6 real,E3_6 real,   X_7 real,Y_7 real,Lv_7 real,E3_7 real,   X_8 real,Y_8 real,Lv_8 real,E3_8 real,   X_9 real,Y_9 real,Lv_9 real,E3_9 real,   X_10 real,Y_10 real,Lv_10 real,E3_10 real);";
            if (DGV_index == 2)
                return "CREATE TABLE datagridview2 (Gray int,   X_1 real,Y_1 real,Lv_1 real,E3_1 real,   X_2 real,Y_2 real,Lv_2 real,E3_2 real,   X_3 real,Y_3 real,Lv_3 real,E3_3 real,  X_4 real,Y_4 real,Lv_4 real,E3_4 real,   X_5 real,Y_5 real,Lv_5 real,E3_5 real,   X_6 real,Y_6 real,Lv_6 real,E3_6 real,   X_7 real,Y_7 real,Lv_7 real,E3_7 real,   X_8 real,Y_8 real,Lv_8 real,E3_8 real,   X_9 real,Y_9 real,Lv_9 real,E3_9 real,   X_10 real,Y_10 real,Lv_10 real,E3_10 real);";
            if (DGV_index == 3)
                return "CREATE TABLE datagridview3 (Gray int,   X_1 real,Y_1 real,Lv_1 real,E3_1 real,   X_2 real,Y_2 real,Lv_2 real,E3_2 real,   X_3 real,Y_3 real,Lv_3 real,E3_3 real,  X_4 real,Y_4 real,Lv_4 real,E3_4 real,   X_5 real,Y_5 real,Lv_5 real,E3_5 real,   X_6 real,Y_6 real,Lv_6 real,E3_6 real,   X_7 real,Y_7 real,Lv_7 real,E3_7 real,   X_8 real,Y_8 real,Lv_8 real,E3_8 real,   X_9 real,Y_9 real,Lv_9 real,E3_9 real,   X_10 real,Y_10 real,Lv_10 real,E3_10 real);";

            //Delta E2
            if (DGV_index == 4)
                return "CREATE TABLE datagridview4 (DBV int,   X_1 real,Y_1 real,Lv_1 real,E2_1 real,   X_2 real,Y_2 real,Lv_2 real,E2_2 real,   X_3 real,Y_3 real,Lv_3 real,E2_3 real,   X_4 real,Y_4 real,Lv_4 real,E2_4 real,   X_5 real,Y_5 real,Lv_5 real,E2_5 real,   X_6 real,Y_6 real,Lv_6 real,E2_6 real,   X_7 real,Y_7 real,Lv_7 real,E2_7 real,   X_8 real,Y_8 real,Lv_8 real,E2_8 real,   X_9 real,Y_9 real,Lv_9 real,E2_9 real,   X_10 real,Y_10 real,Lv_10 real,E2_10 real);";
            if (DGV_index == 5)
                return "CREATE TABLE datagridview5 (DBV int,   X_1 real,Y_1 real,Lv_1 real,E2_1 real,   X_2 real,Y_2 real,Lv_2 real,E2_2 real,   X_3 real,Y_3 real,Lv_3 real,E2_3 real,   X_4 real,Y_4 real,Lv_4 real,E2_4 real,   X_5 real,Y_5 real,Lv_5 real,E2_5 real,   X_6 real,Y_6 real,Lv_6 real,E2_6 real,   X_7 real,Y_7 real,Lv_7 real,E2_7 real,   X_8 real,Y_8 real,Lv_8 real,E2_8 real,   X_9 real,Y_9 real,Lv_9 real,E2_9 real,   X_10 real,Y_10 real,Lv_10 real,E2_10 real);";
            if (DGV_index == 6)
                return "CREATE TABLE datagridview6 (DBV int,   X_1 real,Y_1 real,Lv_1 real,E2_1 real,   X_2 real,Y_2 real,Lv_2 real,E2_2 real,   X_3 real,Y_3 real,Lv_3 real,E2_3 real,   X_4 real,Y_4 real,Lv_4 real,E2_4 real,   X_5 real,Y_5 real,Lv_5 real,E2_5 real,   X_6 real,Y_6 real,Lv_6 real,E2_6 real,   X_7 real,Y_7 real,Lv_7 real,E2_7 real,   X_8 real,Y_8 real,Lv_8 real,E2_8 real,   X_9 real,Y_9 real,Lv_9 real,E2_9 real,   X_10 real,Y_10 real,Lv_10 real,E2_10 real);";

            //GCS Diff
            if (DGV_index == 7)
                return "CREATE TABLE datagridview7 (Gray int,   X_1 real,Y_1 real,Lv_1 real,   X_2 real,Y_2 real,Lv_2 real,   X_3 real,Y_3 real,Lv_3 real,   X_4 real,Y_4 real,Lv_4 real,   X_5 real,Y_5 real,Lv_5 real,   X_6 real,Y_6 real,Lv_6 real,   X_7 real,Y_7 real,Lv_7 real,   X_8 real,Y_8 real,Lv_8 real,   X_9 real,Y_9 real,Lv_9 real,   X_10 real,Y_10 real,Lv_10 real);";
            if (DGV_index == 8)
                return "CREATE TABLE datagridview8 (Gray int,   X_1 real,Y_1 real,Lv_1 real,   X_2 real,Y_2 real,Lv_2 real,   X_3 real,Y_3 real,Lv_3 real,   X_4 real,Y_4 real,Lv_4 real,   X_5 real,Y_5 real,Lv_5 real,   X_6 real,Y_6 real,Lv_6 real,   X_7 real,Y_7 real,Lv_7 real,   X_8 real,Y_8 real,Lv_8 real,   X_9 real,Y_9 real,Lv_9 real,   X_10 real,Y_10 real,Lv_10 real);";
            if (DGV_index == 9)
                return "CREATE TABLE datagridview9 (Gray int,   X_1 real,Y_1 real,Lv_1 real,   X_2 real,Y_2 real,Lv_2 real,   X_3 real,Y_3 real,Lv_3 real,   X_4 real,Y_4 real,Lv_4 real,   X_5 real,Y_5 real,Lv_5 real,   X_6 real,Y_6 real,Lv_6 real,   X_7 real,Y_7 real,Lv_7 real,   X_8 real,Y_8 real,Lv_8 real,   X_9 real,Y_9 real,Lv_9 real,   X_10 real,Y_10 real,Lv_10 real);";

            //BCS Diff
            if (DGV_index == 10)
                return "CREATE TABLE datagridview10 (DBV int,   X_1 real,Y_1 real,Lv_1 real,   X_2 real,Y_2 real,Lv_2 real,   X_3 real,Y_3 real,Lv_3 real,   X_4 real,Y_4 real,Lv_4 real,   X_5 real,Y_5 real,Lv_5 real,   X_6 real,Y_6 real,Lv_6 real,   X_7 real,Y_7 real,Lv_7 real,   X_8 real,Y_8 real,Lv_8 real,   X_9 real,Y_9 real,Lv_9 real,   X_10 real,Y_10 real,Lv_10 real);";
            if (DGV_index == 11)
                return "CREATE TABLE datagridview11 (DBV int,   X_1 real,Y_1 real,Lv_1 real,   X_2 real,Y_2 real,Lv_2 real,   X_3 real,Y_3 real,Lv_3 real,   X_4 real,Y_4 real,Lv_4 real,   X_5 real,Y_5 real,Lv_5 real,   X_6 real,Y_6 real,Lv_6 real,   X_7 real,Y_7 real,Lv_7 real,   X_8 real,Y_8 real,Lv_8 real,   X_9 real,Y_9 real,Lv_9 real,   X_10 real,Y_10 real,Lv_10 real);";
            if (DGV_index == 12)
                return "CREATE TABLE datagridview12 (DBV int,   X_1 real,Y_1 real,Lv_1 real,   X_2 real,Y_2 real,Lv_2 real,   X_3 real,Y_3 real,Lv_3 real,   X_4 real,Y_4 real,Lv_4 real,   X_5 real,Y_5 real,Lv_5 real,   X_6 real,Y_6 real,Lv_6 real,   X_7 real,Y_7 real,Lv_7 real,   X_8 real,Y_8 real,Lv_8 real,   X_9 real,Y_9 real,Lv_9 real,   X_10 real,Y_10 real,Lv_10 real);";

            //AOD
            if (DGV_index == 13)
                return "CREATE TABLE datagridview13 (Gray int,   X_1 real,Y_1 real,Lv_1 real,   X_2 real,Y_2 real,Lv_2 real,   X_3 real,Y_3 real,Lv_3 real,   X_4 real,Y_4 real,Lv_4 real,   X_5 real,Y_5 real,Lv_5 real,   X_6 real,Y_6 real,Lv_6 real,   X_7 real,Y_7 real,Lv_7 real,   X_8 real,Y_8 real,Lv_8 real,   X_9 real,Y_9 real,Lv_9 real,   X_10 real,Y_10 real,Lv_10 real);";

            //Delta E4
            if (DGV_index == 14)
                return "CREATE TABLE datagridview14 (PTN int,   X_1 real,Y_1 real,Lv_1 real,E4_1 real,   X_2 real,Y_2 real,Lv_2 real,E4_2 real,   X_3 real,Y_3 real,Lv_3 real,E4_3 real,  X_4 real,Y_4 real,Lv_4 real,E4_4 real,   X_5 real,Y_5 real,Lv_5 real,E4_5 real,   X_6 real,Y_6 real,Lv_6 real,E4_6 real,   X_7 real,Y_7 real,Lv_7 real,E4_7 real,   X_8 real,Y_8 real,Lv_8 real,E4_8 real,   X_9 real,Y_9 real,Lv_9 real,E4_9 real,   X_10 real,Y_10 real,Lv_10 real,E4_10 real);";

            //TBD
            //if (DGV_index == 15)
                return "CREATE TABLE datagridview15 (TBD int,   X_1 real,Y_1 real,Lv_1 real,   X_2 real,Y_2 real,Lv_2 real,   X_3 real,Y_3 real,Lv_3 real,   X_4 real,Y_4 real,Lv_4 real,   X_5 real,Y_5 real,Lv_5 real,   X_6 real,Y_6 real,Lv_6 real,   X_7 real,Y_7 real,Lv_7 real,   X_8 real,Y_8 real,Lv_8 real,   X_9 real,Y_9 real,Lv_9 real,   X_10 real,Y_10 real,Lv_10 real);";
        }




    }
}
