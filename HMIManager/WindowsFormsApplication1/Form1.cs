using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EasyModbus;
using System.Data.SqlClient;
using System.Threading;
using System.Net.NetworkInformation;
using Modbus;
using System.Diagnostics;

namespace testpanel
{
    public partial class Form1 : Form
    {
        long Crt = 0;
        long Wrg = 0;
        long Rad = 0;
        DateTime startTime;

        ModbusClient svimaster = new ModbusClient();
        SqlConnection connection = new SqlConnection(@"Data Source=192.168.1.11\Towzin;Initial Catalog=Towzin;User ID=towzin;Password=123456");
        SqlCommand command;
        /// شمارنده لیست باکس ای پی ها
        int CounterList = 0;
        int CounterListSql = 0;
        /// برای فعال یا غیر فعال کردن تایمر
        bool timerActive = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        ///لود کردن ای پی ها به صورت دستی
        private void button2_Click(object sender, EventArgs e)
        {
        }

        ///تایمر مربوط به خواندن هر دستگاه
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
            connection = new SqlConnection(@"Data Source=192.168.1.11\Towzin;Initial Catalog=Towzin;User ID=towzin;Password=123456;MultipleActiveResultSets=true");
            connection.Close();
            connection.Open();
            /// مربوط به الگوریتم خواندن کلیه ای پی ها از لیست 
            if (ListIP.Items.Count != 0 & CounterList == 0)
            {
                CounterList = ListIP.Items.Count;
                CounterListSql = CounterList;
            }


