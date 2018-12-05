 2 * 3 - 1 + 1 =

[2 *]3 - 1 + 1 =

2 (*){2} > (PushOperand, TryCalc, push, deletebuffer) - PTPD

 2 *[3 -]1 + 1 =

23(*){3} > PushOperand, TryCalc(case 2)
  (-){3} > Calculate, PushOperator
6 (-){6} > PushOperand(연산결과), deletebuffer

 2 * 3 -[1 +]1 =
    ┌+
61(-){6} > PushOperand, TryCalc(case 3)
  (+){6} > Calculate, PushOperator
5 (+){5} > PushOperand(연산결과), deletebuffer

 2 * 3 - 1 +[1 =]

51(+){5} > 피연산자 PushOperand
  ( ){6} > ReturnResult(Calculate), deletebuffer