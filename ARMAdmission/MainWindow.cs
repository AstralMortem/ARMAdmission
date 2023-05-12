using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;



namespace ARMAdmission
{
    public partial class MainWindow : Form
    {
        SqlConnection connection;
        string curren_abit_id { get; set; }
        public MainWindow(SqlConnection connection, bool isUserAdmin)
        {
            this.connection = connection;
            InitializeComponent();
            if (!isUserAdmin)
            {

                this.tabControl2.TabPages.Remove(abit_add_page);
                this.tabControl2.TabPages.Remove(abit_mark_page);
                this.tabControl1.TabPages.Remove(dictionary_page);
                this.tabControl1.TabPages.Remove(competition_page);
                this.tabControl3.TabPages.Remove(statement_add_page);
                
            }

            this.init_combo_box(); // init combo boxes value/key
            this.init_list_box(); // init all list boxes value/key
            this.init_abit_docs_checkbox(); //init abiturient docs


        }

        private void AutocompleteBuilder(ComboBox text_box, string query) //custom suggest builder for combo box
        {
            var text = text_box.Text;
            SqlCommand com = new SqlCommand(query, this.connection);
            var res = com.ExecuteReader();
            while (res.Read())
            {
                text_box.Items.Add(res.GetString(0));
            }
            res.Close();
            text_box.Text = text;
            text_box.SelectionStart = text.Length;
        }
        private void AutocompleteDictBuilder(ComboBox text_box, string query) //custom suggest builder for combo box in key/value (id/text)
        {
            var text = text_box.Text;
            SqlCommand com = new SqlCommand(query, this.connection);
            List<KeyValuePair<string, string>> data_list = new List<KeyValuePair<string, string>>();
            var res = com.ExecuteReader();
            while (res.Read())
            {
                data_list.Add(new KeyValuePair<string, string>(res.GetString(1), res.GetString(0)));

            }
            res.Close();
            text_box.DataSource = data_list;
            text_box.Text = text;
            text_box.SelectionStart = text.Length;
        }

        private void AutocompleteListBuilde(ListBox list_box, SqlCommand com) //custom suggest builder for combo box in key/value (id/text)
        {
            var text = list_box.Text;
            List<KeyValuePair<string, string>> data_list = new List<KeyValuePair<string, string>>();
            var res = com.ExecuteReader();
            while (res.Read())
            {
                data_list.Add(new KeyValuePair<string, string>($"{res.GetString(1)} {res.GetString(2)} {res.GetString(3)}", res.GetString(0)));

            }
            res.Close();
            list_box.DataSource = data_list;

        }
        private void AutocompleteListDictBuilder(ListBox list_box, SqlCommand com) //custom suggest builder for combo box in key/value (id/text)
        {
            var text = list_box.Text;
            List<KeyValuePair<string, string>> data_list = new List<KeyValuePair<string, string>>();
            var res = com.ExecuteReader();
            while (res.Read())
            {
                data_list.Add(new KeyValuePair<string, string>(res.GetString(1), res.GetString(0)));

            }
            res.Close();
            list_box.DataSource = data_list;

        }

        private void init_combo_box() // add combo_box value
        {
            //init gender box 
            this.gender_box.DisplayMember = "Text";
            this.gender_box.ValueMember = "Value";
            var items = new[]
            {
                new {Text="Чоловічий", Value="m"}, //m - male
                new {Text="Жіночий", Value="f"}, //f - female
            };
            this.gender_box.DataSource = items;

            //init country_box

            this.country_box.DisplayMember = "Key";
            this.country_box.ValueMember = "Value";

            //init region_box
            this.region_box.DisplayMember = "key";
            this.region_box.ValueMember = "value";

            //init exam mark box
            this.exam_mark_box.DisplayMember = "key";
            this.exam_mark_box.ValueMember = "value";



        }

