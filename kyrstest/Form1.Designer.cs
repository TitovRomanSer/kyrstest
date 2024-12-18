namespace DifferentialEquationSolver
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Определение элементов формы
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Button solveButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox resultBox;
        private System.Windows.Forms.TextBox stepSizeBox;
        private System.Windows.Forms.TextBox endTimeBox;
        private System.Windows.Forms.TextBox initialConditionsBox;
        private System.Windows.Forms.TextBox equationsBox;
        private System.Windows.Forms.Label stepSizeLabel;
        private System.Windows.Forms.Label endTimeLabel;
        private System.Windows.Forms.Label initialConditionsLabel;
        private System.Windows.Forms.Label equationsLabel;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.RadioButton firstOrderRadioButton;
        private System.Windows.Forms.RadioButton secondOrderRadioButton;
        private System.Windows.Forms.Label orderLabel;
        private System.Windows.Forms.Button generateButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.loadButton = new System.Windows.Forms.Button();
            this.solveButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.resultBox = new System.Windows.Forms.TextBox();
            this.stepSizeLabel = new System.Windows.Forms.Label();
            this.stepSizeBox = new System.Windows.Forms.TextBox();
            this.endTimeLabel = new System.Windows.Forms.Label();
            this.endTimeBox = new System.Windows.Forms.TextBox();
            this.initialConditionsLabel = new System.Windows.Forms.Label();
            this.initialConditionsBox = new System.Windows.Forms.TextBox();
            this.equationsLabel = new System.Windows.Forms.Label();
            this.equationsBox = new System.Windows.Forms.TextBox();
            this.printButton = new System.Windows.Forms.Button();
            this.firstOrderRadioButton = new System.Windows.Forms.RadioButton();
            this.secondOrderRadioButton = new System.Windows.Forms.RadioButton();
            this.orderLabel = new System.Windows.Forms.Label();
            this.generateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(276, 496);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(150, 23);
            this.loadButton.TabIndex = 9;
            this.loadButton.Text = "Загрузить из файла";
            this.loadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // solveButton
            // 
            this.solveButton.BackColor = System.Drawing.Color.Lime;
            this.solveButton.Location = new System.Drawing.Point(10, 496);
            this.solveButton.Name = "solveButton";
            this.solveButton.Size = new System.Drawing.Size(100, 23);
            this.solveButton.TabIndex = 0;
            this.solveButton.Text = "Решить!";
            this.solveButton.UseVisualStyleBackColor = false;
            this.solveButton.Click += new System.EventHandler(this.SolveButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(120, 496);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(150, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Сохранить в файл";
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // resultBox
            // 
            this.resultBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.resultBox.Location = new System.Drawing.Point(10, 525);
            this.resultBox.Multiline = true;
            this.resultBox.Name = "resultBox";
            this.resultBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultBox.Size = new System.Drawing.Size(593, 200);
            this.resultBox.TabIndex = 2;
            this.resultBox.TextChanged += new System.EventHandler(this.resultBox_TextChanged);
            // 
            // stepSizeLabel
            // 
            this.stepSizeLabel.Location = new System.Drawing.Point(10, 10);
            this.stepSizeLabel.Name = "stepSizeLabel";
            this.stepSizeLabel.Size = new System.Drawing.Size(80, 23);
            this.stepSizeLabel.TabIndex = 3;
            this.stepSizeLabel.Text = "Шаг:";
            // 
            // stepSizeBox
            // 
            this.stepSizeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.stepSizeBox.Location = new System.Drawing.Point(100, 10);
            this.stepSizeBox.Multiline = true;
            this.stepSizeBox.Name = "stepSizeBox";
            this.stepSizeBox.Size = new System.Drawing.Size(100, 22);
            this.stepSizeBox.TabIndex = 4;
            // 
            // endTimeLabel
            // 
            this.endTimeLabel.Location = new System.Drawing.Point(10, 40);
            this.endTimeLabel.Name = "endTimeLabel";
            this.endTimeLabel.Size = new System.Drawing.Size(133, 23);
            this.endTimeLabel.TabIndex = 5;
            this.endTimeLabel.Text = "Время окончания:";
            // 
            // endTimeBox
            // 
            this.endTimeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.endTimeBox.Location = new System.Drawing.Point(150, 40);
            this.endTimeBox.Multiline = true;
            this.endTimeBox.Name = "endTimeBox";
            this.endTimeBox.Size = new System.Drawing.Size(100, 22);
            this.endTimeBox.TabIndex = 6;
            // 
            // initialConditionsLabel
            // 
            this.initialConditionsLabel.Location = new System.Drawing.Point(10, 70);
            this.initialConditionsLabel.Name = "initialConditionsLabel";
            this.initialConditionsLabel.Size = new System.Drawing.Size(260, 25);
            this.initialConditionsLabel.TabIndex = 7;
            this.initialConditionsLabel.Text = "Начальные условия (через запятую):";
            // 
            // initialConditionsBox
            // 
            this.initialConditionsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.initialConditionsBox.Location = new System.Drawing.Point(10, 98);
            this.initialConditionsBox.Multiline = true;
            this.initialConditionsBox.Name = "initialConditionsBox";
            this.initialConditionsBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.initialConditionsBox.Size = new System.Drawing.Size(593, 113);
            this.initialConditionsBox.TabIndex = 8;
            // 
            // equationsLabel
            // 
            this.equationsLabel.Location = new System.Drawing.Point(7, 225);
            this.equationsLabel.Name = "equationsLabel";
            this.equationsLabel.Size = new System.Drawing.Size(306, 23);
            this.equationsLabel.TabIndex = 11;
            this.equationsLabel.Text = "Система уравнений (через точку с запятой):";
            // 
            // equationsBox
            // 
            this.equationsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.equationsBox.Location = new System.Drawing.Point(10, 251);
            this.equationsBox.Multiline = true;
            this.equationsBox.Name = "equationsBox";
            this.equationsBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.equationsBox.Size = new System.Drawing.Size(593, 171);
            this.equationsBox.TabIndex = 12;
            // 
            // printButton
            // 
            this.printButton.Location = new System.Drawing.Point(432, 496);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(100, 23);
            this.printButton.TabIndex = 16;
            this.printButton.Text = "Печать";
            this.printButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // firstOrderRadioButton
            // 
            this.firstOrderRadioButton.Location = new System.Drawing.Point(150, 461);
            this.firstOrderRadioButton.Name = "firstOrderRadioButton";
            this.firstOrderRadioButton.Size = new System.Drawing.Size(80, 20);
            this.firstOrderRadioButton.TabIndex = 13;
            this.firstOrderRadioButton.TabStop = true;
            this.firstOrderRadioButton.Text = "Первый порядок";
            // 
            // secondOrderRadioButton
            // 
            this.secondOrderRadioButton.Location = new System.Drawing.Point(251, 461);
            this.secondOrderRadioButton.Name = "secondOrderRadioButton";
            this.secondOrderRadioButton.Size = new System.Drawing.Size(78, 20);
            this.secondOrderRadioButton.TabIndex = 14;
            this.secondOrderRadioButton.TabStop = true;
            this.secondOrderRadioButton.Text = "Второй порядок";
            // 
            // orderLabel
            // 
            this.orderLabel.Location = new System.Drawing.Point(7, 461);
            this.orderLabel.Name = "orderLabel";
            this.orderLabel.Size = new System.Drawing.Size(136, 20);
            this.orderLabel.TabIndex = 15;
            this.orderLabel.Text = "Выберите порядок ДУ:";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(382, 456);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(150, 30);
            this.generateButton.TabIndex = 0;
            this.generateButton.Text = "Сгенерировать";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.GenerateRandomEquationsButton_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(626, 737);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.solveButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.resultBox);
            this.Controls.Add(this.stepSizeLabel);
            this.Controls.Add(this.stepSizeBox);
            this.Controls.Add(this.endTimeLabel);
            this.Controls.Add(this.endTimeBox);
            this.Controls.Add(this.initialConditionsLabel);
            this.Controls.Add(this.initialConditionsBox);
            this.Controls.Add(this.equationsLabel);
            this.Controls.Add(this.equationsBox);
            this.Controls.Add(this.firstOrderRadioButton);
            this.Controls.Add(this.secondOrderRadioButton);
            this.Controls.Add(this.orderLabel);
            this.Name = "MainForm";
            this.Text = "Решение систем дифференциальных уравнений методом Рунге-Кутта";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
