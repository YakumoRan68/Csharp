using System;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

/*
    TODO LIST
    붙여넣기로 수식 복사
    자릿수 구분자
*/

namespace _1121 {

    public partial class Form1 : Form {
        class Expression {
            private string[] element;
            private int top;
            public Expression() {
                element = new string[3];
                top = -1;
            }

            public bool isempty() {
                return top == -1;
            }

            public string peek() {
                return element[top];
            }

            public void push(string item) {
                if (top == 2) {
                    MessageBox.Show("Stack Overflow");
                    return;
                } else {
                    element[++top] = item;
                }
            }

            public int count() {
                int cnt = 0;
                foreach (var v in element) {
                    cnt = v == null ? cnt : ++cnt; 
                }
                return cnt;
            }

            public string pop() {
                if (top == -1) {
                    MessageBox.Show("Stack Underflow");
                    return string.Empty;
                } else {
                    string str = element[top];
                    element[top--] = null;
                    return str;
                }
            }

            public void clear() {
                for (int i = 0; i < 3; i++) {
                    element[i] = null; 
                }
                top = -1;
            }

            public string print() {
                string str = string.Empty;
                foreach (var v in element) {
                    str = v == null ? null : v;
                    Debug.WriteLine(v);
                }
                return str;
            }
        }

        class Memory {
            private string term = string.Empty;

            public Memory(string str) {
                term = str; 
            }

            public string Term {
                get {
                    return term;
                }
                set {
                    term = value;
                }
            }

            public override string ToString() {
                return term;
            }
        }

        private Form2 form_2;
        static int size = 256; //field initialler(배열크기 설정하는 부분)은 고정적(static)으로 설정해야 한다.
        List<Memory> TheMemory = new List<Memory>();
        Expression[] Operands = new Expression[size];
        Expression[] Operators = new Expression[size];
        byte TermIndex = 0; //괄호가 열릴때마다 이 수치가 1씩올라간다.

        string ScreenBuffer = string.Empty; //수식을 입력받을 때의 버퍼.
        string LastResult = string.Empty; //수식에 적용될때의 버퍼.
        string LastOperator = string.Empty; //(방금 계산한) 마지막 연산자
        string LastOperand = string.Empty; // (방금 계산한) 마지막 피연산자
        string BufferedMemory = string.Empty; //메모리 스크린에 쓰기전에 쓰이는 버퍼.

        bool IsDot = false; // 실수형인지 정수형인지
        byte screen_max_width = 24; //스크린의 너비
        byte memory_max_width = 36; //메모리 스크린의 너비