        private void init_list_box()
        {
            this.abit_mark_listbox.DisplayMember = "Key";
            this.abit_mark_listbox.ValueMember = "Value";
        }
        private void init_abit_docs_checkbox()
        {
            passport_input.ReadOnly = true;
            ipn_input.ReadOnly = true;
            sertificate_input.ReadOnly = true;
            sertificate_seria_input.ReadOnly = true;
            zno_input.ReadOnly = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage15_Click(object sender, EventArgs e)
        {

        }

        private void tabPage14_Click(object sender, EventArgs e)
        {

        }


        //abiturient page Input actions handlers
        private void last_name_box_TextUpdate(object sender, EventArgs e) //event to select abit last name
        {
            try
            {
                string q = $"SELECT TOP 10 last_name from abit where last_name like '{last_name_box.Text}%'";
                this.AutocompleteBuilder(this.last_name_box, q);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }

        }

        private void first_name_box_TextUpdate(object sender, EventArgs e) //event to select abit first name
        {
            try
            {
                string q = $"SELECT TOP 10 first_name from abit where first_name like '{first_name_box.Text}%'";
                this.AutocompleteBuilder(this.first_name_box, q);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void middle_name_box_VisibleChanged(object sender, EventArgs e) //event to select abit middle name
        {
            try
            {
                string q = $"SELECT TOP 10 midle_name from abit where midle_name like '{middle_name_box.Text}%'";
                this.AutocompleteBuilder(this.middle_name_box, q);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void country_box_TextUpdate(object sender, EventArgs e) //country box suggestion
        {
            try
            {
                string q = $"SELECT TOP 20 * from country where country_name like '{country_box.Text}%'";
                this.AutocompleteDictBuilder(this.country_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void region_box_TextUpdate(object sender, EventArgs e) //when input region
        {
            string q = $"SELECT TOP 20 state_id, state_name from state where state_name like '{region_box.Text}%' and country_id = '{country_box.SelectedValue}' ";
            this.AutocompleteDictBuilder(this.region_box, q);
        }

        private void generate_region_id(ComboBox region_box)
        {
            try
            {
                if (region_box.SelectedValue == null)
                {
                    string q = "SELECT TOP 1 state_id FROM state ORDER BY state_id DESC";
                    SqlCommand com = new SqlCommand(q, this.connection);

                    int last_code = Convert.ToInt16((string)com.ExecuteScalar()); // get 001 -> 1
                    string new_code = Convert.ToString(last_code + 1); // 1+1=2
                    for (int i = 0; i < 4 - new_code.Length; i++)
                    {
                        new_code = "0" + new_code; //convert to string -> 002
                    }
                    region_box.DataSource = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(region_box.Text, new_code) }; //change selected value to current item 

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }
        private void region_box_Leave(object sender, EventArgs e) //generate id for Region box data
        {
            this.generate_region_id((ComboBox)sender);
        }

        private void city_box_TextUpdate(object sender, EventArgs e) //city box auto suggest
        {
            try
            {
                string q = $"SELECT TOP 10 city from address where country_id='{country_box.SelectedValue}' and state_id='{region_box.SelectedValue}'";
                this.AutocompleteBuilder(this.city_box, q);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }

        }

        private void street_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 10 street from address where country_id='{country_box.SelectedValue}' and state_id='{region_box.SelectedValue}'";
                this.AutocompleteBuilder(this.street_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        private string CreateFullAddress(ComboBox country_box,
                                         ComboBox region_box,
                                         ComboBox city_box,
                                         ComboBox street_box,
                                         TextBox build_input,
                                         TextBox flat_input)
        {
            SqlCommand com = new SqlCommand("AddFullAddress", this.connection);
            com.CommandType = CommandType.StoredProcedure;
            string address_id = $"{country_box.SelectedValue}/{region_box.SelectedValue}/{Transliteration.TranslateWord(city_box.Text)}/{Transliteration.TranslateWord(street_box.Text)}/{Transliteration.TranslateWord(build_input.Text)}/{Transliteration.TranslateWord(flat_input.Text)}";

            com.Parameters.AddWithValue("ID", address_id);
            com.Parameters.AddWithValue("Region_id", region_box.SelectedValue);
            com.Parameters.AddWithValue("Country_id", country_box.SelectedValue);
            com.Parameters.AddWithValue("Region_name", region_box.Text);
            com.Parameters.AddWithValue("City", city_box.Text);
            com.Parameters.AddWithValue("Street", street_box.Text);
            com.Parameters.AddWithValue("Build", build_input.Text);
            com.Parameters.AddWithValue("Flat", flat_input.Text);
            try
            {
                com.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }

            return address_id;
        }

        private string CreateAbiturient(string address_id)
        {
            string abit_id = GenerateAbitId();
            SqlCommand com = new SqlCommand("AddAbiturient", this.connection);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("ID", abit_id);
            com.Parameters.AddWithValue("FirstName", first_name_box.Text);
            com.Parameters.AddWithValue("LastName", last_name_box.Text);
            com.Parameters.AddWithValue("MiddleName", middle_name_box.Text);
            com.Parameters.AddWithValue("Gender", gender_box.SelectedValue);
            com.Parameters.AddWithValue("BirthDay", birthday_picker.Value);
            com.Parameters.AddWithValue("Phone", phone_input.Text);
            com.Parameters.AddWithValue("Email", mail_input.Text);
            com.Parameters.AddWithValue("Mark", attestation_mark_input.Text);
            com.Parameters.AddWithValue("Entered", 0);
            com.Parameters.AddWithValue("Address_ID", address_id);
            com.Parameters.AddWithValue("Passport", passport_input.Text);
            com.Parameters.AddWithValue("IPN", ipn_input.Text);
            com.Parameters.AddWithValue("Attestation_ID", sertificate_input.Text);
            com.Parameters.AddWithValue("Attestation_Seria", sertificate_seria_input.Text);
            com.Parameters.AddWithValue("Exam_Card", zno_input.Text);

            try
            {
                com.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return abit_id;
        }
        private string GenerateAbitId()
        {

            string q = "SELECT TOP 1 abit_id FROM abit ORDER BY abit_id DESC";
            SqlCommand com = new SqlCommand(q, this.connection);

            long last_code = Convert.ToInt64((string)com.ExecuteScalar()); //get ->0000000001
            string new_code = Convert.ToString(last_code + 1); // 1+1=2
            for (int i = 0; i < 11 - new_code.Length; i++)
            {
                new_code = "0" + new_code; //convert to string -> 0000000002
            }

            return new_code;


        }

        private void clear_abit_page_btn_Click(object sender, EventArgs e)
        {
            first_name_box.Text = "";
            last_name_box.Text = "";
            middle_name_box.Text = "";
            country_box.Text = "";
            region_box.Text = "";
            city_box.Text = "";
            street_box.Text = "";
            build_input.Text = "";
            flat_input.Text = "";

            passport_input.Text = "";
            ipn_input.Text = "";
            attestation_mark_input.Text = "";
            sertificate_input.Text = "";
            sertificate_seria_input.Text = "";
            zno_input.Text = "";
            phone_input.Text = "";
            mail_input.Text = "";
        }


        //abiturient check box actions
        private void passport_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            passport_input.ReadOnly = !passport_checkbox.Checked;
        }

        private void ipn_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            ipn_input.ReadOnly = !ipn_checkbox.Checked;
        }

        private void attestation_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            sertificate_input.ReadOnly = !attestation_checkbox.Checked;
            sertificate_seria_input.ReadOnly = !attestation_checkbox.Checked;
        }

        private void zno_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            zno_input.ReadOnly = !zno_checkbox.Checked;
        }

        //------------------------------------------------------------------------------

        private void saveabit_and_redirect_btn_Click(object sender, EventArgs e)
        {
            this.SaveAbiturient();
            abit_marks_search.Text = this.curren_abit_id;
            search_abit_mark_btn.PerformClick();
            tabControl2.SelectTab(1);
        }

        private void saveabit_btn_Click(object sender, EventArgs e) //Save abiturient to db
        {
            this.SaveAbiturient();
        }

        private void SaveAbiturient()
        {
            try
            {
                string address_id = this.CreateFullAddress(this.country_box,
                                                           this.region_box,
                                                           this.city_box,
                                                           this.street_box,
                                                           this.build_input,
                                                           this.flat_input);
                string abit_id = this.CreateAbiturient(address_id);
                MessageBox.Show($"Абітурієнт з номером {abit_id} успішно доданий");
                this.curren_abit_id = abit_id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void search_abit_mark_btn_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand com = new SqlCommand("FindAbit", this.connection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("SearchString", abit_marks_search.Text);
                this.AutocompleteListBuilde(abit_mark_listbox, com);

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void abit_marks_clear_btn_Click(object sender, EventArgs e)
        {

            abit_marks_search.Text = string.Empty;
            abit_mark_listbox.DataSource = null;

        }

        private void abit_mark_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (abit_mark_listbox.SelectedValue != null)
                {
                    exam_mark_box.Items.Clear();
                    abit_mark_datagrid.Rows.Clear();
                    string q = "SELECT exam_name from exam";
                    this.AutocompleteBuilder(exam_mark_box, q);



                    SqlCommand com = new SqlCommand("SelectAbitMarks", this.connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("ID", abit_mark_listbox.SelectedValue));

                    var res = com.ExecuteReader();
                    while (res.Read())
                    {
                        abit_mark_datagrid.Rows.Add(res.GetString(0), res.GetInt32(1));
                    }
                    res.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public static class Transliteration
        {
            static Dictionary<string, string> table_char = new Dictionary<string, string>() {
            {"а", "a" },
            {"б","b" },
            {"в","v" },
            {"г","h" },
            {"д","d" },
            {"е","e" },
            {"є","ie" },
            {"ж","zh" },
            {"з","z" },
            {"и","y" },
            {"і","i" },
            {"ї","i" },
            {"й","i" },
            {"к","k" },
            {"л","l" },
            {"м","m" },
            {"н","n" },
            {"о","o" },
            {"п","p" },
            {"р","r" },
            {"с","s" },
            {"т","t" },
            {"у","u" },
            {"ф","f" },
            {"х","kh" },
            {"ц","ts" },
            {"ч","ch" },
            {"ш","sh" },
            {"щ","shch" },
            {"ю","iu" },
            {"я","ia" },
        };

            static Dictionary<string, string> extra_table_char = new Dictionary<string, string>() {

            {"є", "ye" },
            {"ї", "yi" },
            {"ю", "yu" },
            {"я", "ya" },
        };

            public static string TranslateWord(string str)
            {
                string result = "";
                string ch = "";

                for (int i = 0; i < str.Length; i++)
                {

                    if (i == 0)
                    {
                        if (!extra_table_char.TryGetValue(str[i].ToString().ToLower(), out ch))
                        {
                            if (!table_char.TryGetValue(str[i].ToString().ToLower(), out ch))
                            {
                                ch = str[i].ToString().ToLower();
                            }
                        }

                        if (Char.IsUpper(str[i]))
                        {
                            ch = FirstCharToUpper(ch);
                        }
                        result += ch;
                    }
                    else
                    {

                        if (!table_char.TryGetValue(str[i].ToString(), out ch))
                        {
                            ch += str[i].ToString();
                        }
                        result += ch;
                    }
                }
                return result;
            }

            public static string TranslateSentence(string str)
            {

                string[] words = str.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = TranslateWord(words[i]);
                }

                return string.Join(" ", words);

            }
            public static string FirstCharToUpper(string input)
            {
                string res = "";
                for (int i = 0; i < input.Length; i++)
                {
                    if (i == 0)
                    {
                        res += Char.ToUpper(input[i]);
                        continue;
                    }
                    res += input[i];
                }
                return res;
            }
        }

        private void add_exam_mark_btn_Click(object sender, EventArgs e)
        {
            abit_mark_datagrid.Rows.Add(exam_mark_box.Text, exam_mark_input.Text);
        }

        private void save_abit_marks_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand com = new SqlCommand("AddAbitMarks", this.connection);
                com.CommandType = CommandType.StoredProcedure;



                int row_count = abit_mark_datagrid.RowCount;
                for (int i = 0; i < row_count; i++)
                {
                    string exam_name = abit_mark_datagrid[0, i].Value.ToString();
                    string exam_mark = abit_mark_datagrid[1, i].Value.ToString();
                    com.Parameters.Clear();
                    com.Parameters.AddWithValue("ID", abit_mark_listbox.SelectedValue);
                    com.Parameters.AddWithValue("Exam_Name", exam_name);
                    com.Parameters.AddWithValue("Mark", Convert.ToInt16(exam_mark));

                    com.ExecuteNonQuery();
                    
                    
                    
                }
                MessageBox.Show("Оцінки добавлено");
                tabControl1.SelectTab(2);
                statement_search_input.Text = this.curren_abit_id;

                abit_marks_search.Text = string.Empty;
                abit_mark_listbox.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dict_uni_search_btn_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand com = new SqlCommand("FindUniversity", this.connection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("SearchString", dict_uni_search_input.Text);
                this.AutocompleteListDictBuilder(dict_uni_listbox, com);

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void SaveDictUniversity()
        {
            try
            {

                string address_id = this.CreateFullAddress(this.dict_country_box,
                                                               this.dict_region_box,
                                                               this.dict_city_box,
                                                               this.dict_street_box,
                                                               this.dict_build_input,
                                                               this.dict_build_input);
                SqlCommand com = new SqlCommand("AddUniversity", this.connection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("ID", dict_uni_establishment.Value.ToString());
                com.Parameters.AddWithValue("Name", dict_uni_fullname.Text);
                com.Parameters.AddWithValue("RecFullName", dict_uni_rector.Text);
                com.Parameters.AddWithValue("Phone", dict_uni_rectorphone.Text);
                com.Parameters.AddWithValue("Date", dict_uni_establishment.Value.ToString());
                com.Parameters.AddWithValue("Address_id", address_id);

                com.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dict_uni_save_btn_Click(object sender, EventArgs e)
        {
            this.SaveDictUniversity();
            this.clearDictUni();
        }

        private void dict_country_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 20 * from country where country_name like '{dict_country_box.Text}%'";
                this.AutocompleteDictBuilder(this.dict_country_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void dict_region_box_TextUpdate(object sender, EventArgs e)
        {
            string q = $"SELECT TOP 20 state_id, state_name from state where state_name like '{dict_region_box.Text}%' and country_id = '{dict_country_box.SelectedValue}' ";
            this.AutocompleteDictBuilder(this.dict_region_box, q);
        }

        private void dict_city_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 10 city from address where country_id='{dict_country_box.SelectedValue}' and state_id='{dict_region_box.SelectedValue}'";
                this.AutocompleteBuilder(this.dict_city_box, q);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void dict_street_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 10 street from address where country_id='{dict_country_box.SelectedValue}' and state_id='{dict_region_box.SelectedValue}'";
                this.AutocompleteBuilder(this.dict_street_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void dict_faculty_university_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 20 university_id, university_name from university where university_name like '{dict_faculty_university_box.Text}%'";
                this.AutocompleteDictBuilder(this.dict_faculty_university_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void dict_faculty_search_btn_Click(object sender, EventArgs e)
        {
            SqlCommand com = new SqlCommand("FindFaculty", this.connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("SearchString", dict_faculty_search.Text);
            com.Parameters.AddWithValue("University_id", dict_faculty_university_box.SelectedValue);
            this.AutocompleteListDictBuilder(this.dict_faculty_listbox, com);
        }

        private void saveDictFaculty()
        {
            try
            {
                string address_id = this.CreateFullAddress(this.faculty_country_box,
                                                               this.faculty_region_box,
                                                               this.faculty_city_box,
                                                               this.faculty_street_box,
                                                               this.faculty_build_input,
                                                               this.faculty_build_input);

                SqlCommand com = new SqlCommand("AddFaculty", this.connection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("ID", dict_faculty_id.Text + Transliteration.TranslateWord(dict_faculty_university_box.Text)[1]);
                com.Parameters.AddWithValue("Name", dict_faculty_name.Text);
                com.Parameters.AddWithValue("DeanFullName", dict_faculty_dean.Text);
                com.Parameters.AddWithValue("Mail", dict_faculty_mail.Text);
                com.Parameters.AddWithValue("Phone", dict_faculty_phone.Text);
                com.Parameters.AddWithValue("Address_id", address_id);
                com.Parameters.AddWithValue("University_id", dict_faculty_university_box.SelectedValue);

                com.ExecuteNonQuery();

                MessageBox.Show($"Факультет {dict_faculty_id.Text} успішно добавлений");

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void clearDictUni()
        {
            dict_uni_listbox.DataSource = null;
            dict_uni_fullname.Text = "";
            dict_uni_rector.Text = "";
            dict_uni_rectorphone.Text = "";
            dict_uni_search_input.Text = "";
            dict_build_input.Text = "";
            dict_country_box.Text = "";
            dict_region_box.Text = "";
            dict_city_box.Text = "";
            dict_street_box.Text = "";
        }
        private void faculty_country_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 20 * from country where country_name like '{faculty_country_box.Text}%'";
                this.AutocompleteDictBuilder(this.faculty_country_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void faculty_region_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 20 state_id, state_name from state where state_name like '{faculty_region_box.Text}%' and country_id = '{faculty_country_box.SelectedValue}' ";
                this.AutocompleteDictBuilder(this.faculty_region_box, q);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void faculty_city_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 10 city from address where country_id='{faculty_country_box.SelectedValue}' and state_id='{faculty_region_box.SelectedValue}' and city like '{faculty_city_box.Text}%'";
                this.AutocompleteBuilder(this.faculty_city_box, q);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void faculty_street_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 10 street from address where country_id='{faculty_country_box.SelectedValue}' and state_id='{faculty_region_box.SelectedValue}' and street like '{faculty_street_box.Text}%'";
                this.AutocompleteBuilder(this.faculty_street_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void dict_faculty_save_btn_Click(object sender, EventArgs e)
        {
            this.saveDictFaculty();
            clearDictFaculty();
        }

        private void faculty_save_redirect_btn_Click(object sender, EventArgs e)
        {
            this.saveDictFaculty();
            clearDictFaculty();
            tabControl4.SelectTab(2);
        }

        private void dict_uni_save_redirect_btn_Click(object sender, EventArgs e)
        {
            this.SaveDictUniversity();
            this.clearDictUni();
            tabControl4.SelectTab(1);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.clearDictUni();
        }

        private void clearDictFaculty()
        {
            dict_faculty_listbox.DataSource = null;
            dict_faculty_dean.Text = "";
            dict_faculty_id.Text = "";
            dict_faculty_name.Text = "";
            dict_faculty_phone.Text = "";
            dict_faculty_mail.Text = "";
            dict_faculty_search.Text = "";
            faculty_build_input.Text = "";
            faculty_country_box.Text = "";
            faculty_region_box.Text = "";
            faculty_city_box.Text = "";
            faculty_street_box.Text = "";
            dict_faculty_university_box.Text = "";

        }

        private void dict_faculty_clear_btn_Click(object sender, EventArgs e)
        {
            clearDictFaculty();
        }

        private void dict_spec_btn_Click(object sender, EventArgs e)
        {
            string q = $"Select spec_id, spec_name from spec where faculty_id = '{dict_spec_faculty_box.SelectedValue}'";
            SqlCommand com = new SqlCommand(q, this.connection);
            this.AutocompleteListDictBuilder(dict_spec_listbox, com);

        }

        private void AddSpec()
        {
            try
            {
                SqlCommand com = new SqlCommand("AddSpec", this.connection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("ID", dict_spec_id.Text);
                com.Parameters.AddWithValue("Faculty_id", dict_spec_faculty_box.SelectedValue);
                com.Parameters.AddWithValue("Name", dict_spec_name.Text);
                com.ExecuteNonQuery();
                MessageBox.Show($"Спеціальність {dict_spec_name.Text} ({dict_spec_id.Text}) успішно добавлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dict_spec_university_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 20 university_id, university_name from university where university_name like '{dict_spec_university_box.Text}%'";
                this.AutocompleteDictBuilder(this.dict_spec_university_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void dict_spec_faculty_box_TextUpdate(object sender, EventArgs e)
        {
            string q = $"SELECT TOP 20 faculty_id, faculty_name from faculty where faculty_name like '{dict_spec_faculty_box.Text}%' and university_id='{dict_spec_university_box.SelectedValue}'";
            this.AutocompleteDictBuilder(this.dict_spec_faculty_box, q);
        }

        private void dict_spec_save_btn_Click(object sender, EventArgs e)
        {
            this.AddSpec();
            this.clearSpec();
        }

        private void clearSpec()
        {
            this.dict_spec_name.Text = "";
            this.dict_spec_id.Text = "";
            dict_spec_listbox.DataSource = null;
            dict_spec_university_box.Text = "";
            dict_spec_faculty_box.Text = "";
        }

        private void dict_spec_clear_btn_Click(object sender, EventArgs e)
        {
            this.clearSpec();
        }

        private void dict_region_box_Leave(object sender, EventArgs e)
        {
            this.generate_region_id((ComboBox)sender);
        }

        private void faculty_region_box_Leave(object sender, EventArgs e)
        {
            this.generate_region_id((ComboBox)sender);
        }

        private void dict_coef_university_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 20 university_id, university_name from university where university_name like '{dict_coef_university_box.Text}%'";
                this.AutocompleteDictBuilder(this.dict_coef_university_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void dict_coef_faculty_box_TextUpdate(object sender, EventArgs e)
        {
            string q = $"SELECT TOP 20 faculty_id, faculty_name from faculty where faculty_name like '{dict_coef_faculty_box.Text}%' and university_id='{dict_coef_university_box.SelectedValue}'";
            this.AutocompleteDictBuilder(this.dict_coef_faculty_box, q);
        }

        private void dict_coef_spec_box_TextUpdate(object sender, EventArgs e)
        {
            string q = $"Select spec_id, spec_name from spec where faculty_id = '{dict_coef_faculty_box.SelectedValue}'";
            this.AutocompleteDictBuilder(this.dict_coef_spec_box, q);
        }

        private void dict_coef_search_btn_Click(object sender, EventArgs e)
        {
            try
            {
                dict_coef_grid.Rows.Clear();
                string q = $"SELECT exam.exam_id, exam.exam_name, koef from exam Inner Join koef on exam.exam_id = koef.exam_id where  koef.spec_id = '{dict_coef_spec_box.SelectedValue}'";
                SqlCommand com = new SqlCommand(q, this.connection);


                var res = com.ExecuteReader();
                while (res.Read())
                {
                    dict_coef_grid.Rows.Add(res.GetString(0), res.GetString(1), res.GetDouble(2));
                }
                res.Close();
            } catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dict_coef_save_btn_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand com = new SqlCommand("AddKoef", this.connection);
                com.CommandType = CommandType.StoredProcedure;

                int row_count = dict_coef_grid.RowCount;
                for (int i = 0; i < row_count; i++)
                {
                    string exam_id = dict_coef_grid[0, i].Value.ToString();
                    double koef = Convert.ToDouble(dict_coef_grid[2, i].Value.ToString());
                    com.Parameters.Clear();
                    com.Parameters.AddWithValue("Exam_id", exam_id);
                    com.Parameters.AddWithValue("Spec_id", dict_coef_spec_box.SelectedValue);
                    com.Parameters.AddWithValue("Koef", koef);
                    com.ExecuteNonQuery();
                }
                MessageBox.Show($"Коефіцієнти {dict_coef_spec_box.Text} для успішно добавлені");
                this.clearKoef();
            }catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }

        private void dict_coef_create_btn_Click(object sender, EventArgs e)
        {
            try
            {
                dict_coef_grid.Rows.Clear();
                string q = $"select * from exam";
                SqlCommand com = new SqlCommand(q, this.connection);


                var res = com.ExecuteReader();
                while (res.Read())
                {
                    dict_coef_grid.Rows.Add(res.GetString(0), res.GetString(1), "");
                }
                res.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dict_coef_clear_btn_Click(object sender, EventArgs e)
        {
            this.clearKoef();
        }
        private void clearKoef()
        {
            dict_coef_grid.Rows.Clear();
            dict_coef_university_box.Text = "";
            dict_coef_faculty_box.Text = "";
            dict_coef_spec_box.Text = "";
        }

        private void statement_search_btn_Click(object sender, EventArgs e)
        {
            try{
                SqlCommand com = new SqlCommand("FindAbit", this.connection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("SearchString", statement_search_input.Text);
                this.AutocompleteListBuilde(statement_abit_listbox, com);

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void statement_abit_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (statement_abit_listbox.SelectedValue != null)
            {
                SqlCommand com = new SqlCommand("SelectAbitMarks", this.connection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("ID", statement_abit_listbox.SelectedValue);
                var res = com.ExecuteReader();
                statement_grid.Rows.Clear();
                while (res.Read())
                {
                    statement_grid.Rows.Add(res.GetString(2), res.GetString(0), res.GetInt32(1), false);
                }
                res.Close();
            }
        }

        private void statement_university_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 20 university_id, university_name from university where university_name like '{statement_university_box.Text}%'";
                this.AutocompleteDictBuilder(this.statement_university_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void statement_faculty_box_TextUpdate(object sender, EventArgs e)
        {
            string q = $"SELECT TOP 20 faculty_id, faculty_name from faculty where faculty_name like '{statement_faculty_box.Text}%' and university_id='{statement_university_box.SelectedValue}'";
            this.AutocompleteDictBuilder(this.statement_faculty_box, q);
        }

        private void statement_spec_box_TextUpdate(object sender, EventArgs e)
        {
            string q = $"Select TOP 20 spec_id, spec_name from spec where faculty_id = '{statement_faculty_box.SelectedValue}' and spec_name LIKE '{statement_spec_box.Text}%'";
            this.AutocompleteDictBuilder(this.statement_spec_box, q);
        }

        private void statement_save_btn_Click(object sender, EventArgs e)
        {
            this.addStatement();
            this.clearStatement(1);
        }

        private void addStatement()
        {
            try
            {
                string card_id = this.createCard();

                SqlCommand com = new SqlCommand("AddSelectedExam", this.connection);
                com.CommandType = CommandType.StoredProcedure;


                int row_count = statement_grid.RowCount;
                for (int i = 0; i < row_count; i++)
                {
                    if (!Convert.ToBoolean(statement_grid[3, i].Value))
                    {
                        continue;
                    }
                    string exam_id = statement_grid[0, i].Value.ToString();

                    com.Parameters.Clear();
                    com.Parameters.AddWithValue("ID", card_id);
                    com.Parameters.AddWithValue("Exam_id", exam_id);
                    com.ExecuteNonQuery();

                }

                MessageBox.Show($"Заявка номер {card_id} успішно добавлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private string createCard()
        {
            string card_id = statement_abit_listbox.SelectedValue.ToString() + DateTime.Now.ToString("mmss");
            try
            {
                SqlCommand com = new SqlCommand("AddCard", this.connection);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("Abit_id", statement_abit_listbox.SelectedValue);
                com.Parameters.AddWithValue("Card_id", card_id);
                com.Parameters.AddWithValue("Date", statement_data.Value);
                com.Parameters.AddWithValue("Spec_id", statement_spec_box.SelectedValue);
                com.Parameters.AddWithValue("Priority", Convert.ToInt32(priority_input.Text));

                com.ExecuteNonQuery();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return card_id;
        }

        private void clearStatement(int index)
        {
            if(index == 1)
            {
                this.statement_abit_listbox.DataSource = null;
                this.statement_search_input.Text = "";
                this.statement_grid.Rows.Clear();

            }
            
            this.statement_spec_box.Text = "";
            this.statement_university_box.Text = "";
            this.statement_faculty_box.Text = "";


        }

        private void statement_clear_btn_Click(object sender, EventArgs e)
        {
            this.clearStatement(1);
        }

        private void statement_save_another_btn_Click(object sender, EventArgs e)
        {
            this.addStatement();
            this.clearStatement(0);
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Прогрма для автоматизації робочого місця секретаря приймальної комісії (created by Vladyslav Chaliuk)");
        }

        private void compet_university_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 20 university_id, university_name from university where university_name like '{compet_university_box.Text}%'";
                this.AutocompleteDictBuilder(this.compet_university_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void compet_faculty_box_TextUpdate(object sender, EventArgs e)
        {
            string q = $"SELECT TOP 20 faculty_id, faculty_name from faculty where faculty_name like '{compet_faculty_box.Text}%' and university_id='{compet_university_box.SelectedValue}'";
            this.AutocompleteDictBuilder(this.compet_faculty_box, q);
        }

        private void compet_spec_box_TextUpdate(object sender, EventArgs e)
        {
            string q = $"Select TOP 20 spec_id, spec_name from spec where faculty_id = '{compet_faculty_box.SelectedValue}' and spec_name LIKE '{compet_spec_box.Text}%'";
            this.AutocompleteDictBuilder(this.compet_spec_box, q);
        }

        private void correction_btn_Click(object sender, EventArgs e)
        {
            try
            {
                correction_data_grid.Rows.Clear();
                SqlCommand com = new SqlCommand("GetSpecCorrection", this.connection);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("Spec_id", compet_spec_box.SelectedValue);
                com.Parameters.AddWithValue("Date_from", correction_data_from.Value);
                com.Parameters.AddWithValue("Date_to", correction_data_to.Value);
                com.Parameters.AddWithValue("Recomend", 0);

                var res = com.ExecuteReader();
                while(res.Read())
                {
                    
                    correction_data_grid.Rows.Add(res.GetString(0), res.GetString(1),res.GetString(2),res.GetString(3),res.GetDouble(4),res.GetInt32(5),res.GetInt32(6),res.GetDateTime(7),false);
                }
                res.Close();


            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void save_correction_btn_Click(object sender, EventArgs e)
        {
            SqlCommand com = new SqlCommand("DoSpecCorrection", this.connection);
            com.CommandType = CommandType.StoredProcedure;
            try
            {
                int row_number = correction_data_grid.RowCount;

                for(int i = 0; i < row_number; i++)
                {
                    
                    if (Convert.ToBoolean(correction_data_grid[8,i].Value.ToString()))
                    {
                        com.Parameters.Clear();
                        com.Parameters.AddWithValue("Card_id", correction_data_grid[0,i].Value.ToString());
                        com.ExecuteNonQuery();
                        
                    }
                }

                MessageBox.Show("Коригування успішно проведено");
                this.clearCorrection();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void clearCorrection()
        {
            correction_data_grid.Rows.Clear();
            compet_university_box.Text = "";
            compet_spec_box.Text = "";
            compet_faculty_box.Text = "";
        }

        private void clear_correction_btn_Click(object sender, EventArgs e)
        {
            this.clearCorrection();
        }

        private void order_university_box_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                string q = $"SELECT TOP 20 university_id, university_name from university where university_name like '{order_university_box.Text}%'";
                this.AutocompleteDictBuilder(this.order_university_box, q);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Помилка");
            }
        }

        private void order_faculty_box_TextUpdate(object sender, EventArgs e)
        {
            string q = $"SELECT TOP 20 faculty_id, faculty_name from faculty where faculty_name like '{order_faculty_box.Text}%' and university_id='{order_university_box.SelectedValue}'";
            this.AutocompleteDictBuilder(this.order_faculty_box, q);
        }

        private void order_spec_box_TextUpdate(object sender, EventArgs e)
        {
            string q = $"Select TOP 20 spec_id, spec_name from spec where faculty_id = '{order_faculty_box.SelectedValue}' and spec_name LIKE '{order_spec_box.Text}%'";
            this.AutocompleteDictBuilder(this.order_spec_box, q);
        }

        private void order_search_Click(object sender, EventArgs e)
        {
            try
            {
                order_datagrid.Rows.Clear();
                SqlCommand com = new SqlCommand("GetSpecCorrection", this.connection);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("Spec_id", order_spec_box.SelectedValue);
                com.Parameters.AddWithValue("Date_from", order_data_from.Value);
                com.Parameters.AddWithValue("Date_to", order_data_to.Value);
                com.Parameters.AddWithValue("Recomend", 1);

                var res = com.ExecuteReader();
                while (res.Read())
                {

                    order_datagrid.Rows.Add(res.GetString(0), res.GetString(1), res.GetString(2), res.GetString(3), res.GetDouble(4), res.GetInt32(5), res.GetInt32(6), res.GetDateTime(7), false);
                }
                res.Close();


            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void order_save_btn_Click(object sender, EventArgs e)
        {


            SqlCommand com = new SqlCommand("makeAbitEntered", this.connection);
            com.CommandType = CommandType.StoredProcedure;
            try
            {
                int row_number = order_datagrid.RowCount;

                for (int i = 0; i < row_number; i++)
                {

                    if (Convert.ToBoolean(order_datagrid[8, i].Value.ToString()))
                    {
                        com.Parameters.Clear();
                        com.Parameters.AddWithValue("Abit_id", order_datagrid[1, i].Value.ToString());
                        com.ExecuteNonQuery();

                    }
                }

                MessageBox.Show("Абітурієнти успішно додано до наказу");
                clearOrder();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }            
        
        private void clearOrder()
        {
            order_datagrid.Rows.Clear();
            order_university_box.Text = "";
            order_spec_box.Text = "";
            order_faculty_box.Text = "";
        }

        private void order_clear_button_Click(object sender, EventArgs e)
        {
            this.clearOrder();
        }
    }
}