            if (CounterList != 0 & timerActive == true)
            {
                lblCorrect.Text = (Crt = Crt + 1).ToString();
                ///کد مر بوط به پینگ کردن ای پی مقصد
                Ping p = new Ping();
                PingReply r;
                string s;
                s = ListIP.Items[CounterList - 1].ToString();


                r = p.Send(s, 1);


                if (r.Status == IPStatus.Success)
                {


                    lblStatus.Text = "connect " + ListIP.Items[CounterList - 1].ToString();
                    svimaster.ConnectionTimeout = 1;
                    s = svimaster.ToString();

                    svimaster.Connect(ListIP.Items[CounterList - 1].ToString(), 502);

                    //  ListErrors.Items.Add(CounterList.ToString() + "s" + DateTime.Now.TimeOfDay.Seconds.ToString() + ":" + DateTime.Now.TimeOfDay.Milliseconds.ToString());
                    int[] Main_ProductCode;
                    // lblStatus.Text = "connect " + ListIP.Items[CounterList - 1].ToString() + " ok";
                    bool[] checkbit = svimaster.ReadCoils(0, 1);// Check bit send from HMI
                                                                //Read Data From HMI
                    if (checkbit[0] == true)
                    {

                        int[] Main_Product_Code = svimaster.ReadHoldingRegisters(2, 12);
                        int[] Weight = svimaster.ReadHoldingRegisters(105, 2);
                        int[] Datee = svimaster.ReadHoldingRegisters(101, 2);
                        int[] Timee = svimaster.ReadHoldingRegisters(103, 2);
                        int[] Oerator_num = svimaster.ReadHoldingRegisters(111, 1);

                        ///1 محصول
                        ///2-49 ضایعات
                        ///50-100 توقف
                        int[] Kind = svimaster.ReadHoldingRegisters(112, 1);//نوع محصول

                        ///کد نمونه
                        int[] AmountHMI1 = svimaster.ReadHoldingRegisters(109, 2);//نمونه

                        /// کد سفارش و کالای مبدا
                        int[] Order_number_Source_HMI = svimaster.ReadHoldingRegisters(107, 2);
                        int[] Product_Code_Source_HMI = svimaster.ReadHoldingRegisters(19, 12);
                        /// کد سفارش و کالای مقصد
                        int[] Order_number_Destination_HMI = svimaster.ReadHoldingRegisters(107, 2);
                        int[] Product_Code_Destination_HMI = svimaster.ReadHoldingRegisters(19, 12);


                        float NetWeightHMI = ModbusClient.ConvertRegistersToFloat(Weight);//وزن 
                        float Amount1 = ModbusClient.ConvertRegistersToFloat(AmountHMI1);//نمونه
                        double DateHMI = ModbusClient.ConvertRegistersToDouble(Datee);//تاریخ
                        double TimeHMI = ModbusClient.ConvertRegistersToDouble(Timee);//ساعت

                        ///کد سفارش مربوط مبدا
                        string Product_Code_Source = ModbusClient.ConvertRegistersToString(Product_Code_Source_HMI, 0, 12);//کد محصول
                        float Order_number_Source = ModbusClient.ConvertRegistersToFloat(Order_number_Source_HMI);//شماره سفارش
                                                                                                                  ///کد سفارش مقصد
                        string Product_Code_Destination = ModbusClient.ConvertRegistersToString(Product_Code_Destination_HMI, 0, 12);//کد محصول
                        float Order_number_Destination = ModbusClient.ConvertRegistersToFloat(Order_number_Destination_HMI);//شماره سفارش


                        DateTime DateFromHMI;

                        //check Order Code
                        command = new SqlCommand("SELECT OrderCode FROM [Order] where OrderCode=" + Order_number_Source.ToString(), connection);
                        var varOrderCode = command.ExecuteScalar();

                        if (varOrderCode == null)
                        {
                            Order_number_Source = 0;
                        }

                        if (DateHMI > 999999)
                        {
                            DateFromHMI = Convert.ToDateTime("20" + DateHMI.ToString().Substring(1, 2) + "-" + DateHMI.ToString().Substring(3, 2) + "-" + DateHMI.ToString().Substring(5, 2) + " " + TimeHMI.ToString().Substring(2, 2) + ":" + TimeHMI.ToString().Substring(4, 2) + ":" + TimeHMI.ToString().Substring(6, 2));
                        }
                        else
                        {
                            DateFromHMI = Convert.ToDateTime("20" + DateHMI.ToString().Substring(0, 2) + "-" + DateHMI.ToString().Substring(2, 2) + "-" + DateHMI.ToString().Substring(4, 2) + " " + TimeHMI.ToString().Substring(2, 2) + ":" + TimeHMI.ToString().Substring(4, 2) + ":" + TimeHMI.ToString().Substring(6, 2));
                        }


                        int temp = Product_Code_Source.IndexOf("\0");
                        Product_Code_Source = Product_Code_Source.Substring(0, temp);

                        if (Product_Code_Source == "\0\0\0\0\0\0\0\0\0\0\0\0")
                        {
                            Product_Code_Source = "99999";
                        }



                        command = new SqlCommand("select ID from Part where PartCode='" + Product_Code_Source + "'", connection);
                        int PartStatus = 0;
                        int OrderStatus = 0;
                        ///جواب دیتابیس آیا کد کالا وجود دارد یا نه

                        var varPartID = command.ExecuteScalar();
                        long PartID = 0;
                        ////آیا کد کالا در دیتابیس وجود دارد
                        if (varPartID != null)
                        {
                            PartID = (long)varPartID;
                        }
                        else
                        {
                            ///در صورتی که وجود ندارد کد پیش فرض تعلق میگیرد
                            PartID = 10011;
                        }


                        int status = 0; //جواب سرور دیتابیس
                        if (PartID == 0)
                        {
                            PartID = 10011;
                            PartStatus = 1;
                        }





                        /////کد کالای ضایعاتی
                        command = new SqlCommand("select PartWesteID from Part where PartCode='" + Product_Code_Source + "'", connection);

                        
                        long PartWasteID = 0;
                        ///جواب دیتابیس آیا کد کالای ضایعاتی وجود دارد یا نه
                        if (command.ExecuteScalar() != null)
                        {
                            if (command.ExecuteScalar().ToString().Trim() != "")
                            {


                                var varPartWasteID = command.ExecuteScalar();




                                ////آیا کد کالای ضایعاتی در دیتابیس وجود دارد
                                if (varPartWasteID != null)
                                {

                                    PartWasteID = (long)varPartWasteID;
                                }

                            }
                        }
                        if(PartWasteID==0)
                        {
                            ///در صورتی که وجود ندارد کد پیش فرض تعلق میگیرد
                            PartWasteID = 30025;
                        }

                        ////شماره سفارش
                        if (Order_number_Source == 0)
                        {

                            Order_number_Source = 99999;
                            OrderStatus = 1;
                        }




                        string Creator = "2b2f093d-19c0-4abd-b4b8-512cdacd97ab";
                        string modifier = "2b2f093d-19c0-4abd-b4b8-512cdacd97ab";

                        ///بررسی اینکه تا الان رکوردی با این زمان درج شده است یا خبر
                        command = new SqlCommand("SELECT OrderCodeID FROM ProductiveDetails where AddDate='" + DateFromHMI + "'", connection);
                        var varTempOrder = command.ExecuteScalar();

                        ///بررسی اینکه تا الان رکوردی با این زمان در جدول توقفات درج شده است یا خبر
                        command = new SqlCommand("SELECT OrderCodeID FROM ProductiveStoppages where AddDate='" + DateFromHMI + "'", connection);
                        var varTempProductiveStopages = command.ExecuteScalar();


                        ////گرفتن شماره خط تولید از ای پی
                        command = new SqlCommand("SELECT ProductionLineID FROM Devices where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                        var varProductionLineID = command.ExecuteScalar();

                        string ProductionLineID = varProductionLineID.ToString();
                        if (Kind[0] == 1 & varTempOrder == null)
                        {
                            command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,ProductionLineID,PartID,OperatorID,IO,Waste,Amount,Amount1,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + Order_number_Source + "," + ProductionLineID + "," + PartID + "," + "10006" + "," + 1 + "," + 0 + "," + NetWeightHMI + "," + Amount1 + "," + 1 + ",'" + Creator + "','" + DateFromHMI + "','" + modifier + "','" + DateFromHMI + "')", connection);

                            int Result = command.ExecuteNonQuery();

                            if (Result != 0 & PartStatus == 0 & OrderStatus == 0)
                            {
                                status = 1;
                            }
                            else
                            {
                                status = 2;
                            }

                        }
                        else if (Kind[0] >= 2 & Kind[0] <= 49 & varTempOrder == null)
                        {
                            command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,ProductionLineID,PartID,FromPartID,OperatorID,IO,Waste,Amount,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + Order_number_Source + "," + ProductionLineID + "," + PartWasteID + "," + PartID + "," + "10006" + "," + 1 + "," + 1 + "," + NetWeightHMI + "," + 1 + ",'" + Creator + "','" + DateFromHMI + "','" + modifier + "','" + DateFromHMI + "')", connection);

                            int Result = command.ExecuteNonQuery();

                            if (Result != 0 & PartStatus == 0 & OrderStatus == 0)
                            {
                                status = 1;
                            }
                            else
                            {
                                status = 2;
                            }

                        }

                        else if (Kind[0] >= 50 & Kind[0] <= 100 & varTempProductiveStopages == null)
                        {


                            command = new SqlCommand("SELECT ID FROM Stoppages where Description='" + Kind[0] + "'", connection);

                            var VarStoppagesID = command.ExecuteScalar();
                            long StoppagesID = 0;
                            if (VarStoppagesID == null)
                            {
                                StoppagesID = 99;

                            }
                            else
                            {

                                StoppagesID = (long)VarStoppagesID;
                            }

                            if (Kind[0] < 100)
                            {
                                command = new SqlCommand("INSERT INTO [dbo].[ProductiveStoppages] ([StoppagesID],[OrderCodeID],[OperatorID],[StartTime],[State],[Creator],[AddDate],[LastModifier],[LastModificationDate]) VALUES(" + StoppagesID + "," + Order_number_Source + "," + "10006" + ",'" + DateFromHMI + "'," + 1 + ",'" + Creator + "','" + DateFromHMI + "','" + modifier + "','" + DateFromHMI + "')", connection);
                            }
                            else
                            {
                                command = new SqlCommand("INSERT INTO [dbo].[ProductiveStoppages] ([StoppagesID],[OrderCodeID],[OperatorID],[EndTime],[State],[Creator],[AddDate],[LastModifier],[LastModificationDate]) VALUES(" + StoppagesID + "," + Order_number_Source + "," + "10006" + ",'" + DateFromHMI + "'," + 1 + ",'" + Creator + "','" + DateFromHMI + "','" + modifier + "','" + DateFromHMI + "')", connection);
                            }
                            int Result = command.ExecuteNonQuery();

                            if (Result != 0 & OrderStatus == 0)
                            {
                                status = 1;
                            }
                            else
                            {
                                status = 2;
                            }

                        }
                        else if (Kind[0] == 101 & varTempOrder == null)
                        {

                            command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,PartID,OperatorID,IO,Waste,Amount,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + Order_number_Source + "," + PartID + "," + "10006" + "," + 0 + "," + 0 + "," + NetWeightHMI * (-1) + "," + 1 + ",'" + Creator + "','" + DateFromHMI + "','" + modifier + "','" + DateFromHMI + "')", connection);

                            int Result = command.ExecuteNonQuery();

                            if (Result != 0 & PartStatus == 0 & OrderStatus == 0)
                            {
                                status = 1;
                            }
                            else
                            {
                                status = 2;
                            }

                        }
                        if (Kind[0] == 0)
                        {
                            status = 2;
                        }
                        if (varTempOrder != null || varTempProductiveStopages != null)
                        {
                            status = 1;
                        }

                        Rad = Rad + 1;
                        lblRead.Text = Rad.ToString();


                        /*Reply From SQL Sever
                        Status=0 Unable to Saved to SQL Server
                        Status=1 Save to SQL 
                        Status=2 Error in Data*/


                        svimaster.WriteSingleRegister(113, status);

                        checkbit[0] = false;

                    }

