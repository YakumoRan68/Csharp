 2 * 3 - 1 + 1 =

[2 *]3 - 1 + 1 =

2 (*){2} > (enqueue, TryCalc, push, deletebuffer) - EPTD

 2 *[3 -]1 + 1 =

23(*){3} > enqueue, TryCalc(case 2)
  (-){3} > InitStruct, push
6 (-){6} > enqueue(연산결과), deletebuffer

 2 * 3 -[1 +]1 =
    ┌+
61(-){6} > enqueue, TryCalc(case 3)
  (+){6} > InitStruct, push
5 (+){5} > enqueue(연산결과), deletebuffer

 2 * 3 - 1 +[1 =]

51(+){5} > 피연산자 enqueue
  ( ){6} > ReturnResult(TryCalc), deletebuffer