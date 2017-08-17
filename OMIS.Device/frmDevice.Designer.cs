namespace OMIS.Device
{
    partial class frmDevice
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtDataQN = new System.Windows.Forms.TextBox();
            this.chbPack = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbLoopTimes = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtDataCon = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnSendData = new System.Windows.Forms.Button();
            this.txtDataCRC = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDataLen = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtHeartbeatQN = new System.Windows.Forms.TextBox();
            this.btnHeartbeat = new System.Windows.Forms.Button();
            this.txtHeartbeatCRC = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtHeartbeatLen = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtHeartbeat = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbHeartbeat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDevCode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHeartbeatCon = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDisconncet = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbServerPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbServerIp = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.timerHeartbeat = new System.Windows.Forms.Timer(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(501, 562);
            this.panel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.txtDataQN);
            this.groupBox3.Controls.Add(this.chbPack);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.cmbLoopTimes);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.txtDataCon);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.btnSendData);
            this.groupBox3.Controls.Add(this.txtDataCRC);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtDataLen);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.txtData);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(8, 204);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(487, 306);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "数据报文";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(8, 50);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(23, 12);
            this.label18.TabIndex = 35;
            this.label18.Text = "QN:";
            // 
            // txtDataQN
            // 
            this.txtDataQN.Location = new System.Drawing.Point(42, 47);
            this.txtDataQN.Name = "txtDataQN";
            this.txtDataQN.ReadOnly = true;
            this.txtDataQN.Size = new System.Drawing.Size(146, 21);
            this.txtDataQN.TabIndex = 34;
            // 
            // chbPack
            // 
            this.chbPack.AutoSize = true;
            this.chbPack.Location = new System.Drawing.Point(318, 278);
            this.chbPack.Name = "chbPack";
            this.chbPack.Size = new System.Drawing.Size(72, 16);
            this.chbPack.TabIndex = 33;
            this.chbPack.Text = "拼包发送";
            this.chbPack.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(291, 279);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 32;
            this.label15.Text = "次";
            // 
            // cmbLoopTimes
            // 
            this.cmbLoopTimes.FormattingEnabled = true;
            this.cmbLoopTimes.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "5",
            "8",
            "13",
            "21",
            "34",
            "55",
            "89"});
            this.cmbLoopTimes.Location = new System.Drawing.Point(235, 276);
            this.cmbLoopTimes.Name = "cmbLoopTimes";
            this.cmbLoopTimes.Size = new System.Drawing.Size(50, 20);
            this.cmbLoopTimes.TabIndex = 31;
            this.cmbLoopTimes.Text = "1";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(198, 279);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(35, 12);
            this.label14.TabIndex = 30;
            this.label14.Text = "循环:";
            // 
            // txtDataCon
            // 
            this.txtDataCon.Location = new System.Drawing.Point(42, 74);
            this.txtDataCon.Multiline = true;
            this.txtDataCon.Name = "txtDataCon";
            this.txtDataCon.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDataCon.Size = new System.Drawing.Size(436, 185);
            this.txtDataCon.TabIndex = 29;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 12);
            this.label13.TabIndex = 28;
            this.label13.Text = "内容:";
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(403, 274);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(75, 23);
            this.btnSendData.TabIndex = 27;
            this.btnSendData.Text = "发送数据";
            this.btnSendData.UseVisualStyleBackColor = true;
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // txtDataCRC
            // 
            this.txtDataCRC.Location = new System.Drawing.Point(148, 276);
            this.txtDataCRC.Name = "txtDataCRC";
            this.txtDataCRC.ReadOnly = true;
            this.txtDataCRC.Size = new System.Drawing.Size(40, 21);
            this.txtDataCRC.TabIndex = 26;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(116, 279);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 25;
            this.label11.Text = "CRC:";
            // 
            // txtDataLen
            // 
            this.txtDataLen.Location = new System.Drawing.Point(42, 276);
            this.txtDataLen.Name = "txtDataLen";
            this.txtDataLen.ReadOnly = true;
            this.txtDataLen.Size = new System.Drawing.Size(60, 21);
            this.txtDataLen.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 279);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 12);
            this.label12.TabIndex = 23;
            this.label12.Text = "长度:";
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(42, 20);
            this.txtData.Name = "txtData";
            this.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtData.Size = new System.Drawing.Size(436, 21);
            this.txtData.TabIndex = 20;
            this.txtData.Text = "QN={0};ST=91;PW=123456;MN={1};CN=2011;CmdFlag=1;CP=&&{2}&&";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "格式:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.txtHeartbeatQN);
            this.groupBox2.Controls.Add(this.btnHeartbeat);
            this.groupBox2.Controls.Add(this.txtHeartbeatCRC);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtHeartbeatLen);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtHeartbeat);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbHeartbeat);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmbDevCode);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtHeartbeatCon);
            this.groupBox2.Location = new System.Drawing.Point(8, 62);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(487, 136);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设备心跳";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(201, 52);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(23, 12);
            this.label16.TabIndex = 25;
            this.label16.Text = "QN:";
            // 
            // txtHeartbeatQN
            // 
            this.txtHeartbeatQN.Location = new System.Drawing.Point(235, 49);
            this.txtHeartbeatQN.Name = "txtHeartbeatQN";
            this.txtHeartbeatQN.Size = new System.Drawing.Size(119, 21);
            this.txtHeartbeatQN.TabIndex = 24;
            this.txtHeartbeatQN.Text = "1234567890";
            // 
            // btnHeartbeat
            // 
            this.btnHeartbeat.Location = new System.Drawing.Point(403, 101);
            this.btnHeartbeat.Name = "btnHeartbeat";
            this.btnHeartbeat.Size = new System.Drawing.Size(75, 23);
            this.btnHeartbeat.TabIndex = 23;
            this.btnHeartbeat.Text = "发送心跳";
            this.btnHeartbeat.UseVisualStyleBackColor = true;
            this.btnHeartbeat.Click += new System.EventHandler(this.btnHeartbeat_Click);
            // 
            // txtHeartbeatCRC
            // 
            this.txtHeartbeatCRC.Location = new System.Drawing.Point(148, 103);
            this.txtHeartbeatCRC.Name = "txtHeartbeatCRC";
            this.txtHeartbeatCRC.ReadOnly = true;
            this.txtHeartbeatCRC.Size = new System.Drawing.Size(40, 21);
            this.txtHeartbeatCRC.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(116, 106);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 21;
            this.label9.Text = "CRC:";
            // 
            // txtHeartbeatLen
            // 
            this.txtHeartbeatLen.Location = new System.Drawing.Point(42, 103);
            this.txtHeartbeatLen.Name = "txtHeartbeatLen";
            this.txtHeartbeatLen.ReadOnly = true;
            this.txtHeartbeatLen.Size = new System.Drawing.Size(60, 21);
            this.txtHeartbeatLen.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 106);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 19;
            this.label8.Text = "长度:";
            // 
            // txtHeartbeat
            // 
            this.txtHeartbeat.Location = new System.Drawing.Point(42, 20);
            this.txtHeartbeat.Name = "txtHeartbeat";
            this.txtHeartbeat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHeartbeat.Size = new System.Drawing.Size(436, 21);
            this.txtHeartbeat.TabIndex = 18;
            this.txtHeartbeat.Text = "QN={0};ST=91;PW=123456;MN={1};CN=9021;CmdFlag=1;CP=&&{2}&&";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "格式:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(289, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "秒";
            // 
            // cmbHeartbeat
            // 
            this.cmbHeartbeat.FormattingEnabled = true;
            this.cmbHeartbeat.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "30",
            "40",
            "45",
            "60"});
            this.cmbHeartbeat.Location = new System.Drawing.Point(235, 103);
            this.cmbHeartbeat.Name = "cmbHeartbeat";
            this.cmbHeartbeat.Size = new System.Drawing.Size(50, 20);
            this.cmbHeartbeat.TabIndex = 15;
            this.cmbHeartbeat.Text = "40";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(198, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "间隔:";
            // 
            // cmbDevCode
            // 
            this.cmbDevCode.FormattingEnabled = true;
            this.cmbDevCode.Items.AddRange(new object[] {
            "72000072"});
            this.cmbDevCode.Location = new System.Drawing.Point(42, 49);
            this.cmbDevCode.Name = "cmbDevCode";
            this.cmbDevCode.Size = new System.Drawing.Size(146, 20);
            this.cmbDevCode.TabIndex = 13;
            this.cmbDevCode.Text = "72000072";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "设备:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "内容:";
            // 
            // txtHeartbeatCon
            // 
            this.txtHeartbeatCon.Location = new System.Drawing.Point(42, 75);
            this.txtHeartbeatCon.Name = "txtHeartbeatCon";
            this.txtHeartbeatCon.Size = new System.Drawing.Size(436, 21);
            this.txtHeartbeatCon.TabIndex = 2;
            this.txtHeartbeatCon.Text = "VER=720:VER1.1 2016-06-11 01:12:59;Mode=1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDisconncet);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbServerPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbServerIp);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 48);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接服务器";
            // 
            // btnDisconncet
            // 
            this.btnDisconncet.Location = new System.Drawing.Point(423, 17);
            this.btnDisconncet.Name = "btnDisconncet";
            this.btnDisconncet.Size = new System.Drawing.Size(55, 23);
            this.btnDisconncet.TabIndex = 5;
            this.btnDisconncet.Text = "断开";
            this.btnDisconncet.UseVisualStyleBackColor = true;
            this.btnDisconncet.Click += new System.EventHandler(this.btnDisconncet_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(363, 17);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(55, 23);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "服务器端口:";
            // 
            // cmbServerPort
            // 
            this.cmbServerPort.FormattingEnabled = true;
            this.cmbServerPort.Items.AddRange(new object[] {
            "23473"});
            this.cmbServerPort.Location = new System.Drawing.Point(295, 19);
            this.cmbServerPort.Name = "cmbServerPort";
            this.cmbServerPort.Size = new System.Drawing.Size(59, 20);
            this.cmbServerPort.TabIndex = 2;
            this.cmbServerPort.Text = "23473";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "服务器IP:";
            // 
            // cmbServerIp
            // 
            this.cmbServerIp.FormattingEnabled = true;
            this.cmbServerIp.Items.AddRange(new object[] {
            "127.0.0.1",
            "122.227.179.90"});
            this.cmbServerIp.Location = new System.Drawing.Point(71, 19);
            this.cmbServerIp.Name = "cmbServerIp";
            this.cmbServerIp.Size = new System.Drawing.Size(141, 20);
            this.cmbServerIp.TabIndex = 0;
            this.cmbServerIp.Text = "127.0.0.1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(501, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(483, 562);
            this.panel2.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtLog);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(483, 534);
            this.panel5.TabIndex = 2;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.ForeColor = System.Drawing.Color.White;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(483, 534);
            this.txtLog.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClear);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 534);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(483, 28);
            this.panel4.TabIndex = 1;
            // 
            // timerHeartbeat
            // 
            this.timerHeartbeat.Tick += new System.EventHandler(this.timerHeartbeat_Tick);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(391, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "清除内容";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmDevice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "模拟设备";
            this.Load += new System.EventHandler(this.frmDevice_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbServerIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbServerPort;
        private System.Windows.Forms.Button btnDisconncet;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnHeartbeat;
        private System.Windows.Forms.TextBox txtHeartbeatCRC;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtHeartbeatLen;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtHeartbeat;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbHeartbeat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDevCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtHeartbeatCon;
        private System.Windows.Forms.Timer timerHeartbeat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.TextBox txtDataCRC;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDataLen;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtDataCon;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbLoopTimes;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox chbPack;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtHeartbeatQN;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtDataQN;
        private System.Windows.Forms.Button btnClear;
    }
}