                    long OrderCodeSource = 0;
                    long OrderCodeDestination = 0;
                    ///خواندن اینکه دستگاه تازه روشن شده است یا خیر اگه تازه روشن شده باشد 0 است
                    bool[] RestartHMI = svimaster.ReadCoils(5, 1);

                    command = new SqlCommand("SELECT SendOrderCode FROM Devices where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                    var varSendOrderCode = command.ExecuteScalar();
                    bool SendOrderCode = (bool)varSendOrderCode;
                    if (SendOrderCode == true || RestartHMI[0] == false)
                    {
                        ////ارسال لیست شماره شماره سفارش مبدا
                        int addressOrder = 371;
                        int addressLine = 299;
                        command = new SqlCommand("SELECT OrderCode,ProductionLineLatinName FROM vwDeviceOrders where IP='" + ListIP.Items[CounterList - 1].ToString() + "' and SendOrderCode=1", connection);

                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                OrderCodeSource = (long)reader.GetInt64(reader.GetOrdinal("OrderCode"));
                                float Main_order = OrderCodeSource;//شماره سفارش از سروربه پنل
                                int[] floatreg = ModbusClient.ConvertFloatToTwoRegisters(Main_order);
                                svimaster.WriteMultipleRegisters(addressOrder, floatreg);
                                addressOrder = addressOrder + 2;

                                string Main_ProductionLineLatinName = (string)reader.GetString(reader.GetOrdinal("ProductionLineLatinName"));//شماره سفارش از سروربه پنل
                                int[] ProductionLineLatinName = ModbusClient.ConvertStringToRegisters(Main_ProductionLineLatinName);
                                svimaster.WriteMultipleRegisters(addressLine, ProductionLineLatinName);
                                addressLine = addressLine + 12;
                            }
                        }
                        //command = new SqlCommand("Update Devices set SendOrderCode=0 where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                        //command.ExecuteNonQuery();
                    }

