using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace BMP_Console {
    public partial class Notes : Form {
        public Notes() {
            InitializeComponent();
        }

        string bmp_id = string.Empty;
        string orig_notes = string.Empty;

        private void Notes_Load(object sender, EventArgs e) {
            string[] array_text = this.Text.Split(' ');
            bmp_id = array_text[3];
            orig_notes = ReturnMemberNotes(bmp_id);
            textBox1.Text = orig_notes;
            textBox1.SelectionLength = 0;

            this.textBox1.SelectionStart = this.textBox1.Text.Length;

        }

        private void Notes_FormClosing(object sender, FormClosingEventArgs e) {            
            Form1.ShowMembers.MNotes = null;
            //this.Close();
        }


        private void button1_Click(object sender, EventArgs e) {
            if(orig_notes == textBox1.Text) {
                MessageBox.Show("No chamges were made", "Add Notes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1.ShowMembers.MNotes = null;
                this.Close();
            }
            else {
                if (IDExist(bmp_id))
                    SaveNotes(textBox1.Text, bmp_id);
                else {
                    InsertNote(textBox1.Text, bmp_id);
                }
                MessageBox.Show("Note added successfully", "Add Notes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1.ShowMembers.MNotes = null;
                this.Close();
            }

        }

        private string ReturnMemberNotes(string id) {
            string notes = "";
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            try {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select note from bmp.notes where bmp_id='" + id + "'", conn);
                MySqlDataReader ret = cmd.ExecuteReader();
                while (ret.Read()) {
                    notes = ret.GetString(0);
                }

            } catch (Exception e) {
                MessageBox.Show(e.Message, "Show Member Notes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                if (conn != null)
                    conn.Close();
            }
            return notes;

        }

        private bool SaveNotes(string note, string id) {

            bool res = false;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            try {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("update bmp.notes set note='" + note + "' where bmp_id='" + id + "'", conn);
                int ret = cmd.ExecuteNonQuery();
                res = (ret > 0) ? true : false;

            } catch (Exception e) {
                MessageBox.Show(e.Message, "Show Member Notes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                if (conn != null)
                    conn.Close();
            }
            return res;
        }

        private bool IDExist(string id) {
            bool res = false;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            try {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select count(*) from bmp.notes where bmp_id='" + id + "'", conn);
                int ret = Int16.Parse(cmd.ExecuteScalar().ToString());
                res = (ret > 0) ? true : false;

            } catch (Exception e) {
                MessageBox.Show(e.Message, "Show Member Notes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                if (conn != null)
                    conn.Close();
            }
            return res;
        }

        private bool InsertNote(string note, string id) {

            bool res = false;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            try {
                conn.Open();
                
                MySqlCommand cmd = new MySqlCommand("insert into bmp.notes (bmp_id,note) values(@id,@note)", conn);
                cmd.Parameters.Add("@id", MySqlDbType.VarChar, id.Length).Value = id;
                cmd.Parameters.Add("@note", MySqlDbType.VarChar, note.Length).Value = note;

                int ret = cmd.ExecuteNonQuery();
                res = (ret > 0) ? true : false;

            } catch (Exception e) {
                MessageBox.Show(e.Message, "Show Member Notes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                if (conn != null)
                    conn.Close();
            }
            return res;
        }
    }
}