        public Form1() {
            InitializeComponent();
            MemoryScreen.Text = string.Empty;
            overflowed.Visible = false;
            KeyPreview = true;
            for (int i = 0; i < size; i++) {
                Operands[i] = new Expression();
                Operators[i] = new Expression();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            //대부분의 경우 키보드에서 키를 누르면 KeyUp, KeyPress, KeyDown 등의 이벤트들이 발생한다.
            //그러나 Control, Alt, ReturnResult 같은 특수한 키들은 상기된 이벤트들을 발생시기 전에 미리 짜여진, 다른 행동을 취한다.
            //ProcessCmdKey는 해당 폼으로 들어오는 키들을 제일 먼저 인식하고 관련된 키에 대한 프로시저들을 실행하는 메소드인데, 이 함수를 오버라이드 한다.
            //Debug.WriteLine(Convert.ToString(keyData));
            if (keyData == Keys.Return) {
                Equal.PerformClick();
                return true; //여기서 return 함으로써 enter키를 누르는 행동이 '버튼을 누르는 등'의 프로시저들 행하지 않고 종료시킨다.
            }
            if (keyData == Keys.Tab) {
                foreach(Control ctr in Controls) {
                    if (ctr.GetType() == typeof(Button)) ctr.TabStop = true;
                }
            }

            if (keyData == (Keys.Shift | Keys.D9)) {
                BracketOpen.PerformClick();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.D0)) {
                BracketClose.PerformClick();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.D5)) {
                Mod.PerformClick();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.D6)) {
                Power.PerformClick();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.D8)) {
                Multiply.PerformClick();
                return true;
            }

            if (keyData == (Keys.OemMinus)) {
                Minus.PerformClick();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.Oemplus)) {
                Plus.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        void CallMethodByName(string name) {
            Type t = GetType(); //현재 인스턴스(_1121.Form1 클래스)에 대한 형식(Type)을 가져옴
            MethodInfo method = t.GetMethod(name, BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Instance);
            //메서드(함수)를 이름으로 검색하되, 지정된 바인드 플래그 형식으로 검색함.
            //바인드 플래그가 없는 버전은 Public 메소드 밖에 검색하지 않음.
            // | 연산자 : 기본적으로 || 와 비슷함. A | B; A의 성질이 있으면서, B의 성질이 있는 플래그들을 포함시킴.
            method.Invoke(this, new object[] { null, EventArgs.Empty });
            //메서드 호출함수. 해당 인스턴스에서 어규먼트(sender, e)를 가진 채로 호출함.
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.D0: case Keys.D1: case Keys.D2: case Keys.D3: case Keys.D4: case Keys.D5: case Keys.D6: case Keys.D7: case Keys.D8: case Keys.D9:
                    CallMethodByName("NUM"+Convert.ToString((int)e.KeyCode-48)+"_Click"); break;
                case Keys.NumPad0: case Keys.NumPad1: case Keys.NumPad2: case Keys.NumPad3: case Keys.NumPad4: case Keys.NumPad5: case Keys.NumPad6: case Keys.NumPad7: case Keys.NumPad8: case Keys.NumPad9:
                    CallMethodByName("NUM"+Convert.ToString((int)e.KeyCode-96)+"_Click"); break;
                case Keys.Decimal:
                    Dot_Click(null, EventArgs.Empty); break;
                case Keys.Add:
                    Plus_Click(null, EventArgs.Empty); break;
                case Keys.Subtract:
                    Minus_Click(null, EventArgs.Empty); break;
                case Keys.Multiply:
                    Multiply_Click(null, EventArgs.Empty); break;
                case Keys.OemQuestion:
                case Keys.Divide:
                    Divide_Click(null, EventArgs.Empty); break;
                case Keys.Back:
                    Backspace_Click(null, EventArgs.Empty); break;
                case Keys.Oemplus:
                    Equal_Click(null, EventArgs.Empty); break;
            }
        }
        private void SetIsDot(string x) {
            // IsDot = Convert.ToDouble(x) != Convert.ToInt32(x); 
            // 왜인진 모르겠지만... "3.333" 등으로 적힌 문자열을 바로 ToInt32 하면 오류가 났음.
            if (x.Equals("NaN")) {
                IsDot = false;
                return;
            }
            double temp = Convert.ToDouble(x);
            IsDot = temp != Convert.ToInt32(temp);
        }
        private bool IsValidModOp() {
            try {
                Convert.ToInt64(Screen.Text);
            } catch {
                MessageBox.Show("나머지 연산은 정수끼리만 가능합니다.");
                return true;
            }
            return false;
        }
        private void ChangeOperator(string op) {
            if (TheMemory.Count == 0) { //결과 값이 나온 상황에서 연산자를 누른 경우(1 + 2 = -) RR case 4
                if (LastResult == string.Empty) { // 처음 - 를 입력한 경우
                    ScreenBuffer = negate();
                    ShowScreenBuffer();
                    return;
                }
                TheMemory.Add(new Memory(LastResult));
                TheMemory.Add(new Memory(op));
                LastOperand = LastResult;
            } else {
                if (Operators[TermIndex].count() == 0) {
                    ScreenBuffer = negate();
                    ShowScreenBuffer();
                    return;
                } else {
                    Operators[TermIndex].pop(); //마지막에 쓴 연산자를 바꿈
                    Operators[TermIndex].push(op);
                }
            }
            IsDot = false;
            TheMemory[TheMemory.Count - 1].Term = op; //메모리에서도 바꿈
            LastOperator = op;
            WriteFormula();
            MemoryScreenUpdate();
        }
        private int ValidateBracket() {
            int NumOpenBracket = 0;
            //foreach (char c in TheFormula) if (c == '(') NumOpenBracket++;

            int NumCloseBracket = 0;
            //foreach (char c in TheFormula) if (c == ')') NumCloseBracket++;

            return NumOpenBracket - NumCloseBracket;
        }
        private void AddOperandToScreen(char operand) {
            ScreenBuffer = ScreenBuffer + operand;
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
        }
        private void AddBracketToScreen(char bracket) {
            ScreenBuffer = string.Empty;
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
        }
        private string GetOperand() {
            // 1. 입력한 숫자가 있으면 입력한 숫자를 대상으로 한다.
            if (ScreenBuffer != string.Empty) return ScreenBuffer;
            // 2. 마지막 연산결과를 대상으로 한다. (1 + 1 = U)
            else if (Operands[TermIndex].count() == 0) {
                return LastResult;
            }
            // 3. 이전에 입력한 피연산자를 대상으로 한다.
            else if (LastOperand != string.Empty) {
                return LastOperand;
            }
            else return "0";
        }
        private string GetFormed() {
            if (TheMemory.Count == 0) return GetOperand();
            if (IsLastTerm()) {
                return TheMemory[TheMemory.Count - 1].Term;
            } else return TheMemory[TheMemory.Count - 2].Term;
        }
        private bool IsLastTerm() { // 수식에 마지막으로 적힌게 숫자인지 아닌지.
            if (TheMemory.Count == 0) return true;
            string[] operators = { "+", "-", "*", "/", "%", "^" };
            bool result = false;
            foreach (var op in operators) {
                result = TheMemory[TheMemory.Count - 1].Term == op;
                if (result) return true;
            }
            Debug.WriteLine(result);
            return false;
        }
        private void UnaryOperater(string function) {
            if (ScreenBuffer != string.Empty) {
                PushOperand(ScreenBuffer);
            } 
            for (int i = Operators[TermIndex].count(); i != 0; i--) {
                BinaryOperate();
            }
            string Operand = GetOperand();
            // string Preformed = GetFormed();
            double calculated = 0;
            switch(function) {
                case "negate":
                    calculated = Convert.ToDouble(negate()); break;
                case "sqrt":
                    calculated = Math.Sqrt(Convert.ToDouble(Operand)); break;
                case "reciproc":
                    calculated = 1 / Convert.ToDouble(Operand); break;
            }
            string result = Convert.ToString(calculated);
            LastResult = result;
            SetIsDot(result);
            ScreenBuffer = result;
            TheMemory.Clear();
            BufferedMemory = string.Empty;
            MemoryScreenUpdate();
            for (int i = 0; i < size; i++) {
                Operands[i].clear();
                Operators[i].clear();
            }
            ScreenUpdate();
            //WriteFormula(Preformed, function);
        }
        private string negate() {
            return ScreenBuffer.Contains("-") ? ScreenBuffer.Remove(0, 1) : ScreenBuffer.Insert(0, "-");
        }
        private void DeleteBuffer() {
            ScreenBuffer = string.Empty;
        }
        private void WriteFormula() { // TheMemory에 적힌 수식을 메모리 버퍼에 포맷해서 적음
            string Formula = string.Empty;
            foreach (var list in TheMemory) {
                //괄호가 아니면
                Formula = string.Format("{0} {1}", Formula, list.ToString());
            }
            BufferedMemory = Formula;
        }
        private void WriteFormula(string PREFORMED, string function) { // 해당 단항연산을 적용한 수식을 메모리 버퍼에 포맷해서 적음
            string _FORMED = function + "(" + PREFORMED + ")";
            int index = IsLastTerm() ? TheMemory.Count - 1 : TheMemory.Count - 2;
            //BufferedMemory = ;
        }
        private void MemoryScreenUpdate() {
            MemoryScreen.Text = string.Format("{0," + Convert.ToString(memory_max_width) + "}", BufferedMemory);
        }
        private void ScreenUpdate() {
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", LastResult);
        }
        private void ShowScreenBuffer() {
            Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
        }
        private void PushOperand(string op) {
            Operands[TermIndex].push(op);
        }
        private void PushOperator(string op) {
            Operators[TermIndex].push(op);
        }
        private string Compute(string Operand1, string Operand2, string Operator) {
            double result = 0;
            double Op1 = Convert.ToDouble(Operand1);
            double Op2 = Convert.ToDouble(Operand2);
            switch (Operator) {
                case "+": result = Op1 + Op2; break;
                case "-": result = Op1 - Op2; break;
                case "*": result = Op1 * Op2; break;
                case "/": result = Op1 / Op2; break;
                case "%": result = Op1 % Op2; break;
                case "^": result = Math.Pow(Op1, Op2); break;
            }
            return Convert.ToString(result);
        }
        private string BinaryOperate() {
            string Op2 = Operands[TermIndex].pop();
            string Op1 = Operands[TermIndex].pop();
            string Operator = Operators[TermIndex].pop();
            string result = Compute(Op1, Op2, Operator);
            LastResult = result;
            LastOperand = Op2;
            LastOperator = Operator;
            return LastResult;
        }
        private void TryCalculate(string op) {
            int opcnt = Operands[TermIndex].count();
            if (opcnt < 2) {
                if (ScreenBuffer != string.Empty) {                      //(TC case 1)
                    LastOperand = ScreenBuffer;
                    LastOperator = op;
                }
            } else {
                bool IsOpPlusMinus = op.Equals("+") || op.Equals("-");
                switch (Operators[TermIndex].count()) {
                    case 1:
                        if (!(Operators[TermIndex].peek().Equals("+") || Operators[TermIndex].peek().Equals("-"))) {
                            PushOperand(BinaryOperate());                //(TC case 2)
                            ScreenUpdate();
                        } else {
                            if (IsOpPlusMinus) {                         //(TC case 3)
                                PushOperand(BinaryOperate());
                                ScreenUpdate();
                            } else {                                     //(TC case 4)
                                //할 것 없음
                            }
                        }
                        break;
                    case 2:
                        if (!IsOpPlusMinus) {                            //(TC case 5)
                            PushOperand(BinaryOperate());
                            ScreenUpdate();
                        } else {                                         //(TC case 6)
                            PushOperand(BinaryOperate());
                            PushOperand(BinaryOperate());
                            ScreenUpdate();
                        }
                        break;
                }
            }
            IsDot = false;
        }
        private void BinaryOperater(string op) {
            //if (IsValidModOp()) return;
            if (ScreenBuffer == string.Empty && LastOperand == string.Empty && op != "-") return; //아무것도 입력 안했는데 눌렀을경우
            if (ScreenBuffer == string.Empty || ScreenBuffer == "-") { // 1 + 2 - +, 1 + 2 = -
                ChangeOperator(op);
                return;
            }
            string operand = ScreenBuffer;
            TheMemory.Add(new Memory(operand));
            TheMemory.Add(new Memory(op));
            WriteFormula();
            PushOperand(operand);
            TryCalculate(op);
            PushOperator(op);
            DeleteBuffer();
            MemoryScreenUpdate();
        }
        private void ReturnResult() {
            if (Operands[TermIndex].count() == 0 && LastOperator == string.Empty) { Debug.WriteLine("ReturnResult Invalid Formula"); return; } //연산자나 피연산자가 입력되지 않았을경우

            string result = string.Empty;

            if (Operands[TermIndex].count() > 0 && ScreenBuffer != string.Empty) { //RR case 1 (1 + 2 = )  
                PushOperand(ScreenBuffer);
                if (Operators[TermIndex].count() == 2 && !(Operators[TermIndex].peek().Equals("+") || Operators[TermIndex].peek().Equals("-"))) PushOperand(BinaryOperate());
                result = BinaryOperate();
            } else if (Operands[TermIndex].count() > 0 && ScreenBuffer == string.Empty) { //RR case 2 (1 + =), RR case 3
                string ToCalc = Operands[TermIndex].pop();
                LastOperand = LastOperand != string.Empty ? LastOperand : ToCalc;
                result = Compute(ToCalc, LastOperand, LastOperator);
                LastOperand = ToCalc;
                LastResult = result;
            } else if (Operands[TermIndex].count() == 0 && ScreenBuffer == string.Empty) { //RR case 2-1 (1 + = =)
                result = Compute(LastResult, LastOperand, LastOperator);
                LastResult = result;
            } else if (Operands[TermIndex].count() == 0 && ScreenBuffer != string.Empty) { //RR case 5 (1 + 2 = - 2 = )
                LastOperand = ScreenBuffer;
                result = Compute(LastResult, LastOperand, LastOperator);
                LastResult = result;
            }
            
            TheMemory.Clear();
            BufferedMemory = string.Empty;
            ScreenBuffer = string.Empty;
            MemoryScreenUpdate();
            for (int i = 0; i < size; i++) {
                Operands[i].clear();
                Operators[i].clear();
            }
            SetIsDot(result);
            ScreenUpdate();
        }
        private void NUM0_Click(object sender, EventArgs e) {
            //숫자 0은 1의 자릿수에서 2개이상 쓰이지 않음.
            //소수점 아래에서 위의 조건은 무시됨.
            if (ScreenBuffer.Length != 0) AddOperandToScreen('0');
        }
        private void NUM00_Click(object sender, EventArgs e) {
            if (ScreenBuffer.Length != 0) {
                AddOperandToScreen('0');
                AddOperandToScreen('0');
            }
        }
        private void NUM1_Click(object sender, EventArgs e) => AddOperandToScreen('1');
        private void NUM2_Click(object sender, EventArgs e) => AddOperandToScreen('2');
        private void NUM3_Click(object sender, EventArgs e) => AddOperandToScreen('3');
        private void NUM4_Click(object sender, EventArgs e) => AddOperandToScreen('4');
        private void NUM5_Click(object sender, EventArgs e) => AddOperandToScreen('5');
        private void NUM6_Click(object sender, EventArgs e) => AddOperandToScreen('6');
        private void NUM7_Click(object sender, EventArgs e) => AddOperandToScreen('7');
        private void NUM8_Click(object sender, EventArgs e) => AddOperandToScreen('8');
        private void NUM9_Click(object sender, EventArgs e) => AddOperandToScreen('9');
        private void BracketOpen_Click(object sender, EventArgs e) => AddBracketToScreen('(');
        private void BracketClose_Click(object sender, EventArgs e) => AddBracketToScreen(')');
        private void Dot_Click(object sender, EventArgs e) {
            if (!IsDot) {
                if (ScreenBuffer.Length == 0) AddOperandToScreen('0');
                AddOperandToScreen('.');
                IsDot = true;
            }
        }
        private void Negate_Click(object sender, EventArgs e) => UnaryOperater("negate");
        private void Root_Click(object sender, EventArgs e) => UnaryOperater("sqrt");
        private void Reciprocal_Click(object sender, EventArgs e) => UnaryOperater("reciproc");
        private void Percent_Click(object sender, EventArgs e) => UnaryOperater("percent");
        private void Plus_Click(object sender, EventArgs e) => BinaryOperater("+");
        private void Minus_Click(object sender, EventArgs e) => BinaryOperater("-");
        private void Multiply_Click(object sender, EventArgs e) => BinaryOperater("*");
        private void Divide_Click(object sender, EventArgs e) => BinaryOperater("/");
        private void Mod_Click(object sender, EventArgs e) => BinaryOperater("%");
        private void Power_Click(object sender, EventArgs e) => BinaryOperater("^");
        private void Equal_Click(object sender, EventArgs e) => ReturnResult();
        private void AC_Click(object sender, EventArgs e) { // 메모리 + 스크린 모두 초기화 
            TheMemory.Clear();
            for (int i = 0; i < size; i++) {
                Operands[i].clear();
                Operators[i].clear();
            }
            TermIndex = 0;

            ScreenBuffer = string.Empty;
            LastResult = string.Empty;
            LastOperator = string.Empty;
            LastOperand = string.Empty;
            BufferedMemory = string.Empty;

            Screen.Text = string.Empty;
            MemoryScreen.Text = string.Empty;

            overflowed.Visible = false;

            IsDot = false;
        }
        private void CE_Click(object sender, EventArgs e) { // 스크린버퍼만 초기화
            IsDot = false;
            ScreenBuffer = string.Empty;
            Screen.Text = string.Empty;
        }
        private void Backspace_Click(object sender, EventArgs e) {
            //기본적으로 c#에는 안정성문제로 포인터 연산자 사용을 막아두었다. (Unsafe 모드로 디버그 해야 한다고함)
            //따라서 문자열을 관리하기 위해서는 주워진 메소드를 사용해야한다.
            //스트링이름.Remove(문자열인덱스, 지울 갯수)
            if (ScreenBuffer.Length == 0) return;
            if (ScreenBuffer[ScreenBuffer.Length - 1] == '.') IsDot = false;
            ScreenBuffer = ScreenBuffer.Remove(ScreenBuffer.Length - 1, 1);
            Screen.Text = Screen.Text = string.Format("{0," + Convert.ToString(screen_max_width) + "}", ScreenBuffer);
        }
        private void 공학용계산기ToolStripMenuItem_Click(object sender, EventArgs e) {
            form_2 = new Form2();
            form_2.Owner = this;
            form_2.Show();
            form_2.Location = Location;
            
            Hide();
        }
        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e) => Dispose(); // 이걸 왜 안알려 주셨을까.
        private void 개발자소개ToolStripMenuItem_Click(object sender, EventArgs e) => new Form3().ShowDialog();
        private void 사용정보ToolStripMenuItem_Click(object sender, EventArgs e) => new Form4().ShowDialog();
        private void 교수님ToolStripMenuItem_Click(object sender, EventArgs e) => new Form5().ShowDialog();
        private void 개발환경ToolStripMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("운영 체제 : Windows 10\n개발 도구 : Microsoft Visual Studio 2017\n\t", "< 개발 환경 >");
        private void 버전정보ToolStripMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("C#으로 내가 만든 계산기 1.0", "< 버전 정보 >");
        private void Form1_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

        private void 일반용계산기ToolStripMenuItem_Click(object sender, EventArgs e) {

        }
    }
}