                    /////ارسال لیست شماره سفارش مقصد
                    command = new SqlCommand("SELECT SendDestinationOrder FROM Devices where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                    var varSendDestinationOrder = command.ExecuteScalar();
                    bool SendDestinationOrder = (bool)varSendDestinationOrder;

                    if (SendDestinationOrder == true || RestartHMI[0] == false)
                    {
                        int addressLine = 399;
                        int addressOrder = 519;
                        command = new SqlCommand("SELECT OrderCode,ProductionLineLatinName FROM vwDeviceOrders where IP!='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);

                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                OrderCodeDestination = (long)reader.GetInt64(reader.GetOrdinal("OrderCode"));
                                float Main_order = OrderCodeDestination;//شماره سفارش از سروربه پنل
                                int[] floatreg = ModbusClient.ConvertFloatToTwoRegisters(Main_order);
                                svimaster.WriteMultipleRegisters(addressOrder, floatreg);
                                addressOrder = addressOrder + 2;

                                string Main_ProductionLineLatinName = (string)reader.GetString(reader.GetOrdinal("ProductionLineLatinName"));//شماره سفارش از سروربه پنل
                                int[] ProductionLineLatinName = ModbusClient.ConvertStringToRegisters(Main_ProductionLineLatinName);
                                svimaster.WriteMultipleRegisters(addressLine, ProductionLineLatinName);
                                addressLine = addressLine + 12;
                            }
                        }
                        command = new SqlCommand("Update Devices set SendDestinationOrder=0 where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                        command.ExecuteNonQuery();
                    }


                    //////نوشتن شماره گاری و وزن آنها رو HMI
                    command = new SqlCommand("SELECT SendGari FROM Devices where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                    var varSendGari = command.ExecuteScalar();
                    bool SendGari = (bool)varSendGari;


                    // if (SendGari == true || RestartHMI[0]==false)

                    if (SendGari == true)
                    {
                        int address = 0;
                        command = new SqlCommand("SELECT ContainerCode,NetWieght FROM Container where IP='" + ListIP.Items[CounterList - 1].ToString() + "' and SendOrderCode=0", connection);

                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                int ContainerCode = (int)reader.GetInt64(reader.GetOrdinal("ContainerCode"));
                                float Main_ContainerCode = ContainerCode;
                                int[] floatreg = ModbusClient.ConvertFloatToTwoRegisters(Main_ContainerCode);
                                svimaster.WriteMultipleRegisters(address, floatreg);


                                float Main_NetWeight = (float)reader.GetFloat(reader.GetOrdinal("NetWeight"));
                                int[] NetWeight = ModbusClient.ConvertFloatToTwoRegisters(Main_NetWeight);
                                svimaster.WriteMultipleRegisters(address + 24, NetWeight);
                                address = address + 1;
                            }
                        }
                        command = new SqlCommand("Update Devices set SendGari=0 where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                        command.ExecuteNonQuery();
                        ///ست کردن بیت ارسال مجدد گاری برای بارگزاری مجدد دیتا از دیتا بیس
                        svimaster.WriteSingleCoil(6, true);

                    }



                    ////ارسال کد کالاهای سفارش مبدا در صورت تغییر در سفارش پنل 

                    bool[] ChangeOrderCodeSource = svimaster.ReadCoils(4, 1);//تغییری در کد سفارش اتفاق افتاده است یا خیر : خیر=0

                    if (ChangeOrderCodeSource[0] == true)
                    {
                        int[] OrderCodeSourceChange = svimaster.ReadHoldingRegisters(0, 2);
                        OrderCodeSource = (long)ModbusClient.ConvertRegistersToFloat(OrderCodeSourceChange);


                        int Address = 199;

                        using (connection)
                        using (command = new SqlCommand("select * from vwOrderParts where OrderCode=" + OrderCodeSource.ToString(), connection))
                        {
                            {
                                for (int j = 199; j <= 251; j = j + 13)
                                {
                                    svimaster.WriteMultipleRegisters(j, ModbusClient.ConvertStringToRegisters("            "));
                                }


                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string PartCodeSource = (string)reader.GetString(reader.GetOrdinal("PartCode"));


                                        string Main_prod_code = PartCodeSource;//کد کالا از سرور به پنل
                                        Main_ProductCode = ModbusClient.ConvertStringToRegisters(Main_prod_code);
                                        svimaster.WriteMultipleRegisters(Address, Main_ProductCode);
                                        Address = Address + 13;
                                    }
                                }
                            }

                            svimaster.WriteSingleCoil(4, false);
                        }
                    }




                    ////ارسال کد کالاهای سفارش مقصد در صورت تغییر در سفارش پنل 

                    bool[] ChangeOrderCodeDestination = svimaster.ReadCoils(6, 1);//تغییری در کد سفارش اتفاق افتاده است یا خیر : خیر=0

                    if (ChangeOrderCodeDestination[0] == true)
                    {
                        int[] OrderCodeDestinationChange = svimaster.ReadHoldingRegisters(0, 2);
                        OrderCodeDestination = (long)ModbusClient.ConvertRegistersToFloat(OrderCodeDestinationChange);


                        int Address = 199;

                        using (connection)
                        using (command = new SqlCommand("select * from vwOrderParts where OrderCode=" + OrderCodeDestination.ToString(), connection))
                        {
                            {
                                for (int j = 199; j <= 251; j = j + 13)
                                {
                                    svimaster.WriteMultipleRegisters(j, ModbusClient.ConvertStringToRegisters("                "));
                                }


                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string PartCodeSource = (string)reader.GetString(reader.GetOrdinal("PartCode"));


                                        string Main_prod_code = PartCodeSource;//کد کالا از سرور به پنل
                                        Main_ProductCode = ModbusClient.ConvertStringToRegisters(Main_prod_code);
                                        svimaster.WriteMultipleRegisters(Address, Main_ProductCode);
                                        Address = Address + 13;
                                    }
                                }
                            }

                            svimaster.WriteSingleCoil(4, false);
                        }
                    }





                    //string Main_prod_code = "uyuy";//کد کالا از سرور به پنل
                    // Main_ProductCode = ModbusClient.ConvertStringToRegisters(Main_prod_code);
                    // svimaster.WriteMultipleRegisters(2, Main_ProductCode);               
                    lblStatus.Text = "Read Device " + ListIP.Items[CounterList - 1].ToString();
                    ///مربوط به الگوریتم خواندن کلیه ای پی ها از لیست
                    if (RestartHMI[0] == false)
                    {
                        svimaster.WriteSingleCoil(5, true);
                    }

                }
                CounterList = CounterList - 1;
                //// برای قسمت تازه روشن دستگاه و یک کردن آن


                connection.Close();
                svimaster.Disconnect();
                int i = DateTime.Compare(DateTime.Now, startTime);
                TimeSpan avgTime = DateTime.Now.Subtract(startTime);
                lblAvarage.Text = (avgTime.TotalSeconds / Crt).ToString();
            }

            connection.Close();
            svimaster.Disconnect();

            }


            catch (Exception ex)
            {
            ListErrors.Items.Add(ListIP.Items[CounterList - 1].ToString() + ex.Message);
            connection.Close();
            lblWrong.Text = (Wrg = Wrg + 1).ToString();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Close();
                connection.Open();
                using (connection)
                using (command = new SqlCommand("select * from Devices", connection))
                {

                    ListIP.Items.Clear();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListIP.Items.Add(reader["IP"].ToString());

                        }
                    }
                }
                CounterList = 0;
            }
            catch
            {
                lblStatus.Text = "Error Read Device From Database";
            }
        }

        private void btnStartAutoRead_Click(object sender, EventArgs e)
        {


            if (btnStartAutoRead.Text == "Start")
            {

                Crt = 0;
                startTime = DateTime.Now;
                btnStartAutoRead.BackColor = Color.Red;
                timerActive = true;
                btnStartAutoRead.Text = "Stop";
                /*                var worker = new BackgroundWorker();
                                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                                worker.RunWorkerAsync();*/

            }
            else
            {
                timerActive = false;
                btnStartAutoRead.BackColor = Color.White;
                btnStartAutoRead.Text = "Start";
                lblStatus.Text = "";

            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

            SqlCommand command = new SqlCommand("select ID from Part where PartCode='" + "35wr66" + "'", connection);

            long ID = (long)command.ExecuteScalar();
        }


    }
}
