using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

using CalculatorApp.Common;

namespace CalculatorApp.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Fields

        private string _entry;
        private string _expression;

        // counter to keep track of the Nth term in the expression.
        private int _n;
        private bool _resetEntry;   // flag to reset the entry display.
        private bool _newEntry;     // checks if a number was entered.
        
        private double _operand1;
        private double _operand2;
        private string _operator;




        #endregion

        #region Properties

        public string Entry
        {
            get
            {
                return _entry;
            }

            set
            {
                SetProperty(ref _entry, value);
            }
        }

        public string Expression
        {
            get
            {
                return _expression;
            }

            set
            {
                SetProperty(ref _expression, value);
            }
        }

        #endregion

        #region Commands

        public ICommand SymbolInputCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand SubtractCommand { get; set; }
        public ICommand MultiplyCommand { get; set; }
        public ICommand DivideCommand { get; set; }
        public ICommand PercentageCommand { get; set; }
        public ICommand BackspaceCommand { get; set; }
        public ICommand ClearEntryCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand EqualCommand { get; set; }
        
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            // register functions to handle the commands.
            SymbolInputCommand = new RelayCommand(SymbolInput);
            AddCommand = new RelayCommand(Add);
            SubtractCommand = new RelayCommand(Subtract);
            MultiplyCommand = new RelayCommand(Multiply);
            DivideCommand = new RelayCommand(Divide);
            // PercentageCommand
            // BackspaceCommand
            ClearEntryCommand = new RelayCommand(ClearEntry);
            ClearCommand = new RelayCommand(Clear);
            EqualCommand = new RelayCommand(Equal);

            // set default values.
            Entry = "0";
            Expression = null;
        }

        #endregion

        #region Methods

        // Triggered when any symbol button is pressed (0-9, .)
        private void SymbolInput(object parameter)
        {
            // reset the Entry display when an operator has been pressed before.
            if (_resetEntry)
            {
                Entry = "";
                
                _resetEntry = false;
            }

            string input = ((Button)parameter).Content.ToString();

            // The ? : syntax is called ternary operator, its a short hand way of writing an if-else statement
            Entry = (Entry == "0") ? input : Entry + input;

            _newEntry = true;
        }

        // Triggered when the + button is pressed.
        private void Add(object parameter)
        {
            Evaluate(Double.Parse(Entry), "+");
        }

        // Triggered when the - button is pressed.
        private void Subtract(object parameter)
        {
            Evaluate(Double.Parse(Entry), "-");
        }

        // Triggered when the X button is pressed.
        private void Multiply(object parameter)
        {
            Evaluate(Double.Parse(Entry), "*");
        }

        // Triggered when the / button is pressed.
        private void Divide(object parameter)
        {
            Evaluate(Double.Parse(Entry), "/");
        }

        // Triggered when the CE button is pressed, clears the entry only.
        private void ClearEntry(object parameter)
        {
            Entry = "0";
        }

        // Triggered when the C button is pressed. Clears the entry and expression.
        private void Clear(object parameter)
        {
            _operand1 = 0;
            _n = 0;

            Expression = null;
            Entry = "0";
        }

        private void Equal(object parameter)
        {
            Evaluate(Double.Parse(Entry), "=");
        }

        // evaluate the expression
        private void Evaluate(double input, string op)
        {
            // 1st number entered.
            if (_n < 1)
            {
                if (_newEntry)
                {
                    _operand1 = input;
                    _operator = op;
                    _n = 1;

                    Expression = _operand1 + " " + _operator + " ";
                }
                else
                {
                    // =
                    Expression = _operand1 + " " + _operator + " " + _operand2;


                    // evaluate expression.
                    switch (_operator)
                    {
                        case "+":
                            _operand1 += _operand2;
                            break;
                        case "-":
                            _operand1 -= _operand2;
                            break;
                        case "*":
                            _operand1 *= _operand2;
                            break;
                        case "/":
                            _operand1 /= _operand2;
                            break;
                        default:
                            break;
                    }
                    Entry = (_operand1).ToString();

                    _n = 0;

                }
                
            }
            // 2nd-Nth numbers entered.
            else
            {
                if (_newEntry)
                {
                    // evaluate
                    if (op == "=")
                    {
                        _operand2 = input;

                        // evaluate expression.
                        switch (_operator)
                        {
                            case "+":
                                _operand1 += _operand2;
                                break;
                            case "-":
                                _operand1 -= _operand2;
                                break;
                            case "*":
                                _operand1 *= _operand2;
                                break;
                            case "/":
                                _operand1 /= _operand2;
                                break;
                            default:
                                break;
                        }


                        Expression += _operand2 + " " + op + " ";
                        Entry = (_operand1).ToString();
                        _n = 0;
                    }
                    else
                    {
                        _operand2 = input;
                        
                        // evaluate expression.
                        switch (_operator)
                        {
                            case "+":
                                _operand1 += _operand2;
                                break;
                            case "-":
                                _operand1 -= _operand2;
                                break;
                            case "*":
                                _operand1 *= _operand2;
                                break;
                            case "/":
                                _operand1 /= _operand2;
                                break;
                            default:
                                break;
                        }

                        _operator = op;

                        Expression += _operand2 + " " + _operator + " ";
                        Entry = (_operand1).ToString();
                        _n++;
                    }

                    
                }
                // swap the operators around
                else
                {
                    // different behaviour "="
                    if (op == "=")
                    {
                        // re-do last operation.
                        Expression = _operand1 + " " + _operator + " " + _operand2;
                    }
                    // others. just swap the operator.
                    else
                    {
                        _operator = op;

                        Expression = _operand1 + " " + _operator + " ";
                    }
                    

                    
                    
                }
            }

            _newEntry = false;
            _resetEntry = true;

            //// 1st number entered
            //if (_n < 1)
            //{
            //    if (_newEntry)
            //    {
            //        _operand1 = input;
            //        _operator = op;
            //        _n = 1;

            //        // update the expression display.
            //        Expression = _operand1 + " " + op + " ";
            //    }
            //    // no new number was entered, swap operators
            //    else
            //    {
            //        _operator = op;

            //        // update the expression display.
            //        Expression = _operand1 + " " + op + " ";
            //    }
            //}
            //// 2nd - Nth numbers entered
            //else
            //{
            //    // store most recent entry as operand 2 and update the expression display

                

            //    _operand2 = input;
            //    Expression += _operand2 + " " + op + " ";

            //    // evaluate the expression
            //    switch (_operator)
            //    {
            //        case "+":
            //            _operand1 += _operand2;
            //            break;
            //        case "-":
            //            _operand1 -= _operand2;
            //            break;
            //        case "*":
            //            _operand1 *= _operand2;
            //            break;
            //        case "/":
            //            _operand1 /= _operand2;
            //            break;
            //        default:
            //            break;
            //    }

            //    // update the entry display to show the current answer
            //    Entry = (_operand1).ToString();

            //    // update flags.
            //    _operator = op;
            //    _n = (op == "=") ? 0 : _n + 1;
                
            //}

            //_resetEntry = true;
            //_newEntry = false;
        }

        #endregion
    }
}
