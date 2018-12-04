 2 * 3 - 1 + 1 =

[2 *]3 - 1 + 1 =

2 (*){2} > (enqueue, push, deletebuffer) - EPD

 2 *[3 -]1 + 1 =

23(*){ } > enqueue, push(스택에 */가 있어서 TryCalc 호출)
  ( ){6} > InitStruct
6 (-){6} > deletebuffer

 2 * 3 -[1 +]1 =
    ┌+
61(-){6} > enqueue, push(스택이 꽉차서(스택에 */가 없어서) TryCalc 호출)
  ( ){5} > InitStruct
5 (+){5} > deletebuffer

 2 * 3 - 1 +[1 =]

51(+){5} > 피연산자 enqueue
  ( ){6} > ReturnResult(TryCalc), deletebuffer